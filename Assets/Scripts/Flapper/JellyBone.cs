using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyBone : MonoBehaviour {
	public bool isRoot;
	public FlapperState state;
	public Vector3 offset = Vector3.zero;

	JellyCore core;
	Rigidbody coreRigidbody;
	Rigidbody rigidbody;
	SphereCollider collider;
	Quaternion baseRotation;

	SphereCollider coreCollider;
	Vector3 localPos;

	//Vector3 lastGoodPosition;

	bool notReached = false;

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
			coreCollider.material = collider.material;
			coreCollider.radius = collider.radius;
			coreCollider.enabled = false;
		}
	}

	float lastCoreSpeedY;
	void FixedUpdate() {
		if (state == FlapperState.solid) {
			if (isRoot) {
				transform.position = core.transform.position + localPos;
				//transform.rotation = core.transform.rotation;
			}
		} else {
			/*if (state == FlapperState.gaseous) {
				rigidbody.AddForce(Physics.gravity * -JellyCore.gaseousAntiGravity);
			}*/
			float acceleration = (coreRigidbody.velocity.y - lastCoreSpeedY) / Time.fixedDeltaTime;
			Vector3 force = (core.transform.position - transform.position + offset) * JellyCore.cohesion;
			if (acceleration < -0.1f) {
				// limit acceleration only when falling
				force.y = Mathf.Clamp(force.y, acceleration, float.MaxValue);
			}
			rigidbody.AddForce(force, ForceMode.Acceleration);
			lastCoreSpeedY = coreRigidbody.velocity.y;

			rigidbody.MoveRotation(baseRotation);
			CheckCorePosition();

			if (!isRoot) {
				transform.localPosition = new Vector3(
					Mathf.Clamp(transform.localPosition.x, JellyCore.minShift, JellyCore.maxShift),
					Mathf.Clamp(transform.localPosition.y, JellyCore.minDistance, JellyCore.maxDistance),
					Mathf.Clamp(transform.localPosition.z, JellyCore.minShift, JellyCore.maxShift)
					);
			}
		}

		//rigidbody.drag = JellyCore.drag;
		rigidbody.useGravity = state != FlapperState.gaseous;
	}

	public void SetState(FlapperState newState) {
		state = newState;
		if (state != FlapperState.solid) {
			rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			if (coreCollider) {
				coreCollider.enabled = false;
			}
		} else { // solid
			rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			if (!isRoot) {
				coreCollider.enabled = true;
				coreCollider.center = Quaternion.Inverse(core.transform.rotation) * ((transform.rotation * collider.center) + transform.position - core.transform.position);
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



		if (Vector3.Distance(core.transform.position, transform.position) > 3.5f) {
			if (Physics.Raycast(transform.position + Vector3.down, (core.transform.position - transform.position).normalized, out hit, 0.0001f, layerMask)
				//CAN BE REMOVED
				|| Physics.Raycast(transform.position + Vector3.up, (core.transform.position - transform.position).normalized, out hit, 0.0001f, layerMask)
				|| Physics.Raycast(transform.position + Vector3.left, (core.transform.position - transform.position).normalized, out hit, 0.0001f, layerMask)
				|| Physics.Raycast(transform.position + Vector3.right, (core.transform.position - transform.position).normalized, out hit, 0.0001f, layerMask)
				|| Physics.Raycast(transform.position + Vector3.back, (core.transform.position - transform.position).normalized, out hit, 0.0001f, layerMask)
				|| Physics.Raycast(transform.position + Vector3.forward, (core.transform.position - transform.position).normalized, out hit, 0.0001f, layerMask)
				|| notReached
				) {
				Debug.DrawRay(transform.position, Vector3.up * 10, Color.green, 20, true);
				notReached = false;
				//rigidbody.AddForce((Vector3.up * 10), ForceMode.Acceleration);
				//Debug.Log(lastGoodPosition);
				transform.LookAt(core.transform.position);
				gameObject.layer = 16;
				//rigidbody.isKinematic = true;
				transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition, 5f * Time.deltaTime);

				Debug.DrawRay(transform.position, (transform.forward * 2 + Vector3.up * 4) * 200f, Color.red, 3, true);
				//continues in elseif
			}
		} else if (Vector3.Distance(core.transform.position, transform.position) < 2f) {
			rigidbody.isKinematic = false;
			notReached = true;
			//lastGoodPosition = (core.transform.position - transform.position);
			gameObject.layer = 9;

		}


	}
}
