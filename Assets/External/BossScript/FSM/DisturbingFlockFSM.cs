using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisturbingFlockFSM : MonoBehaviour
{ 

    public float reactionTime = 3f;
    
    public float switchTime = .5f;

    private FSM fsm;

    bool touched = false;

    Graph g;

    Node currNode=null;

    void Start()
    {

        // Define states and link actions when enter/exit/stay
        FSMState patrol = new FSMState();

        FSMState stayAway = new FSMState();

        patrol.enterActions.Add(StartAlarm);
        patrol.stayActions.Add(StayAlarm);
        patrol.exitActions.Add(ShutAlarm);

        stayAway.enterActions.Add(StartPause);
        stayAway.stayActions.Add(StayPause);
        stayAway.exitActions.Add(ShutPause);

        // Define transitions
        FSMTransition t1 = new FSMTransition(FlockTouched);
        FSMTransition t2 = new FSMTransition(PauseEspired);

        // Link states with transitions
        stayAway.AddTransition(t2, patrol);
        patrol.AddTransition(t1, stayAway);

        // Setup a FSA at initial state
        fsm = new FSM(stayAway);

        // Start monitoring
        StartCoroutine(Patrol());
    }

    void Update()
    {
        if (currNode!=null) transform.position = Vector3.Lerp(transform.position, currNode.sceneObject.transform.position + Vector3.up * (1.5f + currNode.height / 1.5f), .2f);
        else transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * Random.Range(-1,1) + Vector3.right * Random.Range(-1,1), .9f);

        if (((Input.touchCount == 1 && Input.GetTouch(0).phase == 0) || Input.GetMouseButtonDown(0)))
        {
            Ray ray;
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == 0) ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            else ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow, 100f);
            if (Physics.Raycast(ray, out hit))
            {
                if (!touched && hit.collider != null && hit.collider.gameObject == this.gameObject)
                {
                    StartCoroutine(Touched());
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

    private IEnumerator Touched()
    {
        touched = true;
        yield return new WaitForSeconds(5);
        touched = false;
    }
    // CONDITIONS

    public bool FlockTouched()
    {
        return touched;
    }

    public bool PauseEspired()
    {
        return !touched;
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
        Debug.Log(count +"  " +nodes[count]);

        return nodes[count];
    }
}