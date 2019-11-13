using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeRoot : MonoBehaviour
{

    public float RigidbodyMass = 1f;
    public float ColliderRadius = 0.0005f;
    public float ColliderHeight = 0.001f;
    public float JointSpring = 0.1f;
    public float JointDamper = 5f;
    public Vector3 RotationOffset;
    public Vector3 PositionOffset;

    protected List<Transform> CopySource;
    protected List<Transform> CopyDestination;
    protected static GameObject RigidBodyContainer;

    void Awake()
    {
        if (RigidBodyContainer == null)
            RigidBodyContainer = new GameObject("RopeRigidbodyContainer");
        CopySource = new List<Transform>();
        CopyDestination = new List<Transform>();
        //add children
        AddChildren(transform);
        AddHandle(transform.GetChild(transform.childCount-1));

        
        
        ThrowableObject th = CopySource[12].gameObject.AddComponent<ThrowableObject>();

        CopySource[9].gameObject.GetComponent<CapsuleCollider>().enabled = false;
        CopySource[10].gameObject.GetComponent<CapsuleCollider>().enabled = false;
        CopySource[11].gameObject.GetComponent<CapsuleCollider>().enabled = false;
        CopySource[12].gameObject.GetComponent<CapsuleCollider>().enabled = false;
        CopySource[13].gameObject.GetComponent<CapsuleCollider>().enabled = false;
        CopySource[14].gameObject.GetComponent<CapsuleCollider>().enabled = false;
        th.parentBodies = new List<Rigidbody>();
        foreach(Transform bone in CopySource)
        {
            
            th.parentBodies.Add(bone.gameObject.GetComponent<Rigidbody>());
        }
        Debug.Log(th.parentBodies.Count);
        th.enabled = false;
        th.isHandle = true;
        CopySource[12].gameObject.layer = 12;

        GameObject trigger = new GameObject();
        trigger.transform.parent = CopySource[12];
        CapsuleCollider sc = trigger.AddComponent<CapsuleCollider>();
        sc.radius *= 1.8f;
        sc.isTrigger = true;
        trigger.layer = 14;
        
    }

    private void AddChildren(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);
            var representative = new GameObject(child.gameObject.name);
            representative.transform.parent = RigidBodyContainer.transform;
            //rigidbody
            var childRigidbody = representative.gameObject.AddComponent<Rigidbody>();
            childRigidbody.useGravity = true;
            childRigidbody.isKinematic = false;
            childRigidbody.freezeRotation = true;
            childRigidbody.mass = RigidbodyMass;

            //collider
            var collider = representative.gameObject.AddComponent<CapsuleCollider>();
            collider.center = Vector3.zero;
            collider.radius = ColliderRadius;
            collider.height = ColliderHeight;
            //DistanceJoint
            var joint = representative.gameObject.AddComponent<RopeJoint>();
            joint.ConnectedRigidbody = parent;
            joint.DetermineDistanceOnStart = true;
            joint.Spring = JointSpring;
            joint.Damper = JointDamper;
            joint.DetermineDistanceOnStart = false;
            joint.Distance = Vector3.Distance(parent.position, child.position);

            /*joint = representative.gameObject.AddComponent<RopeJoint>();
            joint.ConnectedRigidbody = RigidBodyContainer.transform;
            joint.DetermineDistanceOnStart = true;
            joint.Spring = JointSpring;
            joint.Damper = JointDamper;
            joint.DetermineDistanceOnStart = false;
            joint.Distance = 0;

            if (child.childCount > 0&&child.parent.name!="Armature")
            {
                joint = representative.gameObject.AddComponent<RopeJoint>();
                joint.ConnectedRigidbody = child.GetChild(0);
                joint.DetermineDistanceOnStart = true;
                joint.Spring = JointSpring;
                joint.Damper = JointDamper*100000;
                joint.DetermineDistanceOnStart = false;
                joint.Distance = Vector3.Distance(child.GetChild(0).position, child.position);
            }*/
            //add copy source
            CopySource.Add(representative.transform);
            CopyDestination.Add(child);

            AddChildren(child);

            
        }
    }

    public void Update()
    {
        for (int i = 0; i < CopySource.Count; i++)
        {
            CopyDestination[i].position = CopySource[i].position + PositionOffset;
            CopyDestination[i].rotation = CopySource[i].rotation * Quaternion.Euler(RotationOffset);
        }
    }

    void AddHandle(Transform handle)
    {
        Debug.Log(handle.name);
    }
}