using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour, I_Activable {
	public Object nextLevel;
	public bool isActive = true;

	private void OnTriggerEnter(Collider other) {
		if (isActive && other.GetComponent<JellyCore>()) {
			SceneManager.LoadScene(nextLevel.name);
		}
	}

	public void Activate() {
		isActive = true;
	}

	public void Deactivate() {
		isActive = false;
	}

    public void Activate(bool twoFunctions)
    {
    }
}
