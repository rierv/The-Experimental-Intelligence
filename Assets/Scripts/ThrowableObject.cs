using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    JellyCore core;
    Rigidbody rigidbody;
    BoxCollider collider;
    Quaternion baseRotation;
    FlapperState state;

    SphereCollider coreCollider;
    Vector3 localPos;

    Vector3 lastGoodPosition;


    void Awake()
    {
        core = FindObjectOfType<JellyCore>();
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        rigidbody.drag = JellyCore.drag;
        baseRotation = rigidbody.rotation;
        
        
    }

    void FixedUpdate()
    {
        if (state == FlapperState.solid)
        {
            
                transform.position = core.transform.position + localPos;
                transform.rotation = core.transform.rotation;
            
        }
        else
        {
            if (state == FlapperState.gaseous)
            {
                rigidbody.AddForce(Physics.gravity * -JellyCore.gaseousAntiGravity);
            }
            Vector3 force = (core.transform.position - transform.position) * JellyCore.cohesion/2;
            force.y = Mathf.Clamp(force.y, Physics.gravity.y, float.MaxValue);
            rigidbody.AddForce(force);

            rigidbody.MoveRotation(baseRotation);
        }

        //rigidbody.drag = JellyCore.drag;
    }
}
