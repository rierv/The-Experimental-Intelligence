using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRetargeting : MonoBehaviour, I_Activable {
	bool active;
	public Camera camera;
	Transform cameraPointer;
	public Vector3 NewPosition;
	public Vector3 NewTarget;
	public float duration = 0f;
	public float cameraMovementSpeed = 1;
	public float cameraRotationSpeed = 1;

	public void Activate(bool type = true) {
		if (!active) {
			if (camera.GetComponent<CameraController>().isActiveAndEnabled)
				camera.GetComponent<CameraController>().enabled = false;
			active = true;
			if (duration > 0) StartCoroutine(waitToDeactivate());
		}
	}

	public void Deactivate() {
		camera.GetComponent<CameraController>().enabled = true;
		active = false;
	}

	private void Start() {
		if (camera) cameraPointer = camera.transform.GetChild(0).transform;
	}
	// Update is called once per frame
	void Update() {
		if (active) {
			if (NewPosition != Vector3.zero) camera.transform.position = Vector3.Lerp(camera.transform.position, NewPosition, cameraMovementSpeed * Time.deltaTime);
			cameraPointer.transform.LookAt(NewTarget);
			if (NewTarget != Vector3.zero) camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, cameraPointer.transform.rotation, cameraRotationSpeed * Time.deltaTime);
		}
	}
	IEnumerator waitToDeactivate() {
		yield return new WaitForSeconds(duration);
		Deactivate();
	}
}
