using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureSource : MonoBehaviour {
	[Range(-10, 10)]
	public int variation;

	private void OnTriggerEnter(Collider other) {
		StateManager stateManager = other.GetComponent<StateManager>();
		if (stateManager) {
			stateManager.temperature = variation;
		}
	}
}
