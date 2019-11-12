using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
	public float speed = 5;
	public float jumpForce = 1;
	public float doubleJumpMultiplier = 1.5f;
	public float fallingBoost = 1;
	[Space]
	public float toShrinkWait = 0.7f;
	public float stopShrinkWait = 2f;
	public float jumpingWait = 1f;
	public Rigidbody Up, Down, Left, Front, Right, Back;
	public bool reverseShrink;
	public float maxFallingSpeedForJumping = 0.5f;

	Rigidbody rigidbody;
	StateManager stateManager;
	public bool jumping, shrinking = false;
    public bool can_move=true;
	int shrinkage = 0;
	int shrinking_counter = 0;
	public float _const;
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
		yield return new WaitForSeconds(toShrinkWait + shrinkage * 0.02f);
		shrinking = false;
	}

	IEnumerator stopShrink() {
		yield return new WaitForSeconds(stopShrinkWait + shrinkage * 0.02f);
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
		float high;
		high = _const * 100;
		if (reverseShrink) high = -high;
		Left.AddForce(Left.position + Vector3.down * high);
		Right.AddForce(Right.position + Vector3.down * high);
		Front.AddForce(Front.position + Vector3.down * high);
		Back.AddForce(Back.position + Vector3.down * high);
		//shrinking_counter--;
		yield return new WaitForSeconds(jumpingWait);
		jumping = false;
	}

	void updateShrink() {
		float high;
		high = _const + shrinkage * 7;
		if (reverseShrink) high = -high;
		//Down.MovePosition(Down.position + Vector3.down * shrinkage * -high * Time.deltaTime);
		//Up.MovePosition(Up.position + Vector3.up * shrinkage * -high/4 * Time.deltaTime);
		Left.MovePosition(Left.position + Vector3.left * high * Time.deltaTime);
		Right.MovePosition(Right.position + Vector3.right * high * Time.deltaTime);
		Front.MovePosition(Front.position + Vector3.forward * high * Time.deltaTime);
		Back.MovePosition(Back.position + Vector3.back * high * Time.deltaTime);
		shrinking_counter--;
	}
}
