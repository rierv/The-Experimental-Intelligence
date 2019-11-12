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

    public Rigidbody secondRigidbody, thirdRigidbody, forthRigidbody, fifthRigidbody;

    void Start()
    {
        core = FindObjectOfType<JellyCore>();
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        rigidbody.drag = JellyCore.drag;
        baseRotation = rigidbody.rotation;
        coreRB = core.GetComponent<Rigidbody>();
        
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
                //coreRB.MovePosition(transform.position);
                //coreRB.AddRelativeForce((Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward));
                //rigidbody.MovePosition();
                //coreRB.useGravity = false;
                secondRigidbody.AddForce((Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward + Vector3.down / 2) * 50000 * Time.deltaTime);
                thirdRigidbody.AddForce((Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward + Vector3.down / 2) * 50000 * Time.deltaTime);
                forthRigidbody.AddForce((Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward + Vector3.down / 2) * 50000 * Time.deltaTime);
                fifthRigidbody.AddForce((Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward + Vector3.down / 2) * 50000 * Time.deltaTime);
                rigidbody.AddForce(Vector3.down * 500 * Time.deltaTime);

                rigidbody.AddForce((Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward + Vector3.down / 2) * 50000 * Time.deltaTime);
                core.transform.position = this.transform.position;
                coreRB.AddForce( Vector3.up * 50 );

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
