using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour {
	public bool solidOnly;

	Rigidbody rigidbody;
	RigidbodyConstraints constraints;

	void Awake() {
		rigidbody = GetComponent<Rigidbody>();
		constraints = rigidbody.constraints;
	}

	private void OnTriggerStay(Collider other) {
		StateManager flapper = other.GetComponent<StateManager>();
		if (flapper) {
			if (flapper.state == FlapperState.solid || !solidOnly && flapper.state == FlapperState.jelly) {
				rigidbody.constraints = constraints;
			} else {
				rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.GetComponent<StateManager>()) {
			StartCoroutine(ReleaseDelay());
		}
	}

	IEnumerator ReleaseDelay() {
		yield return new WaitForSeconds(0.35f);
		rigidbody.constraints = constraints;
	}
}
