using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicTriggerIfDontMove : MonoBehaviour, I_Activable {
	public ActivateComic activateComic;
	public float activateAfter;

	void Awake() {
		gameObject.layer = 14;
	}

	public void canActivate(bool enabled) {
	}

	public void Activate(bool type = true) {
		StartCoroutine(ActivateCoroutine());
	}

	public void Deactivate() {
	}

	IEnumerator ActivateCoroutine() {
		yield return new WaitForSeconds(activateAfter);
		activateComic.Activate();
	}

	void Update() {
		if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
			StopAllCoroutines();
		}
	}
}
