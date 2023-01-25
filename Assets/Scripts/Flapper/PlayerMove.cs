using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun;
using static SpawnManager;

public enum MoveType {
	Old,
	Accelerate,
	Rigidbody,
	SpeedVector
}

public class PlayerMove : MonoBehaviour {
	private ImageTracking IM;
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

	private ThrowObjects readyToThrow;


    void Awake() {
		IM = FindObjectOfType<ImageTracking>();
		rigidbody = GetComponent<Rigidbody>();
		collider = GetComponent<SphereCollider>();
		stateManager = GetComponent<StateManager>();
		jumping = false;
		shrinking = false;
		camera = FindObjectOfType<Camera>().transform;
		prevPos = transform.position;
	}
    private void OnEnable()
    {
		jsMovement = GameObject.Find("Joycon_container").GetComponent<VJHandler>();

		Jump_Trigger = GameObject.Find("Jump_Button").GetComponent<JumpButtonScript>();

		readyToThrow = GetComponent<ThrowObjects>();
	}
   
    
    
    void FixedUpdate() {
		if (canMove) {
            
            if (rotateWithCamera)
				transform.rotation = Quaternion.Euler(new Vector3(0, camera.rotation.eulerAngles.y, 0));
			Vector3 right = jsMovement.InputDirection.x * Vector3.right * (canMoveX ? 1 : perpendicularMoveOnPush);
			Vector3 forward = jsMovement.InputDirection.y * Vector3.forward * (canMoveZ ? 1 : perpendicularMoveOnPush);
			if (jsMovement.InputDirection.x == 0 && jsMovement.InputDirection.y == 0)
            {
				rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, new Vector3 (0, rigidbody.velocity.y, 0), Time.fixedDeltaTime);
            }

			rigidbody.AddForce((right+forward)  * velocity, ForceMode.VelocityChange);
			rigidbody.velocity = new Vector3(Mathf.Clamp(rigidbody.velocity.x, -.11f, .11f), Mathf.Clamp(rigidbody.velocity.y, -.5f, 10f), Mathf.Clamp(rigidbody.velocity.z, -.11f, .11f));

		}


		if (stateManager.state == FlapperState.gaseous) {
			
				if (Jump_Trigger.jumpButtonHold) {
					if (rigidbody.velocity.y > 0) rigidbody.velocity= new Vector3 (rigidbody.velocity.x, 0, rigidbody.velocity.z);
					rigidbody.AddForce(Vector3.up * -gaseousShrinkDownForce, ForceMode.VelocityChange);
				} else {
					if (rigidbody.velocity.y < 0) rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
					rigidbody.AddForce(Vector3.up * gaseousFloatUpForce, ForceMode.VelocityChange);
				}
			
		} 
	}

	void Update() {
		if (stateManager.state != FlapperState.gaseous && canMove) {

            if (Jump_Trigger.jumpButtonHold && !readyToThrow.th && !jumping && canJump) {
				StartCoroutine(JumpCoroutine());
                Jump_Trigger.jumpButtonRelease = false;
            }
			
            else {
				shrinking = false;
                //shrinkage = 1;
            }
        }
	}

	void Shrink() {
		shrinkage += shrink_velocity;
	}

	IEnumerator JumpCoroutine() {
		//Debug.Log(shrinking + " " + jumping);
		Shrink();
		updateShrink();
		jumping = true;
		shrinking = false;
		yield return new WaitForSeconds(.04f);
		
		audioSource.Play();
		if (stateManager.state != FlapperState.gaseous)
		{

			if (stateManager.state == FlapperState.solid)
			{
				rigidbody.AddForce(Vector3.up * solidJumpForce, ForceMode.VelocityChange);
			}
			else
			{
				rigidbody.AddForce(Vector3.up * jumpForce * shrinkage, ForceMode.VelocityChange);
			}

		}

		yield return new WaitForSeconds(.6f);
		jumping = false;
		shrinkage = 1;
		
	}

	public void updateShrink() {
		//AudioManager.singleton.PlayClip(shrink);
		float force = bonesForce * shrinkage/2;
		float forceUp = bonesForceUp * shrinkage;

		//Down.MovePosition(Down.position + Vector3.down * shrinkage * -high * Time.deltaTime);
		Up.AddForce( transform.up * forceUp);
		Left.AddForce(-transform.right * force);
		Right.AddForce(transform.right * force);
		Front.AddForce(transform.forward * force);
		Back.AddForce(-transform.forward * force);
	}

	bool cantRestart = false;
    private void OnCollisionEnter(Collision collision)
    {
		if (collision.gameObject.tag == "Platform")
		{
			if (transform.parent.parent != collision.gameObject.transform)
			{
				if(collision.gameObject.GetComponent<MovingPlatformKinematic>())
					transform.parent.parent = collision.gameObject.transform.parent;
				else
					transform.parent.parent = collision.gameObject.transform;
			}
			cantRestart = false;
		}
		else if (collision.gameObject.tag == "Laser")
		{
			object[] data = new object[]
			{
						GetComponentInParent<PhotonView>().ViewID

			};

			RaiseEventOptions raiseEventOptions = new RaiseEventOptions
			{
				Receivers = ReceiverGroup.Others,
				CachingOption = EventCaching.AddToRoomCache

			};


			SendOptions sendOptions = new SendOptions
			{
				Reliability = true
			};

			//Raise Events!
			PhotonNetwork.RaiseEvent((byte)RaiseEventCodes.GaseousTransformation, data, raiseEventOptions, sendOptions);
			stateManager.temperature = 1;
		}
		else if (collision.gameObject.tag == "Finish")
		{
			if (!cantRestart)
			{
				cantRestart = true;
				shrinkage = 3f;
				updateShrink();
				shrinkage = 1f;
			}
			FindObjectOfType<ImageTrackingMultiplayer>().Reset(transform);

		}
	}
    private void OnCollisionExit(Collision collision)
    {
		if (collision.gameObject.tag == "Platform")
		{
			if (transform.parent.parent != collision.gameObject.transform) transform.parent.parent = collision.gameObject.transform.parent;
			canJump = false;
		}
	}
    private void OnCollisionStay(Collision collision)
    {
		if (collision.gameObject.tag == "Platform")
		{
			canJump = true;
		}
	}
	public void SetJumpText(string text)
    {
		Jump_Trigger.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }
}
