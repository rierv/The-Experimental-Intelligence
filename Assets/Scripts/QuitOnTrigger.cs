using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitOnTrigger : MonoBehaviour {
	private void OnTriggerEnter(Collider other) {
		if (other.GetComponent<JellyCore>()) {
			Application.Quit();
		}
	}
}
