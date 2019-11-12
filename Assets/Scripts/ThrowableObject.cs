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

    public bool isHandle;
    Rigidbody coreRB;

    public List<Rigidbody> parentBodies;
    Vector3 startPos;
    private void Awake()
    {
        parentBodies = new List<Rigidbody>();

    }
    void Start()
    {
        core = FindObjectOfType<JellyCore>();
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        rigidbody.drag = JellyCore.drag;
        baseRotation = rigidbody.rotation;
        coreRB = core.GetComponent<Rigidbody>();
        startPos = transform.position;
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
            
            
            if (isHandle)
            {
                
                foreach (Rigidbody bone in parentBodies) bone.AddForce((Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward + Vector3.down / 2) * (200000 / ((coreRB.transform.position-startPos).magnitude+1)) * Time.deltaTime);

                core.transform.position = this.transform.position;
                coreRB.AddForce( Vector3.up * 50 );
                rigidbody.AddForce(Vector3.down * 500 * Time.deltaTime);

            }
            else
            {
                Vector3 force = (core.transform.position - transform.position) * JellyCore.cohesion * 5;
                force.y = Mathf.Clamp(force.y, Physics.gravity.y, float.MaxValue);
                rigidbody.AddForce(force);

                rigidbody.MoveRotation(baseRotation);
            }
        }
        
        //rigidbody.drag = JellyCore.drag;
    }
}
