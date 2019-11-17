using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour, I_Activable {
	public Object nextLevel;
	public float delayToStopFlapper = 0.1f;
	public float delayToLoadLevel = 0.3f;
	public bool isActive = true;
	Light light;

	void Awake() {
		light = GetComponentInChildren<Light>();
		light.enabled = isActive;
	}

	private void OnTriggerEnter(Collider other) {
		if (isActive && other.GetComponent<JellyCore>()) {
			StartCoroutine(LoadLevel());
		}
	}

	IEnumerator LoadLevel() {
		yield return new WaitForSeconds(delayToStopFlapper);
		foreach (PlayerMove p in FindObjectsOfType<PlayerMove>()) {
			p.canMove = false;
		}
		yield return new WaitForSeconds(delayToLoadLevel);
		SceneManager.LoadScene(nextLevel.name);
	}

	public void Activate() {
		isActive = true;
		light.enabled = isActive;
	}

	public void Deactivate() {
		isActive = false;
		light.enabled = isActive;
	}

	public void Activate(bool twoFunctions) {
	}
}
