using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {
	private void OnTriggerEnter(Collider other) {
		if (other.GetComponent<JellyBone>()) {
			gameObject.SetActive(false);
		}
	}
}
