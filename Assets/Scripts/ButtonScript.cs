using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {
	#region Attributes
	public GameObject[] triggeredObjects;
	public float timeBeforeDeactivate = 1;
	public float maxZ;
	public float minZ;

	List<I_Activable> activables = new List<I_Activable>();
	float maxZtemp;
	float timer;
	#endregion

	private void Start() {
		foreach (GameObject go in triggeredObjects) {
			activables.Add(go.GetComponent<I_Activable>());
		}
		maxZtemp = maxZ;
	}

	private void OnTriggerStay(Collider other) {
		if (other.CompareTag("Player") || other.gameObject.layer == 13) {
			foreach (I_Activable ac in activables) {
				ac.Activate();
			}
			maxZtemp = minZ;
			timer = timeBeforeDeactivate;
		}
	}

	private void Update() {
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Clamp(transform.localPosition.z, minZ, maxZtemp));
		timer -= Time.deltaTime;
		if (timer <= 0 && maxZtemp != maxZ) {
			foreach (I_Activable ac in activables) {
				ac.Deactivate();
			}
			maxZtemp = maxZ;
		}
	}
}
