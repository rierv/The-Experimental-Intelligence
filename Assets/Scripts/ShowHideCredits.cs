using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideCredits : MonoBehaviour
{
	public GameObject credits;

	private void OnTriggerEnter(Collider other) {
		if (other.GetComponent<JellyCore>()) {
			credits.SetActive(true);
		}
	}
	private void OnTriggerExit(Collider other) {
		if (other.GetComponent<JellyCore>()) {
			credits.SetActive(false);
		}
	}
}
