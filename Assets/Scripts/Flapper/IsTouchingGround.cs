﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTouchingGround : MonoBehaviour {
	PlayerMove playerMove;
	int colliderCount = 0;
	Vector3 offset;
    bool waitForLanding = false;
    public float coyoteTime = 0.05f;

    void Awake() {
		playerMove = GetComponentInParent<PlayerMove>();
		offset = transform.localPosition;
	}

	private void Update() {
		transform.position = transform.parent.position + offset;
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer != gameObject.layer && other.gameObject.layer != 14) {
			colliderCount++;
			ApplyChanges();
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.layer != gameObject.layer && other.gameObject.layer != 14) {
			colliderCount--;
			ApplyChanges();
		}
	}

	void ApplyChanges() {
		if (colliderCount > 0) {
           //Debug.Log("HERE");
			playerMove.canJump = true;
			playerMove.collider.material.frictionCombine = PhysicMaterialCombine.Average;
            StopAllCoroutines();
		} else {
            //Debug.Log("HERE2");
            playerMove.collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
            StartCoroutine(StopJumping());
		}
	}
    IEnumerator StopJumping()
    {
        waitForLanding = true;
        yield return new WaitForSeconds(coyoteTime);
        waitForLanding = false;
        playerMove.canJump = false;
    }
}
