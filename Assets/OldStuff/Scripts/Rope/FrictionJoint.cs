using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrictionJoint : MonoBehaviour
{

    [Range(0, 2)]
    public float Friction=0.5f;

    protected Rigidbody Rigidbody;

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Rigidbody.velocity = Rigidbody.velocity * (2 - Friction);
        Rigidbody.angularVelocity = Rigidbody.angularVelocity * (2 - Friction);
    }



}