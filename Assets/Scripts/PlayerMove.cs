﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JumpType {
	DilateShrink,
	Shrink,
	Dilate
}
public class PlayerMove : MonoBehaviour {
	public float speed = 5;
	public float jumpForce = 1;
	public float doubleJumpMultiplier = 1.5f;
	public float fallingBoost = 1;
	public float maxFallingSpeedForJumping = 0.5f;
	[Space]
	public JumpType jumpType;
	[Header("Dilate Shrink")]
	public float toShrinkWait = 0.7f;
	public float stopShrinkWait = 2f;
	public float jumpingWait = 1f;
	public float bonesForce;
	public float bonesForceUp;
	[Header("Shrink")]
	public float toShrinkWait1 = 0.7f;
	public float stopShrinkWait1 = 2f;
	public float jumpingWait1 = 1f;
	public float bonesForce1;
	public float bonesForceUp1;
	[Header("Dilate")]
	public float toShrinkWait2 = 0.7f;
	public float stopShrinkWait2 = 2f;
	public float jumpingWait2 = 1f;
	public float bonesForce2;
	public float bonesForceUp2;
	[Space]
	public Rigidbody Up;
	public Rigidbody Down, Left, Front, Right, Back;

	Rigidbody rigidbody;
	StateManager stateManager;
	public bool jumping, shrinking = false;
    public bool can_move=true;
	int shrinkage = 0;
	int shrinking_counter = 0;
	void Awake() {
		rigidbody = GetComponent<Rigidbody>();
		stateManager = GetComponent<StateManager>();
		jumping = false;
	}

	void Update() {
		if(can_move) rigidbody.MovePosition(rigidbody.position + (Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward) * speed * Time.deltaTime);

		if (shrinking_counter > 0) updateShrink();

		if (Input.GetButtonDown("Jump") && !shrinking && !jumping) {
			Shrink();
		}

		if (rigidbody.velocity.y < -0.1f) {
			rigidbody.AddForce(Vector3.up * -fallingBoost, ForceMode.Acceleration);
		}
		//if (!shrinking) shrinkage = 0;
	}

	void Shrink() {
		shrinking = true;
		shrinkage++;
		shrinking_counter = 6;
		StartCoroutine(toShrink());
		StartCoroutine(stopShrink());

		if (shrinkage == 3) shrinkage = 2;
		//if (shrinking_counter == 0 && shrinking && (Left.transform.position - rigidbody.position).magnitude < 0.285) shrinking = false;
		//Debug.Log((Left.transform.position - rigidbody.position).magnitude);
	}

	IEnumerator toShrink() {
		float wait = toShrinkWait;
		switch (jumpType) {
			case JumpType.Shrink:
				wait = toShrinkWait1;
				break;
			case JumpType.Dilate:
				wait = toShrinkWait2;
				break;
		}
		yield return new WaitForSeconds(wait + shrinkage * 0.02f);
		shrinking = false;
	}

	IEnumerator stopShrink() {
		float wait = stopShrinkWait;
		switch (jumpType) {
			case JumpType.Shrink:
				wait = stopShrinkWait1;
				break;
			case JumpType.Dilate:
				wait = stopShrinkWait2;
				break;
		}
		yield return new WaitForSeconds(wait + shrinkage * 0.02f);
		if (!shrinking) {
			jumping = true;
			if (stateManager.state != FlapperState.gaseous && rigidbody.velocity.y > -maxFallingSpeedForJumping) {
				if (shrinkage == 1) {
					rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
				} else {
					rigidbody.AddForce(Vector3.up * jumpForce * doubleJumpMultiplier, ForceMode.VelocityChange);
				}
			}
			shrinkage = 0;
			StartCoroutine(Jumping());
		}
	}

	IEnumerator Jumping() {
		/*float high = bonesForce * 100;
		switch (jumpType) {
			case JumpType.Shrink:
				high = bonesForce1 * 100;
				break;
			case JumpType.Dilate:
				high = bonesForce2 * 100;
				break;
		}
		/*Left.AddForce(Left.position + Vector3.down * high);
		Right.AddForce(Right.position + Vector3.down * high);
		Front.AddForce(Front.position + Vector3.down * high);
		Back.AddForce(Back.position + Vector3.down * high);*/
		//shrinking_counter--;
		float wait = jumpingWait;
		switch (jumpType) {
			case JumpType.Shrink:
				wait = jumpingWait1;
				break;
			case JumpType.Dilate:
				wait = jumpingWait2;
				break;
		}
		yield return new WaitForSeconds(wait);
		jumping = false;
	}

	void updateShrink() {
		float force = bonesForce * shrinkage;
		switch (jumpType) {
			case JumpType.Shrink:
				force = bonesForce1 * shrinkage;
				break;
			case JumpType.Dilate:
				force = bonesForce2 * shrinkage;
				break;
		}
		float forceUp = bonesForceUp * shrinkage;
		switch (jumpType) {
			case JumpType.Shrink:
				forceUp = bonesForceUp1 * shrinkage;
				break;
			case JumpType.Dilate:
				forceUp = bonesForceUp2 * shrinkage;
				break;
		}
		//Down.MovePosition(Down.position + Vector3.down * shrinkage * -high * Time.deltaTime);
		Up.MovePosition(Up.position + Vector3.up * forceUp * Time.deltaTime);
		Left.MovePosition(Left.position + Vector3.left * force * Time.deltaTime);
		Right.MovePosition(Right.position + Vector3.right * force * Time.deltaTime);
		Front.MovePosition(Front.position + Vector3.forward * force * Time.deltaTime);
		Back.MovePosition(Back.position + Vector3.back * force * Time.deltaTime);
		shrinking_counter--;
	}
}
