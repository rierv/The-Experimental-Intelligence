using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeRoot : MonoBehaviour
{

    public float RigidbodyMass = 1f;
    public float ColliderRadius = 0.001f;
    public float JointSpring = 0.009f;
    public float JointDamper = 0.01f;
    public Vector3 RotationOffset;
    public Vector3 PositionOffset;

    protected List<Transform> CopySource;
    protected List<Transform> CopyDestination;
    public GameObject RigidBodyContainer;

    void Awake()
    {
        

        CopySource = new List<Transform>();
        CopyDestination = new List<Transform>();
        Debug.Log("cippala");
        //add children
        AddChildren(transform);
    }

    private void AddChildren(Transform parent)
    {
        RopeJoint preJoint = null ;
        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);
            var representative = child.gameObject;

            representative.transform.parent = RigidBodyContainer.transform;
            //rigidbody
            var childRigidbody = representative.gameObject.GetComponent<Rigidbody>();
            childRigidbody.useGravity = true;
            childRigidbody.isKinematic = false;
            childRigidbody.freezeRotation = true;
            childRigidbody.mass = RigidbodyMass;

            //collider
            var collider = representative.gameObject.AddComponent<SphereCollider>();
            collider.center = Vector3.zero;
            collider.radius = ColliderRadius;

            //DistanceJoint
            var joint = representative.gameObject.AddComponent<RopeJoint>();
            
            joint.ConnectedRigidbody = parent;
            joint.DetermineDistanceOnStart = true;
            joint.Spring = JointSpring;
            joint.Damper = JointDamper;
            joint.DetermineDistanceOnStart = false;
            joint.Distance = Vector3.Distance(parent.position, child.position);
            preJoint = joint;
            representative.gameObject.AddComponent<FrictionJoint>();
            //add copy source
            CopySource.Add(representative.transform);
            CopyDestination.Add(child);

            AddChildren(child);

            if (preJoint)
            {
                Debug.Log(preJoint);

                preJoint.postJoint = joint;
                Debug.Log("aaaa");
            }
            preJoint = joint;
        }
    }

    public void Update()
    {
        for (int i = 0; i < CopySource.Count; i++)
        {
            //CopyDestination[i].position = CopySource[i].position + PositionOffset;
            //CopyDestination[i].rotation = CopySource[i].rotation * Quaternion.Euler(RotationOffset);
        }
    }
}