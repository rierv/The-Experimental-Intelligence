using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBoss : MonoBehaviour
{
    GameObject Core;
    // Start is called before the first frame update
    void Start()
    {
        Core = GameObject.Find("CORE");
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, Core.transform.position) < 2.5f)
        {
            if (Core.GetComponent<PlayerMove>().canMove == false)
            {

                Core.GetComponent<ThrowObjects>().ThrowObject();
            }
            Core.GetComponent<Rigidbody>().AddForce((Vector3.up-transform.forward-transform.up) * 13000);
        }
    }
    
}
