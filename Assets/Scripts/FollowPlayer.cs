using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
	[Header("To lock an axis, freeze Rigidbody position")]
	public float minX = -100;
	public float maxX = 100;
	[Space]
	public float minY = -100;
	public float maxY = 100;

	JellyCore jellyCore;
	Rigidbody rigidbody;

	void Start() {
		jellyCore = FindObjectOfType<JellyCore>();
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.MovePosition(new Vector3(Mathf.Clamp(jellyCore.transform.position.x, minX, maxX), Mathf.Clamp(jellyCore.transform.position.y, minY, maxY), transform.position.z));
	}

	void Update() {
		Vector3 newPosition = new Vector3(Mathf.Clamp(jellyCore.transform.position.x, minX, maxX), Mathf.Clamp(jellyCore.transform.position.y, minY, maxY), transform.position.z);
		rigidbody.MovePosition(newPosition);
	}
}
