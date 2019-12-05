using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour, I_Activable {
	public Object nextLevel;
	//public AudioClip sound;
	public float delayToStopFlapper = 0.1f;
	public float delayToLoadLevel = 0.3f;
	public bool isActive = true;
	bool activable = true;

	Light light;

	float logTimer = 0;

	void Awake() {
		light = GetComponentInChildren<Light>();
		light.enabled = isActive;
	}

	void Update() {
		logTimer += Time.deltaTime;
	}

	private void OnTriggerEnter(Collider other) {
		if (isActive && other.GetComponent<JellyCore>()) {
			StartCoroutine(LoadLevel());
		}
	}

	IEnumerator LoadLevel() {
		System.IO.File.AppendAllText(FlapperCore.logFile, logTimer.ToString());

		yield return new WaitForSeconds(delayToStopFlapper);
		GetComponent<AudioSource>().Play();
		foreach (PlayerMove p in FindObjectsOfType<PlayerMove>()) {
			p.canMove = false;
		}
		yield return new WaitForSeconds(delayToLoadLevel);
		SceneManager.LoadScene(nextLevel.name);
	}

	public void Activate(bool type = true) {
		if (activable) {
			isActive = true;
			light.enabled = isActive;
		}
	}

	public void Deactivate() {
		isActive = false;
		light.enabled = isActive;
	}

	public void canActivate(bool enabled) {
		activable = enabled;
	}
}
