using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public enum MoveType {
	Old,
	Accelerate,
	Rigidbody,
	SpeedVector
}

public class PlayerMove : MonoBehaviour {
	public MoveType moveType;
	public bool rotateWithCamera = true;
	public Transform flapperModel;
	public float speed = 7;
	float timer = -.45f;
	public float velocity = 7f;
	public float acceleration = 4.5f;
	public float speedVectorMultiplier = 1.5f;
	public float accelerationSpeedVector = 0.5f;
	[Space]
	public float jumpForce = 1;
	public float solidJumpForce = 1;
	public float maxFallingSpeedForJumping = 0.5f;
	public float jumpingWait = 2f;
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
	public bool canMove, canJump, canMoveX, canMoveZ = true;
	public float perpendicularMoveOnPush = 0.2f;
	[HideInInspector]
	public bool jumping, shrinking = false;

	[HideInInspector]
	public Rigidbody rigidbody;
	public SphereCollider collider;
	StateManager stateManager;
	float shrinkage = 1;
	Transform camera;

	Vector3 speedVector;
	Vector3 prevPos;

	[Space]
	public AudioSource audioSource;

    public VJHandler jsMovement;
    bool jump = false;
    bool startToJump = false;
    JumpButtonScript Jump_Trigger;
    void Awake() {
		rigidbody = GetComponent<Rigidbody>();
		collider = GetComponent<SphereCollider>();
		stateManager = GetComponent<StateManager>();
		jumping = false;
		shrinking = false;
		camera = FindObjectOfType<Camera>().transform;
		prevPos = transform.position;
	}

    void Start()
    {
        jsMovement = GameObject.Find("Joycon_container").GetComponent<VJHandler>();

        Jump_Trigger = GameObject.Find("Jump_Button").GetComponent<JumpButtonScript>();
    }
    
    
    void FixedUpdate() {
		if (canMove) {
			if (rotateWithCamera)
				transform.rotation = Quaternion.Euler(new Vector3(0, camera.rotation.eulerAngles.y, 0));
			Vector3 right = jsMovement.InputDirection.x * transform.right * (canMoveX ? 1 : perpendicularMoveOnPush);
			Vector3 forward = jsMovement.InputDirection.y * transform.forward * (canMoveZ ? 1 : perpendicularMoveOnPush);

			if (moveType == MoveType.Old) {
				if (jsMovement.InputDirection.x != 0 && jsMovement.InputDirection.y != 0) {
					transform.position = Vector3.Lerp(transform.position, transform.position + (right + forward) / 1.3f, speed * Time.fixedDeltaTime);
				} else if (jsMovement.InputDirection.x != 0 || jsMovement.InputDirection.y != 0) {
					transform.position = Vector3.Lerp(transform.position, transform.position + (right + forward), speed * Time.fixedDeltaTime);
				}
			} else if (moveType == MoveType.Rigidbody) {
				rigidbody.AddForce((right + forward) * velocity, ForceMode.VelocityChange);
			} else if (moveType == MoveType.Accelerate) {
				if (jsMovement.InputDirection.x != 0 && jsMovement.InputDirection.y != 0) {
					timer += Time.fixedDeltaTime * acceleration;
					speed = Mathf.Atan(timer) * velocity + 1;
					transform.position = Vector3.Lerp(transform.position, transform.position + (right + forward) * speed / 1.3f, Time.fixedDeltaTime * speed / 10);
				} else if (jsMovement.InputDirection.x != 0 || jsMovement.InputDirection.y != 0) {
					timer += Time.fixedDeltaTime * acceleration;
					speed = Mathf.Atan(timer) * velocity + 1;
					transform.position = Vector3.Lerp(transform.position, transform.position + (right + forward) * speed, Time.fixedDeltaTime * speed / 10);
				} else {
					timer = -.5f;
					speed = 0;
				}
			} else {
				Debug.Log(transform.position + "-" + prevPos + " -> " + ((transform.position - prevPos) / Time.fixedDeltaTime).magnitude);
				//Debug.Log(speedVector + " -> " + speedVector.magnitude);
				speedVector = (transform.position - prevPos) / Time.fixedDeltaTime;
				prevPos = transform.position;
				if (jsMovement.InputDirection.x != 0 || jsMovement.InputDirection.y != 0) {
					speedVector = Vector3.Lerp(speedVector, (right + forward) * speed * speedVectorMultiplier, accelerationSpeedVector);
					speedVector.y = 0;
					transform.position += speedVector * Time.fixedDeltaTime;
				} else {
					speedVector = Vector3.MoveTowards(speedVector, Vector3.zero, acceleration);
				}
			}
		}

		if (shrinking) {
			updateShrink();
		}
		if (stateManager.state == FlapperState.gaseous) {
			if (moveType == MoveType.Rigidbody) {
				if (Jump_Trigger.jumpButtonHold) {
					rigidbody.AddForce(Vector3.up * -0.45f, ForceMode.VelocityChange);
				} else {
					rigidbody.AddForce(Vector3.up * 0.3f, ForceMode.VelocityChange);
				}
			} else {
				rigidbody.isKinematic = true;
				if (Jump_Trigger.jumpButtonHold) {
					transform.position = Vector3.Lerp(transform.position, transform.position - Vector3.up * 10, gaseousShrinkDownForce / 10 * Time.deltaTime);
				} else {
					transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up, gaseousFloatUpForce * Time.deltaTime);
				}
				rigidbody.isKinematic = false;
			}
		} /*else if (canMove) {
			if (Input.GetButtonUp("Jump") && !jumping) {
				StartCoroutine(JumpCoroutine());
			} else
			if (Input.GetButton("Jump") && !jumping) {
				if (!shrinking) {
					shrinking = true;
				}

				if (stateManager.state == FlapperState.jelly && shrinkage <= max_shrinking) {
					Shrink();
				} else if (stateManager.state == FlapperState.solid) {
					StartCoroutine(JumpCoroutine());
				}
			} else {
				shrinking = false;
			}
		}*/
	}

	void Update() {
		if (stateManager.state != FlapperState.gaseous && canMove) {

            if (Jump_Trigger.jumpButtonRelease && !jumping) {
				StartCoroutine(JumpCoroutine());
                Jump_Trigger.jumpButtonRelease = false;
            }
            else if (Jump_Trigger.jumpButtonHold && !jumping) {
				if (!shrinking) {
					shrinking = true;
				}

				if (stateManager.state == FlapperState.jelly && shrinkage <= max_shrinking) {
					Shrink();
				} else if (stateManager.state == FlapperState.solid) {
					StartCoroutine(JumpCoroutine());
				}
			} else {
				shrinking = false;
			}
		}
	}

	void Shrink() {
		shrinkage += Time.deltaTime * shrink_velocity;

	}

	IEnumerator JumpCoroutine() {
		//Debug.Log(shrinking + " " + jumping);
		if (shrinking && !jumping && canJump) {
			jumping = true;
			shrinking = false;
			yield return 0;
			if (stateManager.state != FlapperState.gaseous) {
				if (stateManager.state == FlapperState.solid) {
					rigidbody.AddForce(Vector3.up * solidJumpForce, ForceMode.VelocityChange);
				} else {
					rigidbody.AddForce(Vector3.up * jumpForce * shrinkage, ForceMode.VelocityChange);
				}
			}
			shrinkage = 1;
			audioSource.Play();
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
}
