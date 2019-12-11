﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
	public float offsetY = 8.5f;
	public float offsetZ = -15.5f;
	public float speed = 55;
	public float initialDelay = 2;
	bool lookAtFlapper;

	PlayerMove jellyCore;
	StateManager stateManager;
	Vector3 initPosition;

	void Start() {
		initPosition = transform.position;
		jellyCore = FindObjectOfType<PlayerMove>();
		stateManager = FindObjectOfType<StateManager>();
		StartCoroutine(InitialCoroutine());
	}
	IEnumerator InitialCoroutine() {
		yield return new WaitForSeconds(initialDelay);
		lookAtFlapper = true;
	}

	void FixedUpdate() {
		if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown("joystick button 2")) { // controller X
			lookAtFlapper = !lookAtFlapper;
		}
		if (lookAtFlapper) {
			float y = transform.position.y;
			if (jellyCore.canJump || stateManager.state == FlapperState.gaseous) {
				y = jellyCore.transform.position.y + offsetY;
			}
			transform.position = Vector3.Lerp(transform.position, new Vector3(jellyCore.transform.position.x, y, jellyCore.transform.position.z + offsetZ), speed * Time.fixedDeltaTime);
		} else {
			transform.position = Vector3.Lerp(transform.position, initPosition, speed * Time.fixedDeltaTime);
		}
	}
}