using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public List<GameObject> Bones;
    public float Distance=10;
    private Vector3[] BonePositions;
    Rigidbody[] BoneBodies;
    Transform[] Handlers;
    public float rigidbodyMass;
    public float power;

    // Start is called before the first frame update
    void Start()
    {
        BonePositions = new Vector3[Bones.Count];
        BoneBodies = new Rigidbody[Bones.Count];
        Handlers = new Transform[Bones.Count];
        int increment = 0;
        foreach (GameObject bone in Bones)
        {
           
            bone.transform.position = Vector3.down * (increment+1) * Distance + bone.transform.position;
            Rigidbody childRigidbody= bone.gameObject.GetComponent<Rigidbody>();
            childRigidbody.useGravity = true;
            childRigidbody.isKinematic = false;
            childRigidbody.freezeRotation = true;
            childRigidbody.mass = rigidbodyMass;
            BoneBodies[increment] = childRigidbody;
            bone.transform.LookAt(bone.transform.position);
            BonePositions[increment] = bone.transform.position ;
            Handlers[increment] = bone.GetComponentInChildren<Transform>();
            increment++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int increment = 0;
        foreach (GameObject bone in Bones)
        {
            Handlers[increment].LookAt(BonePositions[increment]);
            BoneBodies[increment].AddForce(Handlers[increment].forward * (bone.transform.position - BonePositions[increment]).magnitude * power);
                //AddRelativeForce((bone.transform.position-BonePositions[increment])* (bone.transform.position - BonePositions[increment]).magnitude*3);
            increment++;
        }
    }
}
