using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyBone : MonoBehaviour {
	public bool isRoot;
	JellyCore core;
	Rigidbody coreRigidbody;
	Rigidbody rigidbody;
	SphereCollider collider;
	Quaternion baseRotation;
	FlapperState state;

	SphereCollider coreCollider;
	Vector3 localPos;

	Vector3 lastGoodPosition;

	void Awake() {
		core = GetComponentInParent<FlapperCore>().GetComponentInChildren<JellyCore>();
		coreRigidbody = core.GetComponent<Rigidbody>();
		rigidbody = GetComponent<Rigidbody>();
		collider = GetComponent<SphereCollider>();
		rigidbody.drag = JellyCore.drag;
		baseRotation = rigidbody.rotation;
		transform.localPosition = Vector3.zero;
		if (!isRoot) {
			coreCollider = core.gameObject.AddComponent<SphereCollider>();
			coreCollider.radius = collider.radius;
		}
	}

	float lastCoreSpeedY;
	void FixedUpdate() {
		if (state == FlapperState.solid) {
			if (isRoot) {
				transform.position = core.transform.position + localPos;
				transform.rotation = core.transform.rotation;
			}
		} else {
			if (state == FlapperState.gaseous) {
				rigidbody.AddForce(Physics.gravity * -JellyCore.gaseousAntiGravity);
			}
			float acceleration = (coreRigidbody.velocity.y - lastCoreSpeedY) / Time.fixedDeltaTime;
			Vector3 force = (core.transform.position - transform.position) * JellyCore.cohesion;
			if (acceleration < -0.1f) {
				// limit acceleration only when falling
				force.y = Mathf.Clamp(force.y, acceleration, float.MaxValue);
			}
			rigidbody.AddForce(force, ForceMode.Acceleration);
			lastCoreSpeedY = coreRigidbody.velocity.y;

			rigidbody.MoveRotation(baseRotation);
			CheckCorePosition();
		}

		//rigidbody.drag = JellyCore.drag;
	}

	public void SetState(FlapperState newState) {
		state = newState;
		if (state == FlapperState.jelly || state == FlapperState.gaseous) {
			rigidbody.constraints = RigidbodyConstraints.None;
			if (coreCollider) {
				coreCollider.enabled = false;
			}
		} else { // solid
			rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			if (!isRoot) {
				coreCollider.enabled = true;
				coreCollider.center = transform.localRotation * collider.center + transform.position - core.transform.position;
			} else {
				localPos = transform.position - core.transform.position;
			}
		}
		/*if (state == FlapperState.gaseous) {
			rigidbody.drag = JellyCore.gaseousDrag;
		} else {
			rigidbody.drag = JellyCore.drag;
		}*/
	}
	void CheckCorePosition() {
		// This would cast rays only against colliders in layer 9.
		RaycastHit hit;
		// Does the ray intersect any objects in the player layer
		int layerMask = 1 << 9;

		// This would cast rays only against colliders in layer 8.
		// But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
		layerMask = ~layerMask;



		if (Vector3.Distance(core.transform.position, transform.position) > 2.5f) {
			if (Physics.Raycast(transform.position + Vector3.down, (core.transform.position - transform.position).normalized, out hit, 1, layerMask)
				//CAN BE REMOVED
				|| Physics.Raycast(transform.position + Vector3.up, (core.transform.position - transform.position).normalized, out hit, 1, layerMask)
				|| Physics.Raycast(transform.position + Vector3.left, (core.transform.position - transform.position).normalized, out hit, 1, layerMask)
				|| Physics.Raycast(transform.position + Vector3.right, (core.transform.position - transform.position).normalized, out hit, 1, layerMask)
				|| Physics.Raycast(transform.position + Vector3.back, (core.transform.position - transform.position).normalized, out hit, 1, layerMask)
				|| Physics.Raycast(transform.position + Vector3.forward, (core.transform.position - transform.position).normalized, out hit, 1, layerMask)
				) {

				lastGoodPosition.y = Mathf.Abs(lastGoodPosition.y);
				Vector3 force = (lastGoodPosition + Vector3.up * 6f) * JellyCore.cohesion / 7;
				Debug.DrawRay(transform.position, lastGoodPosition * 10, Color.red, 20, true);
				Debug.DrawRay(transform.position, Vector3.up * 10, Color.green, 20, true);
				rigidbody.AddForce(force);
			}
		} else if (Vector3.Distance(core.transform.position, transform.position) < 1f) {
			lastGoodPosition = (core.transform.position - transform.position);
			lastGoodPosition = lastGoodPosition.normalized;

			//Debug.Log(lastGoodPosition);
		}


	}
}
