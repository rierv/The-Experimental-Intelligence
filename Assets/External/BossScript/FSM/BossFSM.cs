using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFSM : MonoBehaviour
{

    public float reactionTime = 3f;

    public float switchTime = .5f;

    private FSM fsm;

    bool followingPlayer = false;

    Graph g;

    Node currNode = null;

    void Start()
    {

        // Define states and link actions when enter/exit/stay
        FSMState patrol = new FSMState();


        FSMState followingFlapper = new FSMState();

        patrol.enterActions.Add(StartAlarm);
        patrol.stayActions.Add(StayAlarm);
        patrol.exitActions.Add(ShutAlarm);


        followingFlapper.enterActions.Add(StartPause);
        followingFlapper.stayActions.Add(StayPause);
        followingFlapper.exitActions.Add(ShutPause);

        // Define transitions
        FSMTransition t1 = new FSMTransition(FlapperOnSight);
        FSMTransition t2 = new FSMTransition(FlapperNotOnSight);

        // Link states with transitions
        followingFlapper.AddTransition(t2, patrol);
        patrol.AddTransition(t1, followingFlapper);

        // Setup a FSA at initial state
        fsm = new FSM(patrol);

        // Start monitoring
        StartCoroutine(Patrol());
    }

    void Update()
    {
        if (currNode != null) transform.position = Vector3.Lerp(transform.position, currNode.sceneObject.transform.position + Vector3.up * (1.5f + currNode.height / 1.5f), .2f);
        else transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * Random.Range(-1, 1) + Vector3.right * Random.Range(-1, 1), .9f);

        if (((Input.touchCount == 1 && Input.GetTouch(0).phase == 0) || Input.GetMouseButtonDown(0)))
        {
            Ray ray;
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == 0) ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            else ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow, 100f);
            if (Physics.Raycast(ray, out hit))
            {
                if (!followingPlayer && hit.collider != null && hit.collider.gameObject == this.gameObject)
                {
                    StartCoroutine(FollowingPlayer());
                }
            }
        }
    }


    // Periodic update, run forever
    public IEnumerator Patrol()
    {
        while (true)
        {
            fsm.Update();
            yield return new WaitForSeconds(reactionTime);
        }

    }

    private IEnumerator FollowingPlayer()
    {
        followingPlayer = true;
        while(isPlayerOnSight())
            yield return new WaitForSeconds(5);
        followingPlayer = false;
    }

    private bool isPlayerOnSight()
    {
        return true;
    }

    // CONDITIONS

    public bool FlapperOnSight()
    {
        return GetComponent<GameManagerPerlin>().isPlayerOnSight();
    }

    public bool FlapperNotOnSight()
    {
        return !GetComponent<GameManagerPerlin>().isPlayerOnSight();
    }

    // ACTIONS

    public void StartAlarm()
    {
        GetComponent<Collider>().enabled = true;
        currNode = getRandomNode();
    }

    public void ShutAlarm()
    {

    }

    public void StayAlarm()
    {
        if ((int)Mathf.Floor((Time.realtimeSinceStartup - 0.5f) / switchTime) % 4 == 0)
        {
            currNode = getRandomNode();
        }
    }
    public void StartPause()
    {
        GetComponent<Collider>().enabled = false;
        currNode = null;
    }

    public void ShutPause()
    {
    }

    public void StayPause()
    {

    }

    Node getRandomNode()
    {
        g = GameObject.Find("GameManager").GetComponent<GameManagerPerlin>().g;
        int count = 0;
        Node[] nodes = g.getNodes();
        while (Random.Range(0f, 1f) > 0.005) count = (count + 1) % nodes.Length;
        Debug.Log(count + "  " + nodes[count]);

        return nodes[count];
    }
}
