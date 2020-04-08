using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraNewOffset : MonoBehaviour {
	public Vector3 offset;
	public bool overwriteX;
	public bool overwriteY;
	public bool overwriteZ;

	CameraController cameraController;
	Vector3 previousOffset;

	void Awake() {
		cameraController = FindObjectOfType<CameraController>();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.GetComponent<JellyCore>()) {
			previousOffset = new Vector3(cameraController.xOffset, cameraController.yOffset, cameraController.zOffset);
			if (overwriteX)
				cameraController.xOffset = offset.x;
			if (overwriteY)
				cameraController.yOffset = offset.y;
			if (overwriteZ)
				cameraController.zOffset = offset.z;
			cameraController.positionOnFlapperX = false;
			cameraController.positionOnFlapperY = false;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.GetComponent<JellyCore>()) {
			if (overwriteX)
				cameraController.xOffset = previousOffset.x;
			if (overwriteY)
				cameraController.yOffset = previousOffset.y;
			if (overwriteZ)
				cameraController.zOffset = previousOffset.z;
			cameraController.positionOnFlapperX = false;
			cameraController.positionOnFlapperY = false;
		}
	}
}
