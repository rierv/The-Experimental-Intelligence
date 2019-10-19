using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyBone : MonoBehaviour {
	JellyCore core;
	Rigidbody rigidbody;
	Quaternion baseRotation;

	void Awake() {
		core = FindObjectOfType<JellyCore>();
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.drag = JellyCore.drag;
		baseRotation = rigidbody.rotation;
	}

	void FixedUpdate() {
		Vector3 force = (core.transform.position - transform.position) * JellyCore.cohesion;
		force.y = Mathf.Clamp(force.y, -JellyCore.maxFallingForce, float.MaxValue);
		rigidbody.AddForce(force);

		rigidbody.MoveRotation(baseRotation);

		rigidbody.drag = JellyCore.drag;
	}
}
