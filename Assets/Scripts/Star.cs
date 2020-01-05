using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {
	//public AudioClip sound;
	public float rotationAngle = 1;

	void Update() {
		transform.Rotate(Vector3.up, rotationAngle * Time.timeScale, Space.Self);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.GetComponent<JellyBone>()) {
			transform.GetChild(0).gameObject.SetActive(false);
			GetComponent<Collider>().enabled = false;
			GetComponent<AudioSource>().Play();

			//FindObjectOfType<ClockManager>().AddStar();
			FindObjectOfType<StarCollector>().AddStar();
		}
	}
}
