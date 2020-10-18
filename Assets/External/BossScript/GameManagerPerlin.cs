using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Xml.Serialization;

public class GameManagerPerlin : MonoBehaviour
{
    public Terrain terrain;
    public int heightLevels = 3;
    public GameObject Spawner, DisturbingSpawner;
    Node currentNode = null, previousNode=null;
    public enum Heuristics { Sight, Zero, Euclidian };
    public HeuristicFunction[] myHeuristics = { SightEstimator, ZeroEstimator, EuclideanEstimator };
    public Heuristics heuristicToUse = Heuristics.Sight;
    public int x = 100, y = 100, speed = 25, blocks = 5, RandomSeed = 0;
    [Range(0f, 1f)] public float edgeProbability = 0.75f;
    public float delay = 0.3f, acceleration = 0f, timeElapsed=0;
    public GameObject startMaterial = null, obstacleMaterial = null, endMaterial = null, boostMaterial = null, freezeMaterial = null;
    // what to put on the scene, not really meaningful
    List<Edge> totalPath = new List<Edge>();
    protected Node[,] matrix;
    public Graph g;
    bool done = false, boost = true, freeze = false, start = false, blockRegeneration = true, climbing = false;
    int boostCount = 3, freezeCount = 0;
    private int xStart = 1, yStart = 1, xEnd = 8, yEnd = 8;
    List<Node> boostList = new List<Node>(), freezeList = new List<Node>(), blockList = new List<Node>(), seenList = new List<Node>();
    float startingDelay;
    float[,] heightPerlin;
    TerrainData td;
    static Vector3 terrainSize;
    static Vector2 gridSize;
    bool sawTheEnd = false;
    bool thirdPersonView = false;
    GameObject myCamera;
    public GameObject pointer;
    Quaternion previousRotation, toRotation;
    Node lastEndPosition, currEndPosition;
    Color originaNpcColor;
    public float playerSpeed=3;
    public GameObject gyroPointer;
    Quaternion startRot, startRotGyro;
    Node lastBlockAllowedPosition;
    JumpButtonScript Jump_Trigger;
    Light RobotLight;
    public GameObject mySwitch;
    void Start()
    {

        if (!Input.gyro.enabled)
        {
            Input.gyro.enabled = true;
            startRot = gyroPointer.transform.rotation;
        }

        Jump_Trigger = GameObject.Find("Jump_Button").GetComponent<JumpButtonScript>();
        RandomSeed = (int)System.DateTime.Now.Ticks;
        Random.InitState(RandomSeed);
        
        

        terrain.gameObject.SetActive(true);
        td = terrain.terrainData;
        td.size = new Vector3(x, heightLevels-1, y);
        //terrain.transform.position -=  Vector3((x - 1) / 2, 0, (y - 1) / 2);
        //terrain.transform.position -= Vector3.right*.8f  + Vector3.forward*.8f ;
        terrain.GetComponent<PerlinTerrain>().Build();
        //Vector3 cameraPosition = terrain.transform.position + Vector3.up * Mathf.Max(x, y) * 2.2f + new Vector3((x - 1) / 2f, 0, 0) -Vector3.forward * x*1.3f;
        heightPerlin = terrain.GetComponent<PerlinTerrain>().GetH();

        startingDelay = delay / 2;
        // create a x * y matrix of nodes (and scene objects)
        // edge weight is now the geometric distance (gap)
        matrix = CreateGrid(x, y);
        //matrix = CreateGrid(sceneObject, x, y, gap);
        // create a graph and put random edges inside
        g = new Graph();
        CreateGraph(g, matrix);

        currentNode = matrix[xStart, yStart];
        previousNode = matrix[xStart, yStart];
        seenList.Add(currentNode);
        //seenGraph.AddNode(currentNode);
        //seenGraph.setConnections(currentNode, g.getConnections(currentNode));

        if (boost) insertSpecialBlocks(matrix, boostCount, 1, 1, boostMaterial, boostList);
        if (freeze) insertSpecialBlocks(matrix, freezeCount, 1 + heightLevels, 1 + heightLevels + 5, freezeMaterial, freezeList);

        myCamera = GameObject.Find("Main Camera");
        //myCamera.transform.position = cameraPosition;
        start = true;
        terrainSize = td.size;
        gridSize = new Vector2(x, y);
        startMaterial = Instantiate(startMaterial);
        startMaterial.transform.position = getNodePosition(matrix[xStart, yStart]) + Vector3.up*.5f;
        RobotLight = startMaterial.GetComponentInChildren<Light>();
        originaNpcColor = RobotLight.color;

        //endMaterial = Instantiate(endMaterial);

        endMaterial.transform.position = getNodePosition(matrix[xEnd, yEnd]);

        if (thirdPersonView)
        {
            pointer.transform.position = endMaterial.transform.position + Vector3.up;
            myCamera.transform.position = endMaterial.transform.position + Vector3.up;
            pointer.transform.LookAt(new Vector3(startMaterial.transform.position.x, endMaterial.transform.position.y, startMaterial.transform.position.z));
        }
        
        lastEndPosition = null;
        currEndPosition = null;
        lastBlockAllowedPosition = null;
        float h = 0;
        Node candidate=null;
        foreach (Node n in g.getNodes())
        {
            if (n.x>0&&n.x<x-1 && n.y>0&& n.y<y-1 && n.height >= h)
            {
                h = n.height;
                candidate = n;
            }
        }
        mySwitch.transform.position = getNodePosition(candidate) - Vector3.up*1.2f;

    }

