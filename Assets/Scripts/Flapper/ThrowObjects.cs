using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		if (obj && state.state != FlapperState.solid && (Jump_Trigger.jumpButtonHold || state.state == FlapperState.gaseous)) {
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
		obj.GetComponent<ThrowableObject>().enabled = false;
		th.enabled = false;
		ready = false;
		if (th.isHandle) {
			transform.parent.SetParent(null);
			GetComponent<PlayerMove>().canMove = true;
		} else {
			obj.transform.parent = null;
			
			if (!release) {
				GetComponent<PlayerMove>().SetJumpText("Jump");
				audioSource.Play();
				obj.GetComponent<Rigidbody>().AddForce((Vector3.up + GetComponent<Rigidbody>().velocity.normalized) * strenght, ForceMode.Force);
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
			obj = other.transform.parent.gameObject;

			if (th.isHandle) {
				//other.transform.parent.gameObject.GetComponent<Rigidbody>().AddForce((Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward + Vector3.down / 2) * 100000 * Time.deltaTime);
				transform.parent.SetParent(other.transform.parent.gameObject.transform);

				this.GetComponent<PlayerMove>().canMove = false;

			} else {
				release = false;
				other.transform.parent.SetParent(transform);
			}
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
