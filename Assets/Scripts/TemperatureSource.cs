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
	[Header("Activable")]
	public bool invertTrueFalse;

	private void Start() {
		myColor = transform.GetComponentInChildren<MeshRenderer>();
		if (!active) Deactivate();
	}

	public void Activate(bool type) {
		if (type && !invertTrueFalse || !type && invertTrueFalse) {
			active = true;
			myColor.material = activeMaterial;
		} else {
			Deactivate();
		}
	}

	public void Deactivate() {
		active = false;
		myColor.material = nonActiveMaterial;

	}

	private void OnTriggerStay(Collider other) {
		if ((other.gameObject.tag == "Bone" || other.gameObject.tag == "Player") && active) {
			other.GetComponentInParent<FlapperCore>().GetComponentInChildren<StateManager>().temperature = variation;
		}
	}
}
