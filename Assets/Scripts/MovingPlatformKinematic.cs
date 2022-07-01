using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformKinematic : MonoBehaviour, I_Activable {
	#region Attributes
	public float speed = 5;
	public Vector3[] targets; //Constraint: targets.Length >= 2

	private int targetsLength;
	private int targetsIndex;
	private bool isPlatformMoving;
	private float platformTimer;
	private float platformStopTime = .2f;
	public bool boltActive = false;
	private bool active = true;
	bool activable = true;
	float timeElapsed = 0;
	Vector3 startingPos;
	#endregion

	private void Start() {
		startingPos = transform.localPosition;
		if (boltActive) active = false;
		platformTimer = platformStopTime;
		isPlatformMoving = false;
		targetsLength = targets.Length;
		targetsIndex = 1;
	}

	private void FixedUpdate() {
		if (active) {
			if (!isPlatformMoving) //The platform is not moving, so you have to wait for a short time and then start moving
			{
				if (platformTimer > 0f) {
					platformTimer -= Time.fixedDeltaTime;
				} else {
					isPlatformMoving = true;
				}
			} else //The platform is moving, you may have to slow down or speed up
			  {
				timeElapsed = timeElapsed + Time.fixedDeltaTime;
				//Get closer to the current target
				transform.localPosition = Vector3.Lerp(transform.localPosition - startingPos, targets[targetsIndex], timeElapsed* speed)+startingPos;

				//Check if you have to change the current target
				if (Vector3.Distance(transform.localPosition - startingPos, targets[targetsIndex]) < .001f) {
					//Update the current target and if necessary change the direction (forward/backward route)
					ChangeDirection();
				}
			}
		}
	}

	private void ChangeDirection() {
		isPlatformMoving = false;
		platformTimer = platformStopTime;
		timeElapsed = 0;
		if (targetsIndex == 0) targetsIndex = 1;
		else targetsIndex = 0; // (targetsIndex + 1) % targets.Length;
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.magenta;
		if (targets != null && targets.Length >= 2) {
			for (var i = 0; i < targets.Length - 1; i++)
				Gizmos.DrawLine(targets[i], targets[i + 1]);
		}
	}

	public void Activate(bool type = true) {
		if (activable) active = true;
	}

	public void Deactivate() {
		active = false;
	}

	public void canActivate(bool enabled) {
		activable = enabled;
	}

    private void OnCollisionEnter(Collision collision)
    {
		if (collision.gameObject.GetComponent<ThrowableObject>()) active = true;
    }
    private void OnCollisionExit(Collision collision)
    {
		if (collision.gameObject.GetComponent<ThrowableObject>() && boltActive) active = false;
	}
}
