using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour, I_Activable {
	public Object nextLevel;
	public bool isActive = true;
	Light light;

	void Awake() {
		light = GetComponentInChildren<Light>();
		light.enabled = isActive;
	}

	private void OnTriggerEnter(Collider other) {
		if (isActive && other.GetComponent<JellyCore>()) {
			SceneManager.LoadScene(nextLevel.name);
		}
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
