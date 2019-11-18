using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownPlatform : MonoBehaviour, I_Activable {
	bool active = false;
	int cCount = 0;
	Vector3 StartPos;
	public float speed = 0.0001f;
	public float fallingSpeed = 0f;
	public float duration = 0;
	bool ready = true;
	// Start is called before the first frame update
	void Start() {
		StartPos = transform.position;
		ready = true;
	}

	// Update is called once per frame
	void Update() {
		if (active) {

			//get the objects current position and put it in a variable so we can access it later with less code
			Vector3 pos = transform.position;
			//calculate what the new Y position will be
			float newY = Time.deltaTime * speed;
			//set the object's Y to the new calculated Y
			transform.position = new Vector3(pos.x, pos.y + newY, pos.z);
		} else
			transform.position = Vector3.Lerp(transform.position, StartPos, Time.deltaTime * fallingSpeed);
		if (Vector3.Distance(transform.position, StartPos) < 5f) ready = true;
	}

	public void Activate() {
		if (ready) StartCoroutine(Move());
		ready = false;
	}
	IEnumerator Move() {
		active = true;
		cCount++;
		yield return new WaitForSeconds(duration);
		if (cCount == 1) active = false;
		cCount--;
	}

	public void Deactivate() { }
}
