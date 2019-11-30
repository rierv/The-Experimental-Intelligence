using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {
	public float surface;
	public float force = 4;
	public float splashForce = 0.85f;

	private void OnTriggerEnter(Collider other) {
		if (other.GetComponent<StateManager>() && other.GetComponent<StateManager>().state == FlapperState.jelly || other.GetComponent<JellyBone>() && other.GetComponent<JellyBone>().state == FlapperState.jelly) {
			Rigidbody r = other.GetComponent<Rigidbody>();
			r.useGravity = false;
			r.AddForce(Vector3.up * -r.velocity.y * splashForce, ForceMode.VelocityChange);
		}
	}

	private void OnTriggerStay(Collider other) {
		if (other.GetComponent<StateManager>() && other.GetComponent<StateManager>().state == FlapperState.jelly || other.GetComponent<JellyBone>() && other.GetComponent<JellyBone>().state == FlapperState.jelly) {
			other.GetComponent<Rigidbody>().AddForce(Vector3.up * (transform.position.y + surface - other.transform.position.y) * force, ForceMode.Acceleration);
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.GetComponent<StateManager>() && other.GetComponent<StateManager>().state == FlapperState.jelly || other.GetComponent<JellyBone>() && other.GetComponent<JellyBone>().state == FlapperState.jelly) {
			other.GetComponent<Rigidbody>().useGravity = true;
		}
	}
}
