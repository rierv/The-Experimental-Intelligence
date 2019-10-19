using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
	public float force = 10;
	public float speed = 5;
	public float jumpForce = 20;

	Rigidbody rigidbody;

	void Start() {
		rigidbody = GetComponent<Rigidbody>();
	}

	void Update() {
		rigidbody.MovePosition(rigidbody.position + (Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward) * speed * Time.deltaTime);
		if (Input.GetButtonDown("Jump")) {
			rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		}
	}
}
