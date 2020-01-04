using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public float cameraSpeed = 0.3f;
	public float maxSpeed = 50;
	public float rotationSpeed = 8;
	public float rotationSpeedZoomOut = 1.5f;
	public float lookAtFlapperDelay = 1;
	[Space]
	public float positionOnFlapperMargin = 0.05f;
	public float rotationOnFlapperMargin = 1;
	[Space]
	public Transform pointer;
	private Transform target;
	public Transform secondTarget;

	public bool allowLookAtTarget = false;
	[Header("To lock an axis, set min and max to 0")]
	public float minX = -100;
	public float maxX = 100;
	public float xOffset = 0;
	[Space]
	public float minY = -100;
	public float maxY = 100;
	public float yOffset = 5;

	[Space]
	public float minZ = 0;
	public float maxZ = 0;
	public float zOffset = -10;

	public Vector3 secondTargetOffset = Vector3.zero;
	private Vector3 directionOnSecondTarget = Vector3.zero;

	[Space]
	public float initialDelay = 3;

	bool lookAtFlapper;
	bool rotateToFlapper;
	[HideInInspector]
	public bool positionOnFlapper;
	bool rotationOnFlapper;
	Vector3 initPosition;
	Quaternion initRotation;
	//Vector3 initOffset;
	Vector3 velocity = Vector3.zero;

	void Awake() {
		initPosition = transform.position;
		initRotation = transform.rotation;
		//initOffset = new Vector3(xOffset, yOffset, zOffset);

		if (target == null) target = FindObjectOfType<JellyCore>().transform;
		//transform.position=Vector3.Lerp(transform.position, new Vector3(Mathf.Clamp(target.transform.position.x, minX, maxX)+ xOffset, Mathf.Clamp(target.transform.position.y, minY, maxY) + yOffset, Mathf.Clamp(target.transform.position.z, minZ, maxZ) + zOffset), Time.deltaTime*cameraSpeed);
	}

	void Start() {
		StartCoroutine(InitialCoroutine());
	}
	IEnumerator InitialCoroutine() {
		yield return new WaitForSeconds(initialDelay);
		lookAtFlapper = true;
	}

	IEnumerator AllowRotateToFlapper() {
		yield return new WaitForSeconds(lookAtFlapperDelay);
		rotateToFlapper = true;
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown("joystick button 2")) { // xbox button X
			lookAtFlapper = !lookAtFlapper;
			if (!lookAtFlapper) {
				positionOnFlapper = false;
				rotationOnFlapper = false;
				rotateToFlapper = false;
			} else {
				StartCoroutine(AllowRotateToFlapper());
			}
		}
		if (lookAtFlapper) {
			Vector3 newPosition = new Vector3(Mathf.Clamp(target.transform.position.x, minX, maxX) + xOffset, Mathf.Clamp(target.transform.position.y, minY, maxY) + yOffset, Mathf.Clamp(target.transform.position.z, minZ, maxZ) + zOffset);
			//transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * cameraSpeed);
			if (positionOnFlapper) {
				transform.position = newPosition;
			} else {
				transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, cameraSpeed, maxSpeed);
				if (Vector3.Distance(transform.position, newPosition) < positionOnFlapperMargin) {
					positionOnFlapper = true;
				}
			}
			if (allowLookAtTarget && rotateToFlapper) {
				if (secondTarget != null) directionOnSecondTarget = secondTarget.position - target.transform.position;
				pointer.transform.LookAt(target.transform.position + directionOnSecondTarget + secondTargetOffset);
				//transform.rotation = Quaternion.Lerp(transform.rotation, pointer.rotation, cameraRotationSpeed * Time.deltaTime);
				if (rotationOnFlapper) {
					transform.rotation = pointer.rotation;
				} else {
					transform.rotation = Quaternion.RotateTowards(transform.rotation, pointer.rotation, rotationSpeed * Time.deltaTime);
					if (Quaternion.Angle(transform.rotation, pointer.rotation) < rotationOnFlapperMargin) {
						rotationOnFlapper = true;
					}
				}
			}
		} else {
			//transform.position = Vector3.Lerp(transform.position, initPosition, cameraSpeed * Time.deltaTime);
			transform.position = Vector3.SmoothDamp(transform.position, initPosition, ref velocity, cameraSpeed, maxSpeed);
			//transform.rotation = Quaternion.Lerp(transform.rotation, initRotation, cameraRotationSpeed * Time.deltaTime);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, initRotation, rotationSpeedZoomOut * Time.deltaTime);
		}
	}

	/*public void ResetOffset() {
		xOffset = initOffset.x;
		yOffset = initOffset.y;
		zOffset = initOffset.z;
	}*/
}
