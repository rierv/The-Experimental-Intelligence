using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {
	public AudioClip sound;
	public float rotationAngle = 1;

	void Update() {
		transform.Rotate(Vector3.up, rotationAngle, Space.Self);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.GetComponent<JellyBone>()) {
			//AudioManager.singleton.PlayClip(sound);
			gameObject.SetActive(false);
            GameObject.Find("Clock").GetComponent<ClockManager>().AddStar();
		}
	}
}
