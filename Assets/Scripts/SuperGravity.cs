using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperGravity : MonoBehaviour {
	public static float fallingBoost = 60;
	Rigidbody rigidbody;

	void Awake() {
		rigidbody = GetComponent<Rigidbody>();
	}

	void Update() {
		if (rigidbody.useGravity && rigidbody.velocity.y < -0.1f) {
			rigidbody.AddForce(Vector3.up * -fallingBoost, ForceMode.Acceleration);
		}
	}
}
