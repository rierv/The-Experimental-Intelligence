using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVMovement : MonoBehaviour
{
    public float speed = 1;
    public float aplitude = 1;
    GameObject core;
    // Start is called before the first frame update
    void Start()
    {
        core = GameObject.Find("CORE");
        Debug.Log(transform.rotation.eulerAngles);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(transform.position.x, transform.position .y+ Mathf.Sin(Time.time/aplitude), transform.position.z), speed*Time.deltaTime);
        this.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.EulerAngles(0, -core.transform.position.x*.05f, 0), Time.deltaTime);
    }
}
