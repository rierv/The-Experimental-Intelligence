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
			gameObject.SetActive(false);
            AudioManager.singleton.PlayClip(sound);
            //FindObjectOfType<ClockManager>().AddStar();
            FindObjectOfType<StarCollector>().AddStar();
            //FindObjectOfType<ActivateComic>().Activate();
        }
	}
}