    void FixedUpdate()
    {
        startMaterial.transform.position = Vector3.Lerp(getNodePosition(previousNode) + Vector3.up * .5f, getNodePosition(currentNode)+ Vector3.up*.5f, (timeElapsed)/(delay));
        Spawner.transform.position = startMaterial.transform.position + Vector3.up;
        Node nPosition = g.FindNear(endMaterial.transform.position.x, endMaterial.transform.position.z, endMaterial.transform.position.y, td.size.x / x, td.size.z / y, xEnd, yEnd);
        if (nPosition!=null&&(xEnd != nPosition.x || yEnd != nPosition.y))
        {
            lastBlockAllowedPosition = matrix[xEnd, yEnd];
            //removeNodeFromBlockList(matrix[xEnd, yEnd]);
            xEnd = nPosition.x;
            yEnd = nPosition.y;
            removeNodeFromBlockList(matrix[xEnd, yEnd]);
        }
        endMaterial.transform.position = Vector3.Lerp(endMaterial.transform.position, checkTerrainPosition()+Vector3.up, Time.fixedDeltaTime*5);
        gyroPointer.transform.rotation = startRotGyro*Input.gyro.attitude;
        if(currentNode!=null) {
            Vector3 aimedPos = getNodePosition(currentNode);
            aimedPos = new Vector3(aimedPos.x, aimedPos.y-1, aimedPos.z);
            Quaternion targetRotation = Quaternion.LookRotation(startMaterial.transform.position - aimedPos);
            targetRotation =  new Quaternion(targetRotation.x, -targetRotation.y, targetRotation.z, -targetRotation.w);
            startMaterial.transform.rotation = Quaternion.Lerp(startMaterial.transform.rotation, targetRotation, Time.fixedDeltaTime);
        } 
        /*if (thirdPersonView) {
            
            myCamera.transform.rotation= Quaternion.Lerp(myCamera.transform.rotation, pointer.transform.rotation, .4f);
            myCamera.transform.position = endMaterial.transform.position + Vector3.up - myCamera.transform.forward *3 ;
            if (Input.GetKey(KeyCode.Space)) endMaterial.transform.Translate((getNodePosition(matrix[xEnd, yEnd]) + myCamera.transform.forward*playerSpeed - endMaterial.transform.position )*Time.fixedDeltaTime);
            
            if (Input.GetKey(KeyCode.LeftArrow)) {
                pointer.transform.Rotate(0, -1f,0);
            }
            if (Input.GetKey(KeyCode.RightArrow)) {
                pointer.transform.Rotate(0, 1f, 0);
            }
            if (Input.gyro.attitude != Quaternion.identity && gyroPointer.transform.up.y > .3f && gyroPointer.transform.forward.z > .7f)
            {
                if (gyroPointer.transform.up.z < -.2f ) endMaterial.transform.Translate((getNodePosition(matrix[xEnd, yEnd]) + myCamera.transform.forward * playerSpeed - endMaterial.transform.position) * Time.fixedDeltaTime);
                if (gyroPointer.transform.up.z > .2f) endMaterial.transform.Translate((getNodePosition(matrix[xEnd, yEnd]) - myCamera.transform.forward * playerSpeed - endMaterial.transform.position) * Time.fixedDeltaTime);
                if (gyroPointer.transform.forward.x < -.2f) pointer.transform.Rotate(Vector3.up);
                if (gyroPointer.transform.forward.x > .2f) pointer.transform.Rotate(-Vector3.up);

                
            }

        }
        else
        {
            if (Input.gyro.attitude != Quaternion.identity && gyroPointer.transform.up.y > .3f && gyroPointer.transform.forward.z > .7f)
            {
                //endMaterial.transform.Translate((getNodePosition(matrix[xEnd, yEnd]) + new Vector3(Mathf.Clamp((correctedQuaternion.eulerAngles.x - 180) * 5, -2, 2), 0, Mathf.Clamp((correctedQuaternion.eulerAngles.y - 180) * 5, -2, 2)) - endMaterial.transform.position) * Time.fixedDeltaTime);
                //endMaterial.transform.position = Vector3.Lerp(endMaterial.transform.position, getNodePosition(matrix[xEnd, yEnd]) + new Vector3(Mathf.Clamp(Input.gyro.attitude.x*5, -3, 3), 0, Mathf.Clamp(Input.gyro.attitude.y*5, -3, 3)), .1f);
                if (gyroPointer.transform.up.z < -.2f) endMaterial.transform.Translate((getNodePosition(matrix[xEnd, yEnd]) + new Vector3(0, 0, playerSpeed) - endMaterial.transform.position) * Time.fixedDeltaTime);
                if (gyroPointer.transform.up.z > .2f) endMaterial.transform.Translate((getNodePosition(matrix[xEnd, yEnd]) + new Vector3(0, 0, -playerSpeed) - endMaterial.transform.position) * Time.fixedDeltaTime);

                if (gyroPointer.transform.forward.x > .2f) endMaterial.transform.Translate((getNodePosition(matrix[xEnd, yEnd]) + new Vector3(playerSpeed, 0, 0) - endMaterial.transform.position) * Time.fixedDeltaTime);
                if (gyroPointer.transform.forward.x < -.2f) endMaterial.transform.Translate((getNodePosition(matrix[xEnd, yEnd]) + new Vector3(-playerSpeed, 0, 0) - endMaterial.transform.position) * Time.fixedDeltaTime);

            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                endMaterial.transform.position = Vector3.Lerp(endMaterial.transform.position, getNodePosition(matrix[xEnd, yEnd]) - Vector3.right, Time.fixedDeltaTime);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                endMaterial.transform.position = Vector3.Lerp(endMaterial.transform.position, getNodePosition(matrix[xEnd, yEnd]) + Vector3.right, Time.fixedDeltaTime);
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                endMaterial.transform.position = Vector3.Lerp(endMaterial.transform.position, getNodePosition(matrix[xEnd, yEnd]) + Vector3.forward, Time.fixedDeltaTime);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                endMaterial.transform.position = Vector3.Lerp(endMaterial.transform.position, getNodePosition(matrix[xEnd, yEnd]) - Vector3.forward, Time.fixedDeltaTime);
            }
        }  */

        timeElapsed += Time.fixedDeltaTime;

        if (start && ((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {

            StartCoroutine(AnimateSolution());
            start = false;
            startRotGyro = Quaternion.Inverse(Input.gyro.attitude);
        }
        else if (Jump_Trigger.jumpButtonHold)
        {
            Node n = matrix[xEnd, yEnd];
            if (n != null && n!=currentNode)
            { 
                if ((!boost || !boostList.Contains(n)) && (!freeze || !freezeList.Contains(n)) && !blockList.Contains(n) && n != currentNode)
                {
                    endMaterial.transform.position = Vector3.Lerp(endMaterial.transform.position, checkTerrainPosition() + Vector3.up*2, Time.fixedDeltaTime * 5);
                    g.RemoveNodeConnections(n);
                    blockList.Add(n);
                    if (seenList.Contains(n)) seenList.Remove(n);
                }
                else if (!blockRegeneration)
                {
                    //removeNodeFromBlockList(n);
                }
            }
            //}
        }
        
    }

    

    private IEnumerator AnimateSolution()
    {
        Edge[] path = null;
        int count = 0;
        while (!done)
        {
            if (path != null && (count<path.Length|| path.Length==0))
            {
                if (path.Length == 0)
                {
                    /*EndButton.SetActive(true);
                    EndButton.GetComponentInChildren<Text>().text = "Sorry, No solution left\nScore: " + Score.text;
                    done = true;*/
                    yield return new WaitForSeconds(.1f);
                    if (currentNode != matrix[xEnd, yEnd])
                    {
                        Node target = bestNodeinSight();
                        if(target!=null) path = AStarSolver.Solve(g, currentNode, target, myHeuristics[(int)Heuristics.Sight]);
                    }
                    else
                    {
                        sawTheEnd = true;
                        path = null;
                    }
                }
                else
                {

                    delay = startingDelay*((  ((path[count].weight - heightLevels + 1) / 2)) / (1 + acceleration))/1.5f;
                    acceleration *= 1.1f;

                    if (path[count].to == matrix[xEnd, yEnd])
                    {
                        /*
                        EndButton.SetActive(true);
                        EndButton.GetComponentInChildren<Text>().text = "Sorry, End of the Run\nScore: " + Score.text;
                        done = true;
                        */
                        yield return new WaitForSeconds(1f);

                        sawTheEnd = true;
                        path = null;
                    }
                    
                    else if (boostList.Contains(path[count].to))
                    {
                        StartCoroutine(ChangeOfSpeedCoroutine(1, 1.6f));
                        boostList.Remove(path[count].to);
                    }
                    else if (freezeList.Contains(path[count].to))
                    {
                        StartCoroutine(ChangeOfSpeedCoroutine(2, 1 / 1.6f));
                        freezeList.Remove(path[count].to);
                    }

                    if (path!=null && !blockList.Contains(path[count].to))
                    {
                        totalPath.Add(path[count]);
                        previousNode = currentNode;
                        currentNode = path[count].to;
                        timeElapsed = 0;
                        Debug.Log(isHit(currentNode, matrix[xEnd, yEnd]));
                        nodeDiscover();
                        if (isPlayerOnSight())
                        {
                            RobotLight.color = Color.red;
                            lastEndPosition = matrix[xEnd, yEnd];
                            if (currEndPosition == null || Vector3.Distance(getNodePosition(currEndPosition), getNodePosition(lastEndPosition))>1.5f)
                            {
                                sawTheEnd = true;
                                path = null;
                            }
                        }
                        else if (currEndPosition!= null)
                        {
                            path = null;
                        }
                        yield return new WaitForSeconds(delay);

                    }
                    path = checkPath(path);
                    
                    count++;
                }
            }
            else if (isPlayerOnSight() || sawTheEnd)
            {
                //removeNodeFromBlockList(currentNode);
                //removeNodeFromBlockList(matrix[xEnd, yEnd]);
                currEndPosition = matrix[xEnd, yEnd];
                lastEndPosition = matrix[xEnd, yEnd];
                sawTheEnd = false;
                if(currentNode!=currEndPosition) path = AStarSolver.Solve(g, currentNode, currEndPosition, myHeuristics[(int)Heuristics.Sight]);
                else yield return new WaitForSeconds(1f);
                RobotLight.color = Color.red;
                count = 0;
                Debug.DrawRay(getNodePosition(currentNode) +Vector3.up, getNodePosition(matrix[xEnd, yEnd]) - getNodePosition(currentNode), Color.white, 20);
            }
            else
            {
                Node target;
                if (currEndPosition != null) lastEndPosition = matrix[xEnd, yEnd];
                currEndPosition = null;
                //removeNodeFromBlockList(target);
                //removeNodeFromBlockList(currentNode);
                RobotLight.color = originaNpcColor;
                if (currentNode != matrix[xEnd, yEnd])
                {
                    target = bestNodeinSight();
                    if (target != null) path = AStarSolver.Solve(g, currentNode, target, myHeuristics[(int)Heuristics.Sight]);
                }
                else yield return new WaitForSeconds(.1f);
                yield return new WaitForSeconds(.1f);
                count = 0;
            }
        }
    }

    public void Restart()
    {
        //Scenes.Load("MainMenu");

    }

    protected virtual Node[,] CreateGrid(int x, int y)
    {
        Node[,] matrix = new Node[x, y];
        for (int i = 0; i < x; i += 1)
        {
            for (int j = 0; j < y; j += 1)
            {
                matrix[i, j] = new Node(i, j);

                matrix[i, j].height = heightPerlin[(int)(j * (heightPerlin.GetLength(1) / y)), (int)(i * (heightPerlin.GetLength(0) / x))]*(heightLevels);

            }
        }
        return matrix;
    }

    protected void CreateGraph(Graph g, Node[,] crossings)
    {
        for (int i = 0; i < crossings.GetLength(0); i += 1)
        {
            for (int j = 0; j < crossings.GetLength(1); j += 1)
            {
                g.AddNode(crossings[i, j]);
                foreach (Edge e in RandomEdges(crossings, i, j, 1))
                    g.AddEdge(e);
            }
        }
    }

    protected Edge[] RandomEdges(Node[,] matrix, int x, int y, float threshold)
    {
        List<Edge> result = new List<Edge>();
        if (x != 0 && Random.Range(0f, 1f) <= threshold)
            result.Add(new Edge(matrix[x, y], matrix[x - 1, y], Distance(matrix[x, y], matrix[x - 1, y])));

        if (y != 0 && Random.Range(0f, 1f) <= threshold)
            result.Add(new Edge(matrix[x, y], matrix[x, y - 1], Distance(matrix[x, y], matrix[x, y - 1])));

        if (x != (matrix.GetLength(0) - 1) && Random.Range(0f, 1f) <= threshold)
            result.Add(new Edge(matrix[x, y], matrix[x + 1, y], Distance(matrix[x, y], matrix[x + 1, y])));

        if (y != (matrix.GetLength(1) - 1) && Random.Range(0f, 1f) <= threshold)
            result.Add(new Edge(matrix[x, y], matrix[x, y + 1], Distance(matrix[x, y], matrix[x, y + 1])));

        return result.ToArray();
    }

    protected float Distance(Node from, Node to)
    {
        return to.height - from.height + 1 + heightLevels;
    }

    void insertSpecialBlocks(Node[,] matrix, int blockCount, int height, int weight, GameObject go, List<Node> specialList)
    {
        int stop = 0;
        while (blockCount > 0 && stop < 500)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (Random.Range(0f, 1f) < 0.005 && blockCount > 0 && !freezeList.Contains(matrix[i, j]) && !boostList.Contains(matrix[i, j]) && (i != xStart || j != yStart) && (i != xEnd || j != yEnd))
                    {
                        bool good = true;
                        foreach (Edge e in g.getConnections(matrix[i, j]))
                            if (freezeList.Contains(e.to) || boostList.Contains(e.to)) good = false;
                        if (good)
                        {
                            GameObject newgo = Instantiate(go);
                            newgo.transform.position = new Vector3(i * (td.size.x / x), matrix[i, j].height, j * (td.size.z / y));
                            //matrix[i, j].height = height;
                            blockCount--;
                            specialList.Add(matrix[i, j]);
                            g.changeWeight(matrix[i, j].description, weight);
                        }
                    }
                }

            }
            stop++;
        }
    }

