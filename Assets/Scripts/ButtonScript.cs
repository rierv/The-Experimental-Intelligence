﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {
	#region Attributes
	public GameObject[] triggeredObjects;
	public float timeBeforeDeactivate = 1;
	public float maxZ;
	public float minZ;
	public bool continousActivate;

	List<I_Activable> activables = new List<I_Activable>();
	float maxZtemp;
	float timer = 0;
	bool waiting;
	public GameObject body;
	AudioSource audioSource;
	#endregion

	private void Start() {
		audioSource = GetComponent<AudioSource>();
		foreach (GameObject go in triggeredObjects) {
			activables.Add(go.GetComponent<I_Activable>());
		}
		maxZtemp = maxZ;
	}

	private void OnTriggerEnter(Collider other) {
		if (timer <= 0 && (other.GetComponent<StateManager>() && other.GetComponent<StateManager>().state != FlapperState.gaseous) || (other.gameObject.layer == 13 || other.gameObject.layer == 12) && !other.isTrigger) {
			foreach (I_Activable ac in activables) {
				ac.Activate();
			}
			PlayClip();
			maxZtemp = minZ;
			waiting = true;
		}
	}

	private void OnTriggerStay(Collider other) {
		if ((other.GetComponent<StateManager>() && other.GetComponent<StateManager>().state != FlapperState.gaseous) || (other.gameObject.layer == 13 || other.gameObject.layer == 12) && !other.isTrigger) {
			if (continousActivate || !waiting) {
				foreach (I_Activable ac in activables) {
					ac.Activate();
				}
			}
			waiting = true;
			timer = timeBeforeDeactivate;
		}
	}

	private void OnTriggerExit(Collider other) {
		if ((other.GetComponent<StateManager>() && other.GetComponent<StateManager>().state != FlapperState.gaseous) || (other.gameObject.layer == 13 || other.gameObject.layer == 12) && !other.isTrigger) {
			waiting = false;
		}
	}

	private void Update() {
		body.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Clamp(transform.localPosition.z, minZ, maxZtemp));
		timer -= Time.deltaTime;
		if (!waiting && timer <= 0 && maxZtemp != maxZ) {
			foreach (I_Activable ac in activables) {
				ac.Deactivate();
			}
			canPlayClip = true;
			maxZtemp = maxZ;
		}
	}

	bool canPlayClip = true;
	void PlayClip() {
		if (canPlayClip) {
			audioSource.Play();
			canPlayClip = false;
		}
	}
}
