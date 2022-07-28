using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SpawnManager;

public class ThrowObjects : MonoBehaviour {
	public float strenght = 500f;
	public AudioSource audioSource;
    Quaternion rotation;
	GameObject obj = null;
	[HideInInspector]
	public ThrowableObject th;

	StateManager state;
	public bool ready = true;
	bool release = false;
    public VJHandler jsMovement;
    JumpButtonScript Jump_Trigger;

    private void Start()
    {
        jsMovement = GameObject.Find("Joycon_container").GetComponent<VJHandler>();
        Jump_Trigger = GameObject.Find("Jump_Button").GetComponent<JumpButtonScript>();
        state = GetComponent<StateManager>();
        rotation = transform.rotation;
	}

	void Update() {
		if (ready && obj && state.state != FlapperState.solid && (Jump_Trigger.jumpButtonHold || state.state == FlapperState.gaseous)) {
			if (state.state == FlapperState.gaseous) {
				release = true;
			}
			StartCoroutine(Throw());

		}
	}
	public void ThrowObject()
    {
		StartCoroutine(Throw());
    }
	IEnumerator Throw() {
		
		ready = false;
		if (th.isHandle) {
			transform.parent.SetParent(null);
			GetComponent<PlayerMove>().canMove = true;
			obj.GetComponent<ThrowableObject>().enabled = false;
			th.enabled = false;
		} else {
			obj.GetComponent<ThrowableObject>().enabled = false;
			th.enabled = false;
			obj.transform.parent = GameObject.Find("Bolt").transform;
			
			if (!release) {
				GetComponent<PlayerMove>().SetJumpText("Jump");
				audioSource.Play();
				obj.GetComponent<Rigidbody>().AddForce((Vector3.up + GetComponent<Rigidbody>().velocity.normalized) * strenght, ForceMode.VelocityChange);
			}
			yield return new WaitForSeconds(.02f);
			obj.layer = 18;
		}

		yield return new WaitForSeconds(0.1f);
		if(obj) obj.layer = 12;
		obj = null;
		th = null;
		ready = true;
	}





	private void OnTriggerEnter(Collider other) {
		if (ready && other.gameObject.layer == 14 && obj == null && state.state == FlapperState.jelly) {
			GetComponent<PlayerMove>().SetJumpText("Throw");
			th = other.transform.parent.gameObject.GetComponent<ThrowableObject>();
			th.enabled = true;
			th.core = GetComponent<JellyCore>();
			th.state = GetComponent<StateManager>();
			obj = other.transform.parent.gameObject;

			if (th.isHandle) {
				//other.transform.parent.gameObject.GetComponent<Rigidbody>().AddForce((Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward + Vector3.down / 2) * 100000 * Time.deltaTime);
				transform.parent.SetParent(other.transform.parent.gameObject.transform);

				this.GetComponent<PlayerMove>().canMove = false;

			} else {
				release = false;
				other.transform.parent.SetParent(transform);
			}
			/*
			PhotonView _photonView = obj.GetComponent<PhotonView>();
			object[] data = new object[]
			{
						_photonView.ViewID
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
			PhotonNetwork.RaiseEvent((byte)RaiseEventCodes.BoltOwner, data, raiseEventOptions, sendOptions);
			*/
		}
	}
    private void OnTriggerExit(Collider other)
    {
		//if(obj!=null) obj.GetComponent<ThrowableObject>().enabled = false;
		//if(th!=null) th.enabled = false;
		//ready = false;
		transform.rotation = rotation;
    }
}
