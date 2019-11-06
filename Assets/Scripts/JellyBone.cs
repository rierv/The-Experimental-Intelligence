﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyBone : MonoBehaviour {
	public bool isRoot;
	JellyCore core;
	Rigidbody rigidbody;
	SphereCollider collider;
	Quaternion baseRotation;
	FlapperState state;

	SphereCollider coreCollider;
	Vector3 localPos;

    Vector3 lastGoodPosition;
    

    void Awake() {
		core = FindObjectOfType<JellyCore>();
		rigidbody = GetComponent<Rigidbody>();
		collider = GetComponent<SphereCollider>();
		rigidbody.drag = JellyCore.drag;
		baseRotation = rigidbody.rotation;
        transform.localPosition = Vector3.zero;
        if (!isRoot) {
			coreCollider = core.gameObject.AddComponent<SphereCollider>();
			coreCollider.radius = collider.radius;
		}
	}

	void FixedUpdate() {
		if (state == FlapperState.solid) {
			if (isRoot) {
				transform.position = core.transform.position + localPos;
				transform.rotation = core.transform.rotation;
			}
		} else {
			if (state == FlapperState.gaseous) {
				rigidbody.AddForce(Physics.gravity * -JellyCore.gaseousAntiGravity);
			}
			Vector3 force = (core.transform.position - transform.position) * JellyCore.cohesion;
			force.y = Mathf.Clamp(force.y, Physics.gravity.y, float.MaxValue);
			rigidbody.AddForce(force);

			rigidbody.MoveRotation(baseRotation);
            CheckCorePosition();
		}

		//rigidbody.drag = JellyCore.drag;
	}

	public void SetState(FlapperState newState) {
		state = newState;
		if (state == FlapperState.jelly || state == FlapperState.gaseous) {
			rigidbody.constraints = RigidbodyConstraints.None;
			if (coreCollider) {
				coreCollider.enabled = false;
			}
		} else { // solid
			rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			if (!isRoot) {
				coreCollider.enabled = true;
				coreCollider.center = transform.localRotation * collider.center + transform.position - core.transform.position;
			} else {
				localPos = transform.position - core.transform.position;
			}
		}
		/*if (state == FlapperState.gaseous) {
			rigidbody.drag = JellyCore.gaseousDrag;
		} else {
			rigidbody.drag = JellyCore.drag;
		}*/
	}
    void CheckCorePosition()
    {
        // This would cast rays only against colliders in layer 9.
        RaycastHit hit;
        // Does the ray intersect any objects in the player layer
        int layerMask = 1 << 9;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        

        if (transform.position.magnitude > 4f)
        {
            if (Physics.Raycast(core.transform.position + transform.localPosition + Vector3.up, -transform.localPosition.normalized, out hit, transform.localPosition.magnitude, layerMask))
            {
                // We hit a non-player object!
                //if (Physics.Raycast(transform.position, (core.transform.position - transform.position).normalized, out hit, Mathf.Infinity) && hit.collider.gameObject.layer != 9)
                //{
                Vector3 force = (lastGoodPosition) * JellyCore.cohesion;
                //force.y = Mathf.Clamp(force.y, Physics.gravity.y, float.MaxValue);
                //rigidbody.AddForce(force);
                Debug.Log(transform.localPosition);
                Debug.DrawRay(core.transform.position+transform.localPosition, lastGoodPosition*10, Color.red, 20, true);
                Debug.DrawRay(core.transform.position + transform.localPosition/10, Vector3.up * 10, Color.green, 20, true);

                rigidbody.AddForce(force);
                Debug.Log("strong");
            }
        }

        
        else if(transform.position.magnitude < 0.5f)
        {
            lastGoodPosition = -transform.localPosition;
            lastGoodPosition = lastGoodPosition.normalized;
            
            //Debug.Log(lastGoodPosition);
        }


    }
}
