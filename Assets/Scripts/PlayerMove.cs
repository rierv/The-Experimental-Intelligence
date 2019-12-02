using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMove : MonoBehaviour {
	public Transform flapperModel;
	public float speed = 5;
	[Space]
	public float jumpForce = 1;
	public float doubleJumpMultiplier = 1.5f;
	public float solidJumpForce = 1;
	public float maxFallingSpeedForJumping = 0.5f;
	public float jumpingWait = 2f;
	//public float fallingBoost = 1;
	public AudioClip shrink;
	[Space]
	public float gaseousFloatUpForce = 1;
	public float gaseousShrinkDownForce = 1;
	[Space]

	[Header("Shrink")]

	public float bonesForce;
	public float bonesForceUp;
	public float max_shrinking = 1.5f;
	public float shrink_velocity = 1f;

	[Space]
	public Rigidbody Up;
	public Rigidbody Down, Left, Front, Right, Back;
	public bool canMove, canJump = true;
	[HideInInspector]
	public bool jumping, shrinking = false;

	[HideInInspector]
	public Rigidbody rigidbody;
	public SphereCollider collider;
	StateManager stateManager;
	float shrinkage = 0;

	void Awake() {
		rigidbody = GetComponent<Rigidbody>();
		collider = GetComponent<SphereCollider>();
		stateManager = GetComponent<StateManager>();
		jumping = false;
		shrinking = false;
	}

	void FixedUpdate() {
		if (canMove) {

			if (Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") != 0) transform.position = Vector3.Lerp(transform.position, transform.position + (Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward) / 1.3f, speed * Time.fixedDeltaTime);
			else transform.position = Vector3.Lerp(transform.position, transform.position + (Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward), speed * Time.fixedDeltaTime);

		}
	}

	void Update() {
		if (shrinking) {
			updateShrink();
		}


		if (stateManager.state == FlapperState.gaseous) {
			rigidbody.isKinematic = true;
			if (Input.GetButton("Jump")) {
				transform.position = Vector3.Lerp(transform.position, transform.position - Vector3.up * 10, gaseousShrinkDownForce / 10 * Time.deltaTime);
			} else {
				transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up, gaseousFloatUpForce * Time.deltaTime);
			}
			rigidbody.isKinematic = false;
		} else {
			if (Input.GetButtonUp("Jump") && !jumping) {
				StartCoroutine(JumpCoroutine());
			} else if (Input.GetButton("Jump") && !jumping) {
				if (!shrinking) {
					shrinking = true;
				}

				if (stateManager.state == FlapperState.jelly && shrinkage <= max_shrinking) {
					Shrink();
				} else if (stateManager.state == FlapperState.solid || shrinkage > max_shrinking) {
					StartCoroutine(JumpCoroutine());
				}
			}
		}

	}

	void Shrink() {
		shrinkage += Time.deltaTime * shrink_velocity;
	}




	IEnumerator JumpCoroutine() {

		//Debug.Log(shrinking + " " + jumping);

		if (shrinking && !jumping) {
			jumping = true;
			shrinking = false;
			if (stateManager.state != FlapperState.gaseous && canJump) {
				if (stateManager.state == FlapperState.solid) {
					rigidbody.AddForce(Vector3.up * solidJumpForce, ForceMode.VelocityChange);
				} else {
					rigidbody.AddForce(Vector3.up * jumpForce * Mathf.Clamp(shrinkage, 1, float.MaxValue), ForceMode.VelocityChange);
				}
			}
			shrinkage = 0;
			yield return new WaitForSeconds(jumpingWait);
			jumping = false;
		}
	}



	public void updateShrink() {
		//AudioManager.singleton.PlayClip(shrink);

		float force = bonesForce * shrinkage;
		float forceUp = bonesForceUp * shrinkage;



		//Down.MovePosition(Down.position + Vector3.down * shrinkage * -high * Time.deltaTime);
		Up.MovePosition(Up.position + Vector3.up * forceUp * Time.deltaTime);
		Left.MovePosition(Left.position + Vector3.left * force * Time.deltaTime);
		Right.MovePosition(Right.position + Vector3.right * force * Time.deltaTime);
		Front.MovePosition(Front.position + Vector3.forward * force * Time.deltaTime);
		Back.MovePosition(Back.position + Vector3.back * force * Time.deltaTime);



	}
	/*
     * public enum JumpType {
	DilateShrink,
	Shrink,
	Dilate,
	Scale
    }
    public Transform flapperModel;
	public float speed = 5;
	[Space]
	public float jumpForce = 1;
	public float doubleJumpMultiplier = 1.5f;
	public float solidJumpForce = 1;
	public float maxFallingSpeedForJumping = 0.5f;
	public float jumpingWait = 1f;
	//public float fallingBoost = 1;
	public AudioClip shrink;
	[Space]
	public float gaseousFloatUpForce = 1;
	public float gaseousShrinkDownForce = 1;
	[Space]
	public JumpType jumpType;
	[Header("Dilate Shrink")]
	public float toShrinkWait = 0.7f;
	public float stopShrinkWait = 2f;
	public float bonesForce;
	public float bonesForceUp;
	[Header("Shrink")]
	public float toShrinkWait1 = 0.7f;
	public float stopShrinkWait1 = 2f;
	public float bonesForce1;
	public float bonesForceUp1;
	[Header("Dilate")]
	public float toShrinkWait2 = 0.7f;
	public float stopShrinkWait2 = 2f;
	public float bonesForce2;
	public float bonesForceUp2;
	[Header("Dilate")]
	public float toShrinkWait3 = 0.7f;
	public float stopShrinkWait3 = 2f;
	public float scaleXZ = 0.8f;
	public float scaleY = 0.6f;
	public float scaleSpeedXZ = 7;
	public float scaleSpeedY = 7;
	[Space]
	public Rigidbody Up;
	public Rigidbody Down, Left, Front, Right, Back;
	public bool canMove, canJump = true;
	[HideInInspector]
	public bool jumping, shrinking = false;

	[HideInInspector]
	public Rigidbody rigidbody;
	public SphereCollider collider;
	StateManager stateManager;
	int shrinkage = 0;
	int shrinking_counter = 0;

	void Awake() {
		rigidbody = GetComponent<Rigidbody>();
		collider = GetComponent<SphereCollider>();
		stateManager = GetComponent<StateManager>();
		jumping = false;
	}

	void FixedUpdate() {
		if (canMove) {

			if (Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") != 0) transform.position = Vector3.Lerp(transform.position, transform.position + (Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward) / 1.3f, speed * Time.fixedDeltaTime);
			else transform.position = Vector3.Lerp(transform.position, transform.position + (Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward), speed * Time.fixedDeltaTime);

			//rigidbody.MovePosition(rigidbody.position + (Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward) * speed * Time.fixedDeltaTime);
		}
	}

	void Update() {
		if (shrinking_counter > 0) {
			updateShrink();
		} else {
			flapperModel.localScale = new Vector3(Mathf.Lerp(flapperModel.localScale.x, 1, scaleSpeedXZ * Time.deltaTime), Mathf.Lerp(flapperModel.localScale.y, 1, scaleSpeedY * Time.deltaTime), Mathf.Lerp(flapperModel.localScale.z, 1, scaleSpeedXZ * Time.deltaTime));
		}

		if (stateManager.state == FlapperState.gaseous) {
			rigidbody.isKinematic = true;
			if (Input.GetButton("Jump")) {
				transform.position = Vector3.Lerp(transform.position, transform.position - Vector3.up * 10, gaseousShrinkDownForce / 10 * Time.deltaTime);
				//rigidbody.MovePosition(rigidbody.position + (Vector3.up * -gaseousShrinkDownForce * Time.deltaTime));
			} else {
				transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up, gaseousFloatUpForce * Time.deltaTime);
				//rigidbody.MovePosition(rigidbody.position + (Vector3.up * gaseousFloatUpForce * Time.deltaTime));
			}
			rigidbody.isKinematic = false;
		} else {
			if (Input.GetButtonDown("Jump") && !shrinking && !jumping) {
				if (stateManager.state == FlapperState.jelly && shrinkage < 2) {
					Shrink();
				} else if (stateManager.state == FlapperState.solid && shrinkage < 1) {
					StartCoroutine(JumpCoroutine());
				}
			}
		}

		//if (!shrinking) shrinkage = 0;
	}

	void Shrink() {
		shrinking = true;
		shrinkage++;
		shrinking_counter = 6;
		StartCoroutine(ShrinkCoroutine());
		StartCoroutine(JumpCoroutine());
		
	}
	public void justShrink() {
		shrinking = true;
		shrinkage++;
		shrinking_counter = 6;
		StartCoroutine(ShrinkCoroutine());
		jumping = false;
	}

	IEnumerator ShrinkCoroutine() {
		float wait = toShrinkWait;
		switch (jumpType) {
			case JumpType.Shrink:
				wait = toShrinkWait1;
				break;
			case JumpType.Dilate:
				wait = toShrinkWait2;
				break;
			case JumpType.Scale:
				wait = toShrinkWait3;
				break;
		}
		yield return new WaitForSeconds(wait + shrinkage * 0.02f);
		shrinking = false;
		if (!jumping) {
			yield return new WaitForSeconds(jumpingWait);
			shrinkage = 0;
		}


	}

	IEnumerator JumpCoroutine() {
		float wait = stopShrinkWait;
		switch (jumpType) {
			case JumpType.Shrink:
				wait = stopShrinkWait1;
				break;
			case JumpType.Dilate:
				wait = stopShrinkWait2;
				break;
			case JumpType.Scale:
				wait = stopShrinkWait3;
				break;
		}
		yield return new WaitForSeconds(wait + shrinkage * 0.02f);
		if (!shrinking && !jumping) {
			jumping = true;
			if (stateManager.state != FlapperState.gaseous && canJump) {
				if (stateManager.state == FlapperState.solid) {
					rigidbody.AddForce(Vector3.up * solidJumpForce, ForceMode.VelocityChange);
				} else if (shrinkage == 1) {
					rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
				} else {
					rigidbody.AddForce(Vector3.up * jumpForce * doubleJumpMultiplier, ForceMode.VelocityChange);
				}
			}
			shrinkage = 0;
			yield return new WaitForSeconds(jumpingWait);
			jumping = false;
		}
	}

	

	public void updateShrink() {
		//AudioManager.singleton.PlayClip(shrink);

		float force = bonesForce * shrinkage;
		float forceUp = bonesForceUp * shrinkage;
		switch (jumpType) {
			case JumpType.Shrink:
				force = bonesForce1 * shrinkage;
				forceUp = bonesForceUp1 * shrinkage;
				break;
			case JumpType.Dilate:
				force = bonesForce2 * shrinkage;
				forceUp = bonesForceUp2 * shrinkage;
				break;
			case JumpType.Scale:
				force = 0;
				forceUp = 0;
				break;
		}

		//Down.MovePosition(Down.position + Vector3.down * shrinkage * -high * Time.deltaTime);
		Up.MovePosition(Up.position + Vector3.up * forceUp * Time.deltaTime);
		Left.MovePosition(Left.position + Vector3.left * force * Time.deltaTime);
		Right.MovePosition(Right.position + Vector3.right * force * Time.deltaTime);
		Front.MovePosition(Front.position + Vector3.forward * force * Time.deltaTime);
		Back.MovePosition(Back.position + Vector3.back * force * Time.deltaTime);

		if (jumpType == JumpType.Scale) {
			flapperModel.localScale = new Vector3(Mathf.Lerp(flapperModel.localScale.x, scaleXZ, scaleSpeedXZ * Time.deltaTime), Mathf.Lerp(flapperModel.localScale.y, scaleY, scaleSpeedY * Time.deltaTime), Mathf.Lerp(flapperModel.localScale.z, scaleXZ, scaleSpeedXZ * Time.deltaTime));
		}

		shrinking_counter--;
	}*/
}
