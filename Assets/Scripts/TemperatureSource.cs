using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureSource : MonoBehaviour {
	[Range(-1, 1)]
	public int variation;

	private void OnTriggerEnter(Collider other) {
		JellyBone jellyBone = other.GetComponent<JellyBone>();
		if (jellyBone) {
			jellyBone.GetComponentInParent<FlapperCore>().GetComponentInChildren<StateManager>().temperature = variation;
		}
	}
}
