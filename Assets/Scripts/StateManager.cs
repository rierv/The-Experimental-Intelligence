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
	public float temperatureChangeRate = 1;
	public FlapperState state;
	public float gaseousPush = 3;
	public float gaseousMass = 0.5f;
	[Space]
	public GameObject mesh;
	public Material jelly;
	public Material gas;
	public Material solid;
	public GameObject[] gasParticles;

	JellyBone[] bones;
	Rigidbody rigidbody;
	float defaultMass;
	SphereCollider collider;
	SkinnedMeshRenderer meshRenderer;

	void Awake() {
		bones = FindObjectsOfType<JellyBone>();
		rigidbody = GetComponent<Rigidbody>();
		defaultMass = rigidbody.mass;
		collider = GetComponent<SphereCollider>();
		meshRenderer = mesh.GetComponent<SkinnedMeshRenderer>();
	}

	void Start() {
		SetState(FlapperState.jelly);
	}

	void Update() {
		if (state == FlapperState.gaseous) {
			rigidbody.AddForce(Physics.gravity * -JellyCore.gaseousAntiGravity);
		}
		if (temperature >= 10) {
			SetState(FlapperState.gaseous);
		} else if (temperature <= -10) {
			SetState(FlapperState.solid);
		}
		if (temperature > 0) {
			temperature = Mathf.Clamp(temperature - Time.deltaTime * temperatureChangeRate, 0, float.MaxValue);
			if (temperature <= 0) {
				SetState(FlapperState.jelly);
			}
		} else if (temperature < 0) {
			temperature = Mathf.Clamp(temperature + Time.deltaTime * temperatureChangeRate, float.MinValue, 0);
			if (temperature >= 0) {
				SetState(FlapperState.jelly);
			}
		}
	}

	public void SetState(FlapperState newState) {
		Debug.Log("New state: " + newState);
		state = newState;
		foreach (JellyBone b in bones) {
			b.SetState(state);
		}
		if (state == FlapperState.gaseous) {
			rigidbody.mass = gaseousMass;
			rigidbody.AddForce(Vector3.up * gaseousPush, ForceMode.Impulse);
			/*foreach (JellyBone bone in FindObjectsOfType<JellyBone>()) {
				bone.GetComponent<Rigidbody>().AddForce(Vector3.up * gaseousPush, ForceMode.Impulse);
			}*/
			//mesh.SetActive(false);
			foreach (GameObject go in gasParticles) {
				go.SetActive(true);
			}
			meshRenderer.material = gas;
		} else {
			rigidbody.mass = defaultMass;
			//mesh.SetActive(true);
			foreach (GameObject go in gasParticles) {
				go.SetActive(false);
			}
			if (state == FlapperState.solid) {
				meshRenderer.material = solid;
			} else {
				meshRenderer.material = jelly;
			}
		}
		collider.enabled = (state != FlapperState.solid);
	}
}
