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
    public float SpineStrenght = 50000f;
    public List<Rigidbody> parentBodies;
    public float handle_hight=1f;
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

                foreach (Rigidbody bone in parentBodies)
                {
                    bone.AddForce((Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward + Vector3.down / 2) * (SpineStrenght / ((coreRB.transform.position - startPos).magnitude + 1)) * Time.deltaTime);


                }
                core.transform.position = new Vector3(core.transform.position.x, this.transform.position.y*handle_hight, core.transform.position.z);
                //coreRB.AddForce( Vector3.up * 50 );
                //rigidbody.AddForce(Vector3.down * 500 * Time.deltaTime);

            }
            else
            {
                Vector3 force = (core.transform.position - transform.position) * JellyCore.cohesion * 5;
                force.y = Mathf.Clamp(force.y, Physics.gravity.y, float.MaxValue);
                rigidbody.AddForce(force);

                rigidbody.MoveRotation(baseRotation);
            }
        }
        
    }
}
