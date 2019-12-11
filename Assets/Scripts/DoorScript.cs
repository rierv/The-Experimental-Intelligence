using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour, I_Activable {
	#region Attributes
	public float upperLockerY;
	public float lowerLockerY;
	public float doorOpenTime;
	bool goingUp = false;
	bool stopped = false;
	private Vector3 translateVector;
	bool activable = true;
	#endregion

	private void Start() {
		translateVector = Vector3.up;
	}

	private void FixedUpdate() {
		if (goingUp) {
			if ((upperLockerY - transform.position.y) > 0) {
				transform.position = Vector3.Lerp(transform.position, transform.position + translateVector, Time.deltaTime * doorOpenTime);
			}
		} else if (!stopped && (transform.position.y - lowerLockerY) > 0) {
			transform.position = Vector3.Lerp(transform.position, transform.position - translateVector, Time.deltaTime * doorOpenTime);
		}
	}

	private void OnCollisionStay(Collision collision) {
		if (!goingUp && (collision.gameObject.GetComponent<StateManager>() && collision.gameObject.GetComponent<StateManager>().state == FlapperState.solid || collision.gameObject.layer == 13)) {
			stopped = true;
		}
	}
	private void OnCollisionExit(Collision collision) {
		if (stopped && (collision.gameObject.GetComponent<StateManager>() && collision.gameObject.GetComponent<StateManager>().state == FlapperState.solid || collision.gameObject.layer == 13)) {
			stopped = false;
		}
	}

	public void Activate(bool type = true) {
		if (activable) goingUp = true;
	}

	public void Deactivate() {
		goingUp = false;
	}

	public void canActivate(bool enabled) {
		activable = enabled;
	}
}
