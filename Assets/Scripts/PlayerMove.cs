using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
	public float force = 10;
	public float speed = 5;
	public float jumpForce = 1;
	[Space]
	public float toShrinkWait = 0.33f;
	public float stopShrinkWait = 0.66f;
	public float jumpingWait = 1f;

	bool jumping, shrinking = false;
	int shrinkage = 0;
	public Rigidbody Up, Down, Left, Front, Right, Back;
	Rigidbody rigidbody;
	int shrinking_counter = 0;

	void Start() {
		rigidbody = GetComponent<Rigidbody>();
		jumping = false;


	}

	void Update() {
		rigidbody.MovePosition(rigidbody.position + (Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward) * speed * Time.deltaTime);

		if (shrinking_counter > 0) updateShrink();

		if (Input.GetButtonDown("Jump") && !shrinking && !jumping) {
			Shrink();//rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		}
		//if (!shrinking) shrinkage = 0;

	}

	void Shrink() {
		shrinking = true;
		shrinkage++;
		shrinking_counter = 6;
		StartCoroutine("toShrink");
		StartCoroutine("stopShrink");

		if (shrinkage == 4) shrinkage = 3;
		//if (shrinking_counter == 0 && shrinking && (Left.transform.position - rigidbody.position).magnitude < 0.285) shrinking = false;
		Debug.Log((Left.transform.position - rigidbody.position).magnitude);

	}
	IEnumerator toShrink() {

		yield return new WaitForSeconds(toShrinkWait);
		shrinking = false;
	}
	IEnumerator stopShrink() {

		yield return new WaitForSeconds(stopShrinkWait);
		if (!shrinking) {
			jumping = true;
			Debug.Log("ciao");
			rigidbody.AddForce(Vector3.up * jumpForce * shrinkage, ForceMode.Impulse);
			shrinkage = 0;
			StartCoroutine("Jumping");
		}
	}
	IEnumerator Jumping() {

		yield return new WaitForSeconds(jumpingWait);
		jumping = false;
	}
	void updateShrink() {

		Up.MovePosition(Up.position + Vector3.down * shrinkage * 5 * Time.deltaTime);
		Down.MovePosition(Down.position + Vector3.up * shrinkage * 5 * Time.deltaTime);
		Left.MovePosition(Left.position + Vector3.left * shrinkage * 5 * Time.deltaTime);
		Right.MovePosition(Right.position + Vector3.right * shrinkage * 5 * Time.deltaTime);
		Front.MovePosition(Front.position + Vector3.forward * shrinkage * 5 * Time.deltaTime);
		Back.MovePosition(Back.position + Vector3.back * shrinkage * 5 * Time.deltaTime);
		shrinking_counter--;

	}
}
