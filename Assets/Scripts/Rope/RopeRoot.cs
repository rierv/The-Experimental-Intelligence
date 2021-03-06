﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeRoot : MonoBehaviour
{
    public GameObject throwableModel;
    public float RigidbodyMass = 0.1f;
    public float ColliderRadius = 0.5f;
    public float ColliderHeight = 1f;
    public float JointSpring = 0f;
    public float JointDamper = 1f;
    public Vector3 RotationOffset;
    public Vector3 PositionOffset;
    public float spineStrenght = 10;
    public float exitForce = 1f;
    public float rotationSpeed=1f;
    public float movementSpeed = 1f;
    protected List<Transform> CopySource;
    protected List<Transform> CopyDestination;
    protected static GameObject RigidBodyContainer;
    ThrowableObject th;
    bool bonesActive = true;
    public int activeBone = 4;
    void Awake()
    {
        if (RigidBodyContainer == null)
            RigidBodyContainer = new GameObject("RopeRigidbodyContainer");
        RigidBodyContainer.transform.position = transform.position;
        CopySource = new List<Transform>();
        CopyDestination = new List<Transform>();
        //add children
        AddChildren(transform);
        AddHandle(transform.GetChild(transform.childCount-1));

        
        
        th = CopySource[activeBone].gameObject.AddComponent<ThrowableObject>();
        th.SpineStrenght = spineStrenght;
        CopySource[activeBone].gameObject.GetComponent<CapsuleCollider>().enabled = false;
        CopySource[0].gameObject.GetComponent<CapsuleCollider>().enabled = false;
        CopySource[1].gameObject.GetComponent<CapsuleCollider>().enabled = false;
        th.parentBodies = new List<Rigidbody>();
        foreach(Transform bone in CopySource)
        {
            
            th.parentBodies.Add(bone.gameObject.GetComponent<Rigidbody>());
        }
        th.enabled = false;
        th.isHandle = true;
        CopySource[activeBone].gameObject.layer = 12;
        GameObject tM = Instantiate(throwableModel);
        tM.layer = 12;
        tM.transform.parent = CopySource[activeBone];
        tM.transform.localPosition = Vector3.up;
        GameObject trigger = new GameObject();
        trigger.transform.position = CopySource[activeBone].position;
        trigger.transform.parent = CopySource[activeBone];
        CapsuleCollider sc = trigger.AddComponent<CapsuleCollider>();
        sc.radius *= 3f;
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
            representative.transform.position = child.transform.position;
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
            joint.ParentTransform = parent;
            joint.DetermineDistanceOnStart = true;
            joint.Spring = JointSpring;
            joint.Damper = JointDamper;
            joint.DetermineDistanceOnStart = false;
            joint.movementSpeed = movementSpeed;
            joint.rotationSpeed = rotationSpeed;
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
        if (th.isActiveAndEnabled)
        {
            if (bonesActive)
            {
                SphereCollider[] bones = GameObject.Find("Root").GetComponentsInChildren<SphereCollider>();
                GameObject.Find("CORE").GetComponent<Rigidbody>().isKinematic = true;
                /*foreach (SphereCollider bone in bones)
                {
                    bone.enabled = false;
                    bone.GetComponent<JellyBone>().enabled = false;
                    bone.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                }*/
                bonesActive = false;
            }
        }
        else if (!bonesActive)
        {
            SphereCollider[] bones = GameObject.Find("Root").GetComponentsInChildren<SphereCollider>();
            Rigidbody core = GameObject.Find("CORE").GetComponent<Rigidbody>();
            core.isKinematic = false;
            core.AddForce(Vector3.up * exitForce* 2000 + (Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward) * exitForce*400+ new Vector3(CopySource[activeBone].GetComponent<Rigidbody>().velocity.x, 0 , CopySource[activeBone].GetComponent<Rigidbody>().velocity.z)* 500* exitForce);
            Debug.Log(core.velocity);
            /*foreach (SphereCollider bone in bones)
            {
                bone.enabled = true;
                bone.GetComponent<JellyBone>().enabled = true;
                bone.gameObject.GetComponent<Rigidbody>().isKinematic = false;

            }*/
            bonesActive = true;
        }
        
        
    }

    void AddHandle(Transform handle)
    {
        //Debug.Log(handle.name);
    }
}