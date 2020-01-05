using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObjects : MonoBehaviour {
	public float strenght = 500f;
	public AudioSource audioSource;
    Quaternion rotation;
	GameObject obj = null;
	ThrowableObject th;
	StateManager state;
	bool ready = true;
	bool release = false;

	void Start() {
		state = GetComponent<StateManager>();
        rotation = transform.rotation;
	}

	void Update() {
		if (obj && state.state != FlapperState.solid && (Input.GetButtonDown("Jump") || state.state == FlapperState.gaseous)) {
			if (state.state == FlapperState.gaseous) {
				release = true;
			}
			StartCoroutine(Throw());

		}
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
			obj.layer = 18;
			if (!release) {
				audioSource.Play();
				obj.GetComponent<Rigidbody>().AddForce((Vector3.up + (Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward)) * strenght, ForceMode.VelocityChange);
			}
		}

		yield return new WaitForSeconds(0.1f);
		if(obj) obj.layer = 12;
		obj = null;
		ready = true;
	}

	private void OnTriggerEnter(Collider other) {
		if (ready && other.gameObject.layer == 14 && obj == null && state.state == FlapperState.jelly) {
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
        transform.rotation = rotation;
    }
}
