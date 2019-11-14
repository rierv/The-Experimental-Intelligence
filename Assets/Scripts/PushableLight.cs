using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableLight : MonoBehaviour {
	[Tooltip("False = move on X")]
	public bool moveOnZ = false;

	Rigidbody rigidbody;

	void Awake() {
		rigidbody = GetComponentInParent<Rigidbody>();
	}

	private void OnTriggerEnter(Collider other) {
		JellyBone jellyBone = other.GetComponent<JellyBone>();
		if (jellyBone) {
			if (jellyBone.state != FlapperState.gaseous) {
				if (moveOnZ) {
					rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
				} else {
					rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
				}
			} else {
				rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			}
		}
	}
}
