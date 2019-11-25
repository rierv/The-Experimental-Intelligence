using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SwitchScript : MonoBehaviour {
	#region Attributes
	public Collider solidCollider;
	public GameObject switchBase;
	public GameObject targetObject;
	private float maxRotation = 90f;
	private float firstMinRotation; //Min rotation to activate first function
	private float firstMaxRotation; //Max rotation to activate first function; note this is necessary since angles are between 0 and 360
	private float secondMinRotation;
	private float secondMaxRotation;
	Vector3 currentRotation;
	Vector3 validPos;
	public float yOffset = 1;
	public GameObject handle;
	public float maxInclination = 5f;
	public float stickSpeed = 5f;
	public bool comingBackToVerticalPos = true;
	public bool vertical = true;
	public bool horizontal = true;
	bool bonesActive = true;
	public Transform pointer, cursor;
	public GameObject camera;
	public float xCameraOffset = 0f;
	public float yCameraOffset = 0f;
	public float zCameraOffset = 0f;
	Vector3 cameraStartingPos;
	Transform cameraPointer;
	public float cameraMovementSpeed = 3f;
	public float cameraRotationSpeed = 3f;
	#endregion
	// Start is called before the first frame update

	private void Start() {
		bonesActive = true;
		handle.GetComponent<ThrowableObject>().parentBodies.Add(handle.GetComponent<Rigidbody>());
		firstMaxRotation = maxRotation;
		firstMinRotation = maxRotation / 2;
		secondMinRotation = 360 - firstMaxRotation;
		secondMaxRotation = 360 - firstMinRotation;
		if (camera) cameraPointer = camera.transform.GetChild(0).transform;
	}

	// Update is called once per frame
	private void Update() {

		if (handle.GetComponent<ThrowableObject>().isActiveAndEnabled) {
			if (bonesActive) {
				SphereCollider[] bones = GameObject.Find("Root").GetComponentsInChildren<SphereCollider>();
				GameObject.Find("CORE").GetComponent<Rigidbody>().isKinematic = true;
				foreach (SphereCollider bone in bones) {
					bone.enabled = false;
					bone.gameObject.GetComponent<Rigidbody>().isKinematic = true;
				}
				bonesActive = false;
				if (vertical && horizontal) targetObject.GetComponent<I_Activable>().Activate();

				if (camera != null) {
					if (camera.GetComponent<CameraController>().isActiveAndEnabled) {
						camera.GetComponent<CameraController>().enabled = false;

						cameraStartingPos = new Vector3(camera.transform.position.x + xCameraOffset, camera.transform.position.y + yCameraOffset, camera.transform.position.z + zCameraOffset);
					}
				}
			}
			if (camera != null) {
				cameraPointer.transform.LookAt(targetObject.transform);
				camera.transform.position = Vector3.Lerp(camera.transform.position, cameraStartingPos, cameraMovementSpeed * Time.deltaTime);
				camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, cameraPointer.transform.rotation, cameraRotationSpeed * Time.deltaTime);
			}
			float x = Input.GetAxis("Horizontal");
			float y = Input.GetAxis("Vertical");

			if (y != 0 || x != 0) {
				x = x * Time.deltaTime * stickSpeed * 3;
				y = y * Time.deltaTime * stickSpeed * 3;
				if (vertical && horizontal)
					cursor.localPosition = new Vector3(Mathf.Clamp(cursor.localPosition.x + x, -maxInclination, maxInclination), cursor.localPosition.y, Mathf.Clamp(cursor.localPosition.z + y, -maxInclination, maxInclination));
				//else if (horizontal) cursor.localPosition = new Vector3(Mathf.Clamp(cursor.localPosition.x + x,-maxInclination, maxInclination), cursor.localPosition.y, cursor.localPosition.z);
				//else if (vertical) cursor.localPosition = new Vector3(cursor.localPosition.x, cursor.localPosition.y, Mathf.Clamp(cursor.localPosition.z + y,-maxInclination, maxInclination));
				else if (horizontal) pointer.LookAt(transform.position + Vector3.right - x * Vector3.up * maxInclination);
				else if (vertical) pointer.LookAt(transform.position + Vector3.forward - y * Vector3.up * maxInclination);
			}
		} else {
			if (!bonesActive) {
				SphereCollider[] bones = GameObject.Find("Root").GetComponentsInChildren<SphereCollider>();
				GameObject.Find("CORE").GetComponent<Rigidbody>().isKinematic = false;
				foreach (SphereCollider bone in bones) {
					bone.enabled = true;
					bone.gameObject.GetComponent<Rigidbody>().isKinematic = false;

				}
				bonesActive = true;
				if (vertical && horizontal) targetObject.GetComponent<I_Activable>().Deactivate();

				if (camera != null)
					camera.GetComponent<CameraController>().enabled = true;
			}
		}

		if ((horizontal && !vertical) || (!horizontal && vertical)) {
			if (pointer.rotation.eulerAngles.x > 300 && pointer.rotation.eulerAngles.x < 321)
				targetObject.GetComponent<I_Activable>().Activate();
			else if (pointer.rotation.eulerAngles.x < 60 && pointer.rotation.eulerAngles.x > 40)
				targetObject.GetComponent<I_Activable>().Activate(false);
			else
				targetObject.GetComponent<I_Activable>().Deactivate();
		}


		if (comingBackToVerticalPos) cursor.localPosition = Vector3.Lerp(cursor.localPosition, Vector3.up * 5, stickSpeed * Time.deltaTime);

		if (vertical && horizontal) pointer.LookAt(cursor.position);
		transform.rotation = Quaternion.Lerp(transform.rotation, pointer.rotation, stickSpeed * Time.deltaTime);
	}

	private void OnTriggerStay(Collider other) {
		JellyBone jellyBone = other.GetComponent<JellyBone>();
		if (jellyBone) {
			solidCollider.isTrigger = jellyBone.state != FlapperState.solid;
		}
	}
}



/*
 USING VECTORS 180° ROTATION BUG ON Y AXIS :'(
if (vertical && horizontal)
    if (Mathf.Abs(y) - Mathf.Abs(x) > 0)
        if (y > 0)
            if (x > 0)
                pointer.LookAt(transform.position + Vector3.forward + (x * Vector3.right - (x + y) * Vector3.up * maxInclination));
            else
                pointer.LookAt(transform.position + Vector3.forward + (x * Vector3.right - (-x + y) * Vector3.up * maxInclination));
        else
            if (x > 0)
                pointer.LookAt(transform.position + Vector3.forward + (x * -Vector3.right - (-x + y) * Vector3.up * maxInclination));

            else
                pointer.LookAt(transform.position + Vector3.forward + (x * -Vector3.right - (x + y) * Vector3.up * maxInclination));
    else
        if (x > 0)
            if (y > 0)
                pointer.LookAt(transform.position + Vector3.right + (-(x+ y) * Vector3.up * maxInclination + y * Vector3.forward));
            else
                pointer.LookAt(transform.position + Vector3.right + (-(x - y) * Vector3.up * maxInclination + y * Vector3.forward));
        else
            if (y > 0)
                pointer.LookAt(transform.position + Vector3.right + ((x - y) * -Vector3.up * maxInclination - y * Vector3.forward));
            else
                pointer.LookAt(transform.position + Vector3.right + ((x + y) * -Vector3.up * maxInclination - y* Vector3.forward));
else if (horizontal) pointer.LookAt(transform.position + Vector3.right - x * Vector3.up * maxInclination);
else if (vertical) pointer.LookAt(transform.position + Vector3.forward - y * Vector3.up * maxInclination);*/
