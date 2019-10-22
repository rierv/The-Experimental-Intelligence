using System.Collections;
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

	void Awake() {
		core = FindObjectOfType<JellyCore>();
		rigidbody = GetComponent<Rigidbody>();
		collider = GetComponent<SphereCollider>();
		rigidbody.drag = JellyCore.drag;
		baseRotation = rigidbody.rotation;
	}

	void FixedUpdate() {
		if (state == FlapperState.solid) {
			if (isRoot) {
				transform.position = core.transform.position + localPos;
				transform.rotation = core.transform.rotation;
			}
		} else {
			if (state == FlapperState.gaseous) {
				rigidbody.AddForce(Physics.gravity * -2);
			}
			Vector3 force = (core.transform.position - transform.position) * JellyCore.cohesion;
			force.y = Mathf.Clamp(force.y, -JellyCore.maxFallingForce, float.MaxValue);
			rigidbody.AddForce(force);

			rigidbody.MoveRotation(baseRotation);
		}

		rigidbody.drag = JellyCore.drag;
	}

	public void SetState(FlapperState newState) {
		state = newState;
		switch (state) {
			case FlapperState.jelly:
				rigidbody.constraints = RigidbodyConstraints.None;
				if (coreCollider) {
					coreCollider.enabled = false;
				}
				break;
			case FlapperState.solid:
				rigidbody.constraints = RigidbodyConstraints.FreezeAll;
				if (!isRoot) {
					if (!coreCollider) {
						coreCollider = core.gameObject.AddComponent<SphereCollider>();
						coreCollider.radius = collider.radius;
					} else {
						coreCollider.enabled = true;
					}
					coreCollider.center = transform.localRotation * collider.center + transform.localPosition;
					Debug.Log(name + " -> " + transform.rotation * collider.center);
				} else {
					localPos = transform.position - core.transform.position;
				}
				break;
			case FlapperState.gaseous:
				break;
		}
	}
}
