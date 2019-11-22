using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureSource : MonoBehaviour, I_Activable {
	[Range(-1, 1)]
	public int variation;
	public bool active = false;
	MeshRenderer myColor;
	public Material activeMaterial;
	public Material nonActiveMaterial;

	private void Start() {
		myColor = transform.GetComponentInChildren<MeshRenderer>();
		if (!active) Deactivate();
	}
	public void Activate() {
		active = true;
		myColor.material = activeMaterial;
	}
	public void Deactivate() {
		active = false;
		myColor.material = nonActiveMaterial;

	}

	private void OnTriggerStay(Collider other) {
		if ((other.gameObject.tag == "Bone"||other.gameObject.tag == "Player") && active) {
			other.GetComponentInParent<FlapperCore>().GetComponentInChildren<StateManager>().temperature = variation;
		}
	}
}