    private IEnumerator ChangeOfSpeedCoroutine(float time, float coefficient)
    {
        startingDelay = startingDelay / coefficient;
        yield return new WaitForSeconds(time);
        startingDelay = startingDelay * coefficient;
    }

    public void AddNodeConnections(Node n, Node[,] crossings, List<Node> blockList)
    {

        if (n.x > 0 && !blockList.Contains(crossings[n.x - 1, n.y]))
        {
            g.AddEdge(new Edge(crossings[n.x - 1, n.y], n, Distance(crossings[n.x - 1, n.y], crossings[n.x, n.y])));
            g.AddEdge(new Edge(n, crossings[n.x - 1, n.y], Distance(crossings[n.x, n.y], crossings[n.x - 1, n.y])));
        }
        if (n.y > 0 && !blockList.Contains(crossings[n.x, n.y - 1]))
        {
            g.AddEdge(new Edge(crossings[n.x, n.y - 1], n, Distance(crossings[n.x, n.y - 1], crossings[n.x, n.y])));
            g.AddEdge(new Edge(n, crossings[n.x, n.y - 1], Distance(crossings[n.x, n.y], crossings[n.x, n.y - 1])));
        }
        if (n.x < crossings.GetLength(0) - 1 && !blockList.Contains(crossings[n.x + 1, n.y]))
        {
            g.AddEdge(new Edge(crossings[n.x + 1, n.y], n, Distance(crossings[n.x + 1, n.y], crossings[n.x, n.y])));
            g.AddEdge(new Edge(n, crossings[n.x + 1, n.y], Distance(crossings[n.x, n.y], crossings[n.x + 1, n.y])));
        }
        if (n.y < crossings.GetLength(1) - 1 && !blockList.Contains(crossings[n.x, n.y + 1]))
        {
            g.AddEdge(new Edge(crossings[n.x, n.y + 1], n, Distance(crossings[n.x, n.y + 1], crossings[n.x, n.y])));
            g.AddEdge(new Edge(n, crossings[n.x, n.y + 1], Distance(crossings[n.x, n.y], crossings[n.x, n.y + 1])));
        }

    }

