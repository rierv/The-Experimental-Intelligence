using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
	public float minX = -100;
	public float maxX = 100;
	[Space]
	public float minY = 0;
	public float maxY = 0;

	JellyCore jellyCore;
	Rigidbody rigidbody;

	void Start() {
		jellyCore = FindObjectOfType<JellyCore>();
		rigidbody = GetComponent<Rigidbody>();
		transform.position = new Vector3(Mathf.Clamp(jellyCore.transform.position.x, minX, maxX), Mathf.Clamp(jellyCore.transform.position.y, minY, maxY), transform.position.z);
	}

	void Update() {
		Vector3 newPosition = new Vector3(Mathf.Clamp(jellyCore.transform.position.x, minX, maxX), Mathf.Clamp(jellyCore.transform.position.y, minY, maxY), transform.position.z);
		rigidbody.MovePosition(newPosition);
	}
}
