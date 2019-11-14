using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {
	#region Attributes
	public GameObject triggeredObject;
	public float timeBeforeDeactivate = 1;
	public float maxZ;
	public float minZ;

	I_Activable activable;
	float maxZtemp;
	#endregion

	private void Start() {
		activable = triggeredObject.GetComponent<I_Activable>();
		maxZtemp = maxZ;
	}

	private void OnTriggerStay(Collider other) {
		if (other.CompareTag("Player") || other.gameObject.layer == 13) {
			activable.Activate();
			if (transform.localPosition.z <= minZ) {
				maxZtemp = minZ;
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player") || other.gameObject.layer == 13) {
			StartCoroutine(DeactivateCoroutine());
		}
	}

	IEnumerator DeactivateCoroutine() {
		yield return new WaitForSeconds(timeBeforeDeactivate);
		activable.Deactivate();
		maxZtemp = maxZ;
	}

	private void Update() {
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Clamp(transform.localPosition.z, minZ, maxZtemp));
	}
}
