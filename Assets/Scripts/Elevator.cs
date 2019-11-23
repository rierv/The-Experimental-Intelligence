using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour, I_Activable {
	public float activeY = 3;
	public float speed = 1;
	float inactiveY = 1;
	float targetY = 1;
	Rigidbody rigidbody;
	[Header("Activable")]
	public bool invertTrueFalse;

	void Awake() {
		rigidbody = GetComponent<Rigidbody>();
		inactiveY = transform.position.y;
		targetY = inactiveY;
	}

	void Update() {
		float y = transform.position.y;
		if (y < targetY) {
			y = Mathf.Clamp(y + Time.deltaTime * speed, float.MinValue, targetY);
		} else if (y > targetY) {
			y = Mathf.Clamp(y - Time.deltaTime * speed, targetY, float.MaxValue);
		}
		rigidbody.MovePosition(new Vector3(transform.position.x, y, transform.position.z));
	}

	public void Activate(bool type) {
		if (type && !invertTrueFalse || !type && invertTrueFalse) {
			targetY = activeY;
		} else {
			Deactivate();
		}
	}

	public void Deactivate() {
		targetY = inactiveY;
	}
}
