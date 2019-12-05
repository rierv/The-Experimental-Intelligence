using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour {
	public bool heavy = false;
	[Tooltip("False = move on X")]
	public bool moveOnZ = false;

	Rigidbody rigidbody;

	void Awake() {
		rigidbody = GetComponentInParent<Rigidbody>();
	}

	private void OnTriggerStay(Collider other) {
		StateManager flapper = other.GetComponent<StateManager>();
		if (flapper) {
			if (!heavy && flapper.state != FlapperState.gaseous || heavy && flapper.state == FlapperState.solid) {
				if (moveOnZ) {
					rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX;
				} else {
					rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
				}
			} else {
				rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
			}
		} else {
			Pushable pushable = other.GetComponent<Pushable>();
			if (pushable) {
				if (!heavy) {
					rigidbody.constraints = pushable.rigidbody.constraints;
				}
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.GetComponent<StateManager>() || other.GetComponent<Pushable>()) {
			rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
		}
	}
}
