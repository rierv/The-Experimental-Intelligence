﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableHeavy : MonoBehaviour {
	[Tooltip("False = move on X")]
	public bool moveOnZ = false;

	Rigidbody rigidbody;

	void Awake() {
		rigidbody = GetComponentInParent<Rigidbody>();
	}

	private void OnTriggerStay(Collider other) {
		JellyBone jellyBone = other.GetComponent<JellyBone>();
		if (jellyBone) {
			Debug.Log(name + " " + other.name + " " + jellyBone.state);
			if (jellyBone.state == FlapperState.solid) {
				if (moveOnZ) {
					rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX;
				} else {
					rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
				}
			} else {
				rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
			}
		}
	}
}
