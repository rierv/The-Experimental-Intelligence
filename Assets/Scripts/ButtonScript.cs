using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {
	#region Attributes
	public GameObject triggeredObject;
	public float timeBeforeDeactivate = 1;
	public float maxY;
	public float minY;

	I_Activable activable;
	private Vector3 currentPosition;
	private float clampedY;
	#endregion

	private void Start() {
		activable = triggeredObject.GetComponent<I_Activable>();
	}

	private void OnTriggerStay(Collider other) {
		if (other.CompareTag("Player") || other.gameObject.layer == 13) {
			activable.Activate();
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
	}

	private void Update() {
		currentPosition = transform.position;
		clampedY = Mathf.Clamp(currentPosition.y, minY, maxY);
		currentPosition.y = clampedY;
		transform.position = currentPosition;
	}
}
