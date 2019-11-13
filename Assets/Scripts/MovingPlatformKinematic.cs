using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformKinematic : MonoBehaviour {
	#region Attributes
	public float speed = 10;
	public Vector3[] targets; //Constraint: targets.Length >= 2

	Rigidbody platform;
	private int targetsLength;
	private int targetsIndex;
	private bool direction; //true is forward, false is backward
	private bool isPlatformMoving;
	private float platformTimer;
	private float platformStopTime = 1.5f;
	#endregion

	private void Start() {
		direction = true;
		platformTimer = platformStopTime;
		isPlatformMoving = false;
		targetsLength = targets.Length;
		targetsIndex = 1;
		platform = GetComponent<Rigidbody>();
	}

	private void FixedUpdate() {
		if (!isPlatformMoving) //The platform is not moving, so you have to wait for a short time and then start moving
		{
			if (platformTimer > 0f) {
				platformTimer -= Time.fixedDeltaTime;
			} else {
				isPlatformMoving = true;
				if (direction) {
					targetsIndex = 1;
					platform.transform.position = Vector3.MoveTowards(platform.transform.position, targets[targetsIndex], Time.fixedDeltaTime * speed);
				} else {
					targetsIndex = targetsLength - 2;
					platform.transform.position = Vector3.MoveTowards(platform.transform.position, targets[targetsIndex], Time.fixedDeltaTime);
				}
			}
		} else //The platform is moving, you may have to slow down or speed up
		  {

			//Get closer to the current target
			platform.transform.position = Vector3.MoveTowards(platform.transform.position, targets[targetsIndex], Time.fixedDeltaTime);

			//Check if you have to change the current target
			if (Vector3.Distance(platform.transform.position, targets[targetsIndex]) < Time.fixedDeltaTime) {
				//Update the current target and if necessary change the direction (forward/backward route)
				if (direction && (targetsIndex < (targetsLength - 1))) {
					Debug.Log(targetsIndex);
					targetsIndex++;
					Debug.Log(targetsIndex);
				} else if (!direction && (targetsIndex > 0)) {
					Debug.Log(targetsIndex);
					targetsIndex--;
					Debug.Log(targetsIndex);
				} else
					ChangeDirection();
			}
		}
	}

	private void OnTriggerEnter(Collider other) {
		//If it's the player, stick it on the platform
		if (other.CompareTag("Player")) {
			other.transform.parent = platform.gameObject.transform;
		}
	}

	private void OnTriggerExit(Collider other) {
		//Free the player
		if (other.CompareTag("Player"))
			other.transform.parent = null;
	}

	private void ChangeDirection() {
		direction = !direction;
		isPlatformMoving = false;
		platformTimer = platformStopTime;
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.magenta;
		if (targets != null && targets.Length >= 2) {
			for (var i = 0; i < targets.Length - 1; i++)
				Gizmos.DrawLine(targets[i], targets[i + 1]);
		}
	}
}

