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
	public Material jelly;
	public Material gas;
	public Material solid;
	public GameObject[] gasParticles;
	public ParticleSystem gasParticle;
	public float jellySpeed = 7;
	public float solidSpeed = 6f;
	public float gaseousSpeed = 4f;
	public AudioClip gasTransition;
	public AudioClip jellyTransition;
	public AudioClip solidTransition;
	JellyBone[] bones;
	Rigidbody rigidbody;
	float defaultMass;
	SphereCollider collider;
	SkinnedMeshRenderer meshRenderer;
	PlayerMove pm;
	AudioSource audioSource;

	void Awake() {
		bones = GetComponentInParent<FlapperCore>().GetComponentsInChildren<JellyBone>();
		rigidbody = GetComponent<Rigidbody>();
		defaultMass = rigidbody.mass;
		collider = GetComponent<SphereCollider>();
		meshRenderer = mesh.GetComponent<SkinnedMeshRenderer>();
		pm = GetComponent<PlayerMove>();
		audioSource = GetComponent<AudioSource>();
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
			if (temperature <= 0) {
				SetState(FlapperState.jelly);
			}
		} else if (temperature < 0) {
			temperature = Mathf.Clamp(temperature + Time.deltaTime / coldTemperatureChangeDuration, float.MinValue, 0);
			if (temperature >= 0) {
				SetState(FlapperState.jelly);
			}
		}
	}

	public void SetState(FlapperState newState) {
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
			pm.speed = gaseousSpeed;
			/*foreach (JellyBone bone in bones) {
				bone.GetComponent<Rigidbody>().AddForce(Vector3.up * gaseousPush, ForceMode.Impulse);
			}*/
			//mesh.SetActive(false);
			/*foreach (GameObject go in gasParticles) {
				go.SetActive(true);
			}*/
			gasParticle.Play();
			meshRenderer.material = gas;
			audioSource.PlayOneShot(gasTransition);
		} else {
			rigidbody.mass = defaultMass;
			//mesh.SetActive(true);
			/*foreach (GameObject go in gasParticles) {
				go.SetActive(false);
			}*/
			gasParticle.Stop();
			if (state == FlapperState.solid) {
				meshRenderer.material = solid;
				pm.speed = solidSpeed;
				audioSource.PlayOneShot(solidTransition);
			} else {
				meshRenderer.material = jelly;
				pm.speed = jellySpeed;
				audioSource.PlayOneShot(jellyTransition);
			}
		}
		rigidbody.useGravity = state != FlapperState.gaseous;
		collider.enabled = (state != FlapperState.solid);
	}
}
