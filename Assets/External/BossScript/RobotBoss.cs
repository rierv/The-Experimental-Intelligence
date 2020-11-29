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
        Debug.Log("ZAGABRIA" + transform.position);
        if(Mathf.Abs(transform.position.x- Core.transform.position.x)<.04f&& Mathf.Abs(transform.position.z - Core.transform.position.z) < .04f)
        {
            if (Core.GetComponent<PlayerMove>().canMove == false)
            {

                Core.GetComponent<ThrowObjects>().ThrowObject();
            }
            Core.GetComponent<Rigidbody>().AddForce((Vector3.up-transform.forward*.4f-transform.up)*2.5f, ForceMode.VelocityChange);
        }
    }
    
}
