using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTouchingGround : MonoBehaviour {
	PlayerMove playerMove;
	int colliderCount = 0;

	void Awake() {
		playerMove = GetComponentInParent<PlayerMove>();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer != gameObject.layer) {
			colliderCount++;
			playerMove.canJump = colliderCount > 0;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.layer != gameObject.layer) {
			colliderCount--;
			playerMove.canJump = colliderCount > 0;
		}
	}
}
