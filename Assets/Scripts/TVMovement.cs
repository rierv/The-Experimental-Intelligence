using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVMovement : MonoBehaviour
{
    public float speed = 1;
    public float aplitude = 1;
    float time=0;
    GameObject core;
    // Start is called before the first frame update
    void Start()
    {
        //core = GameObject.Find("CORE");
        Debug.Log(transform.rotation.eulerAngles);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.deltaTime;
        this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(transform.position.x, transform.position .y+ Mathf.Sin(time)*aplitude, transform.position.z), speed*Time.fixedDeltaTime);
        //this.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.EulerAngles(0, -(core.transform.position.x - core.transform.parent.transform.position.x)*3, 0), Time.deltaTime);
    }
}