    protected static float EuclideanEstimator(Node from, Node to)
    {
        return (getNodePosition(from) - getNodePosition(to)).magnitude;
    }

    protected static float SightEstimator(Node from, Node to) {
        if (isHit(from, to)) return .9f;
        else return .1f;
    }

    protected static float ZeroEstimator(Node from, Node to) { return 0f; }


    static Vector3 getNodePosition(Node n)
    {
        return new Vector3(n.x * Mathf.Floor((terrainSize.x / gridSize.x)), n.height, n.y * Mathf.Floor((terrainSize.z / gridSize.y)));
    }

    static bool isHit (Node currNode, Node nodeToHit)
    {
        RaycastHit hit;
        if (Physics.Raycast(getNodePosition(currNode) + Vector3.up, (getNodePosition(nodeToHit) - getNodePosition(currNode)).normalized, out hit, Mathf.Infinity)&& hit.collider != null) {
             if (Vector3.Distance(hit.point, getNodePosition(nodeToHit)) < 1.5f && hit.normal.y > -.1f) return true;
        }
        return false;
    }
    Edge [] checkPath(Edge[] path)
    {
        if (path != null)
        {
            foreach (Edge e in path)
            {
                if (blockList.Contains(e.to)) path = null;
            }
        }
        return path;
    }
    bool nodeDiscover()
    {

        bool newNodesFound = false;
        foreach(Node n in g.getNodes())
        {
            //Debug.DrawRay(getNodePosition(currentNode) + Vector3.up, getNodePosition(n) - getNodePosition(currentNode), Color.white, 10);

            if (!blockList.Contains(n) && isHit(currentNode, n))
            {
                seenList.Add(n);
                newNodesFound = true;
            }
            else seenList.Remove(n);
        }
        foreach (Edge e in g.getConnections(currentNode))
        {
            if (!blockList.Contains(e.to) && !seenList.Contains(e.to)) seenList.Add(e.to);
        }
        //seenGraph.setConnections();
        return newNodesFound;
    }
    Node bestNodeinSight()
    {
        Node candidate=null;
        nodeDiscover();
        float maxDistance=0;

        if (lastEndPosition != null)
        {
            candidate = lastEndPosition;
            lastEndPosition = null;
        }
        else
        {
            foreach (Node n in seenList)
            {
                if (Vector3.Distance(getNodePosition(n), getNodePosition(currentNode)) > maxDistance)
                {
                    maxDistance = Vector3.Distance(getNodePosition(n), getNodePosition(currentNode));
                    candidate = n;
                }
            }
            if (candidate == null) candidate = g.getConnections(currentNode)[0].to;
        }
        if (blockList.Contains(candidate))
        {
            candidate = nearestAviablePosition(candidate);
        }
        return candidate;
    }
    bool isPlayerOnSight()
    {
        RaycastHit hit;
        if (Physics.Raycast(startMaterial.transform.position, (endMaterial.transform.position -startMaterial.transform.position).normalized, out hit, Mathf.Infinity) && hit.collider != null)
        {
            if(Vector3.Distance(hit.collider.gameObject.transform.position, endMaterial.transform.position)<3) return true;
        }
        if (Physics.Raycast(startMaterial.transform.position + Vector3.up, (endMaterial.transform.position - startMaterial.transform.position).normalized, out hit, Mathf.Infinity) && hit.collider != null)
        {
            if (Vector3.Distance(hit.collider.gameObject.transform.position, endMaterial.transform.position) < 3) return true;
        }
        if (Physics.Raycast(startMaterial.transform.position + Vector3.up+Vector3.right, (endMaterial.transform.position - startMaterial.transform.position).normalized, out hit, Mathf.Infinity) && hit.collider != null)
        {
            if (Vector3.Distance(hit.collider.gameObject.transform.position, endMaterial.transform.position) < 3) return true;
        }
        if (Physics.Raycast(startMaterial.transform.position + Vector3.up + Vector3.forward, (endMaterial.transform.position - startMaterial.transform.position).normalized, out hit, Mathf.Infinity) && hit.collider != null)
        {
            if (Vector3.Distance(hit.collider.gameObject.transform.position, endMaterial.transform.position) < 3) return true;
        }
        if (Physics.Raycast(startMaterial.transform.position + Vector3.up - Vector3.right, (endMaterial.transform.position - startMaterial.transform.position).normalized, out hit, Mathf.Infinity) && hit.collider != null)
        {
            if (Vector3.Distance(hit.collider.gameObject.transform.position, endMaterial.transform.position) < 3) return true;
        }
        if (Physics.Raycast(startMaterial.transform.position + Vector3.up - Vector3.forward, (endMaterial.transform.position - startMaterial.transform.position).normalized, out hit, Mathf.Infinity) && hit.collider != null)
        {
            if (Vector3.Distance(hit.collider.gameObject.transform.position, endMaterial.transform.position) < 3) return true;
        }
        return false;
    }
    Vector3 checkTerrainPosition()
    {
        int layerMask = 1 << 8;
        RaycastHit hit;
        if (Physics.Raycast(endMaterial.transform.position + Vector3.right, endMaterial.transform.position + Vector3.right + Vector3.up, out hit, Mathf.Infinity, layerMask) && hit.collider != null)
        {
            Debug.Log("Puppete");
            if (hit.collider.gameObject == terrain) return hit.point + Vector3.up * 2f;
        }
        if (Physics.Raycast(endMaterial.transform.position - Vector3.right, endMaterial.transform.position - Vector3.right - Vector3.up, out hit, Mathf.Infinity, layerMask) && hit.collider != null)
        {
            Debug.Log("Puppete2");
            if (hit.collider.gameObject == terrain) return hit.point + Vector3.up * 2f;
        }
        if (Physics.Raycast(endMaterial.transform.position - Vector3.forward, endMaterial.transform.position - Vector3.forward - Vector3.up, out hit, Mathf.Infinity, layerMask) && hit.collider != null)
        {
            Debug.Log("Puppete2");
            if (hit.collider.gameObject == terrain) return hit.point + Vector3.up * 2f;
        }
        if (Physics.Raycast(endMaterial.transform.position + Vector3.forward, endMaterial.transform.position + Vector3.forward - Vector3.up, out hit, Mathf.Infinity, layerMask) && hit.collider != null)
        {
            Debug.Log("Puppete2");
            if (hit.collider.gameObject == terrain) return hit.point + Vector3.up*2f;
        }

        return new Vector3 (endMaterial.transform.position.x, getNodePosition(matrix[xEnd,yEnd]).y, endMaterial.transform.position.z);
    }
    void removeNodeFromBlockList(Node n)
    {
        if (blockList.Contains(n))
        {
            AddNodeConnections(n, matrix, blockList);
            blockList.Remove(n);
        }
    }
    Node nearestAviablePosition(Node candidate)
    {
        Node tmp = null;
        float min = 0;
        if (blockList.Contains(candidate)) AddNodeConnections(candidate, matrix, blockList);
        foreach(Edge e in g.getConnections(candidate))
        {
            if (!blockList.Contains(e.to) && e.to.height > min)
            {
                min = e.to.height;
                tmp = e.to;
            }
        }
        if(blockList.Contains(candidate)) g.RemoveNodeConnections(candidate);

        return tmp;
    }

}