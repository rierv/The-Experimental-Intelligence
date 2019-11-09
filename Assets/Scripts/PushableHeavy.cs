using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableHeavy : MonoBehaviour {
	Rigidbody rigidbody;

	void Awake() {
		rigidbody = GetComponent<Rigidbody>();
	}

	private void OnTriggerEnter(Collider other) {
		JellyBone jellyBone = other.GetComponent<JellyBone>();
		if (jellyBone) {
			if (jellyBone.state == FlapperState.solid) {
				rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			} else {
				rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			}
		}
	}
}
