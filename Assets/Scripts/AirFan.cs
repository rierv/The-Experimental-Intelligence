using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirFan : MonoBehaviour {
	public Transform fan;
	public float fanSpeed;
	public float force = 4;
	public float splashForce = 0.85f;
	float surface;

	void Awake() {
		surface = transform.localScale.y;
	}

	void Update() {
		fan.Rotate(transform.up, fanSpeed * Time.deltaTime);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.GetComponent<StateManager>() && other.GetComponent<StateManager>().state == FlapperState.jelly || other.GetComponent<JellyBone>() && other.GetComponent<JellyBone>().state == FlapperState.jelly) {
			Rigidbody r = other.GetComponent<Rigidbody>();
			r.useGravity = false;
			r.AddForce(transform.up * -r.velocity.y * splashForce, ForceMode.VelocityChange);
		}
	}

	private void OnTriggerStay(Collider other) {
		if (other.GetComponent<StateManager>() && other.GetComponent<StateManager>().state == FlapperState.jelly || other.GetComponent<JellyBone>() && other.GetComponent<JellyBone>().state == FlapperState.jelly) {
			other.GetComponent<Rigidbody>().AddForce(transform.up * (transform.position.y + surface - other.transform.position.y) * force, ForceMode.Acceleration);
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.GetComponent<StateManager>() && other.GetComponent<StateManager>().state == FlapperState.jelly || other.GetComponent<JellyBone>() && other.GetComponent<JellyBone>().state == FlapperState.jelly) {
			other.GetComponent<Rigidbody>().useGravity = true;
		}
	}
}
