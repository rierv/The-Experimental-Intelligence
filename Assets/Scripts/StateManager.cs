using System.Collections;
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
	public float jellySpeed = 7;
	public float solidSpeed = 6f;
	public float gaseousSpeed = 4f;
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
				pm.velocity = gaseousSpeed;
				break;
			case FlapperState.jelly:
				meshRenderer.material.SetColor("_BaseColor", jelly);
				meshRenderer.material.SetColor("_EmissionColor", jelly);
				pm.velocity = jellySpeed;
				break;
			case FlapperState.solid:
				meshRenderer.material.SetColor("_BaseColor", solid);
				meshRenderer.material.SetColor("_EmissionColor", solid);
				pm.velocity = solidSpeed;
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
			//pm.velocity = gaseousSpeed;
			/*foreach (JellyBone bone in bones) {
				bone.GetComponent<Rigidbody>().AddForce(Vector3.up * gaseousPush, ForceMode.Impulse);
			}*/
			//mesh.SetActive(false);
			/*foreach (GameObject go in gasParticles) {
				go.SetActive(true);
			}*/
			shadow.SetActive(false);
			gasParticle.Play();
			meshRenderer.enabled = false;
			audioSource.PlayOneShot(gasTransition);
		} else {
			rigidbody.mass = defaultMass;
			//mesh.SetActive(true);
			/*foreach (GameObject go in gasParticles) {
				go.SetActive(false);
			}*/
			shadow.SetActive(true);
			gasParticle.Stop();
			meshRenderer.enabled = true;
			if (state == FlapperState.solid) {
				//pm.velocity = solidSpeed;
				audioSource.PlayOneShot(solidTransition);
			} else {
				//pm.velocity = jellySpeed;
				audioSource.PlayOneShot(jellyTransition);
			}
		}
		rigidbody.useGravity = state != FlapperState.gaseous;
		collider.enabled = (state != FlapperState.solid);
	}
}
