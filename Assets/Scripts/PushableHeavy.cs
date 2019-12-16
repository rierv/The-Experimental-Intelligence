using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableHeavy : MonoBehaviour {
	Rigidbody rigidbody;

	void Awake() {
		rigidbody = GetComponent<Rigidbody>();
	}

	private void OnTriggerStay(Collider other) {
		StateManager flapper = other.GetComponent<StateManager>();
		if (flapper) {
			if (flapper.state == FlapperState.solid) {
				rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			} else {
				rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.GetComponent<StateManager>()) {
			rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		}
	}
}
