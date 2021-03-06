﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlapperState {
	jelly,
	solid,
	gaseous
}

public class StateManager : MonoBehaviour {
	public float temperature;
	public float hotTemperatureChangeDuration = 5;
	public float coldTemperatureChangeDuration = 5;
	public FlapperState state;
	public float gaseousPush = 3;
	public float gaseousMass = 0.5f;
	[Space]
	public GameObject mesh;
	public Color jelly;
	public Color gas;
	public Color solid;
	//public GameObject[] gasParticles;
	public GameObject shadow;
	public ParticleSystem gasParticle;
	public ParticleSystemRenderer gasParticleRenderer;
	[Header("Old move type")]
	public float jellySpeed = 7;
	public float solidSpeed = 6f;
	public float gaseousSpeed = 4f;
	[Header("Accelerate move type")]
	public float jellyAccelerate = 7;
	public float solidAccelerate = 6f;
	public float gaseousAccelerate = 4f;
	[Header("Rigidbody move type")]
	public float jellySpeedRigidbody = 7;
	public float solidSpeedRigidbody = 6f;
	public float gaseousSpeedRigidbody = 4f;
	[Space]
	public AudioClip gasTransition;
	public AudioClip jellyTransition;
	public AudioClip solidTransition;
	public AudioSource audioSource;

	JellyBone[] bones;
	Rigidbody rigidbody;
	float defaultMass;
	SphereCollider collider;
	SkinnedMeshRenderer meshRenderer;
	PlayerMove pm;

	void Awake() {
		bones = GetComponentInParent<FlapperCore>().GetComponentsInChildren<JellyBone>();
		rigidbody = GetComponent<Rigidbody>();
		defaultMass = rigidbody.mass;
		collider = GetComponent<SphereCollider>();
		meshRenderer = mesh.GetComponent<SkinnedMeshRenderer>();
		pm = GetComponent<PlayerMove>();
	}

	void Start() {
		SetState(FlapperState.jelly);
	}

	void Update() {
		if (temperature == 1) {
			SetState(FlapperState.gaseous);
		} else if (temperature == -1) {
			SetState(FlapperState.solid);
		}
		if (temperature > 0) {
			temperature = Mathf.Clamp(temperature - Time.deltaTime / hotTemperatureChangeDuration, 0, float.MaxValue);
			if (temperature < 0.5f) {
				//gasParticleRenderer.material.SetColor("_BaseColor", Color.Lerp(jelly, gas, temperature * 2));
				//gasParticleRenderer.material.SetColor("_EmissionColor", Color.Lerp(jelly, gas, temperature * 2));
				gasParticle.startColor = Color.Lerp(jelly, gas, temperature * 2);
			}
			if (temperature <= 0) {
				SetState(FlapperState.jelly);
			}
		} else if (temperature < 0) {
			temperature = Mathf.Clamp(temperature + Time.deltaTime / coldTemperatureChangeDuration, float.MinValue, 0);
			if (temperature > -0.5f) {
				meshRenderer.material.SetColor("_BaseColor", Color.Lerp(jelly, solid, -temperature * 2));
				meshRenderer.material.SetColor("_EmissionColor", Color.Lerp(jelly, solid, -temperature * 2));
			}
			if (temperature >= 0) {
				SetState(FlapperState.jelly);
			}
		}
	}

	public void SetState(FlapperState newState) {
		switch (newState) {
			case FlapperState.gaseous:
				//gasParticleRenderer.material.SetColor("_BaseColor", gas);
				//gasParticleRenderer.material.SetColor("_EmissionColor", gas);
				gasParticle.startColor = gas;
				if (pm.moveType == MoveType.Rigidbody)
					pm.velocity = gaseousSpeedRigidbody;
				else if (pm.moveType == MoveType.Accelerate)
					pm.velocity = gaseousAccelerate;
				else
					pm.speed = gaseousSpeed;
				break;
			case FlapperState.jelly:
				meshRenderer.material.SetColor("_BaseColor", jelly);
				meshRenderer.material.SetColor("_EmissionColor", jelly);
				if (pm.moveType == MoveType.Rigidbody)
					pm.velocity = jellySpeedRigidbody;
				else if (pm.moveType == MoveType.Accelerate)
					pm.velocity = jellyAccelerate;
				else
					pm.speed = jellySpeed;
				break;
			case FlapperState.solid:
				meshRenderer.material.SetColor("_BaseColor", solid);
				meshRenderer.material.SetColor("_EmissionColor", solid);
				if (pm.moveType == MoveType.Rigidbody)
					pm.velocity = solidSpeedRigidbody;
				else if (pm.moveType == MoveType.Accelerate)
					pm.velocity = solidAccelerate;
				else
					pm.speed = solidSpeed;
				break;
		}
		if (newState == state) {
			return;
		}
		Debug.Log("New state: " + newState);
		state = newState;
		foreach (JellyBone b in bones) {
			b.SetState(state);
		}
		if (state == FlapperState.gaseous) {
			rigidbody.AddForce(Vector3.up * gaseousPush, ForceMode.Impulse);
			/*foreach (JellyBone bone in bones) {
				bone.GetComponent<Rigidbody>().AddForce(Vector3.up * gaseousPush, ForceMode.Impulse);
			}*/
			//mesh.SetActive(false);
			/*foreach (GameObject go in gasParticles) {
				go.SetActive(true);
			}*/
			//shadow.SetActive(false);
			gasParticle.Play();
			meshRenderer.enabled = false;
			audioSource.PlayOneShot(gasTransition);
		} else {
			//rigidbody.mass = defaultMass;
			//mesh.SetActive(true);
			/*foreach (GameObject go in gasParticles) {
				go.SetActive(false);
			}*/
			//shadow.SetActive(true);
			gasParticle.Stop();
			meshRenderer.enabled = true;
			if (state == FlapperState.solid) {
				audioSource.PlayOneShot(solidTransition);
			} else {
				audioSource.PlayOneShot(jellyTransition);
			}
		}
		rigidbody.useGravity = state != FlapperState.gaseous;
		collider.enabled = (state != FlapperState.solid);
	}
}
