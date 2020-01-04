using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMove : MonoBehaviour {
	public Transform flapperModel;
    float speed=0;
    float timer = -.45f;
    public float velocity = 7f;
    float acceleration=4.5f;
    [Space]
	public float jumpForce = 1;
	public float doubleJumpMultiplier = 1.5f;
	public float solidJumpForce = 1;
	public float maxFallingSpeedForJumping = 0.5f;
	public float jumpingWait = 2f;
	//public float fallingBoost = 1;
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

	[Space]
	public AudioSource audioSource;

	void Awake() {
		rigidbody = GetComponent<Rigidbody>();
		collider = GetComponent<SphereCollider>();
		stateManager = GetComponent<StateManager>();
		jumping = false;
		shrinking = false;
		camera = FindObjectOfType<Camera>().transform;

    }

    void FixedUpdate() {
		if (canMove) {
			transform.rotation = Quaternion.Euler(new Vector3(0, camera.rotation.eulerAngles.y, 0));
			Vector3 right = Input.GetAxis("Horizontal") * transform.right * (canMoveX ? 1 : perpendicularMoveOnPush);
			Vector3 forward = Input.GetAxis("Vertical") * transform.forward * (canMoveZ ? 1 : perpendicularMoveOnPush);
            /*if (Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") != 0) {
				transform.position = Vector3.Lerp(transform.position, transform.position + (right + forward) / 1.3f, speed * Time.fixedDeltaTime);
                if(speed < startingSpeed*2.5f) speed += .05f;
			} else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
				transform.position = Vector3.Lerp(transform.position, transform.position + (right + forward), speed * Time.fixedDeltaTime);
                if (speed < startingSpeed * 2.5f) speed += .05f;

            }
            else
            {
                speed = startingSpeed;
            }*/
            if (Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") != 0)
            {
                timer += Time.fixedDeltaTime*acceleration; 
                speed = Mathf.Atan(timer) * velocity + 1;
                transform.position = Vector3.Lerp(transform.position, transform.position + (right + forward) * speed / 1.3f, Time.fixedDeltaTime * speed/10);
            }
            else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                timer += Time.fixedDeltaTime*acceleration;
                speed = Mathf.Atan(timer) *velocity+1;
                Debug.Log(speed);
                transform.position = Vector3.Lerp(transform.position, transform.position + (right + forward) *speed , Time.fixedDeltaTime * speed / 10);
            }
            else
            {
                timer = -.5f;
                speed = 0;
            }
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
		} else if (canMove) {
			if (Input.GetButtonUp("Jump") && !jumping) {
				StartCoroutine(JumpCoroutine());
			} else if (Input.GetButton("Jump") && !jumping) {
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
