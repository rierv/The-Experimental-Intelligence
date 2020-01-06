using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {
	//public AudioClip sound;
	public float rotationAngle = 1;
	public Sprite profSprite;
	public float resetSpriteDelay = 1;
	GameManager gameManager;

	void Start() {
		gameManager = FindObjectOfType<GameManager>();
	}

	void Update() {
		transform.Rotate(Vector3.up, rotationAngle * Time.timeScale, Space.Self);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.GetComponent<JellyBone>()) {
			gameManager.profSprite.sprite = profSprite;
			transform.GetChild(0).gameObject.SetActive(false);
			GetComponent<Collider>().enabled = false;
			GetComponent<AudioSource>().Play();

			//FindObjectOfType<ClockManager>().AddStar();
			FindObjectOfType<StarCollector>().AddStar();
			StartCoroutine(ResetProfSprite());
		}
	}

	IEnumerator ResetProfSprite() {
		yield return new WaitForSeconds(resetSpriteDelay);
		gameManager.ResetProfSprite();
	}
}
