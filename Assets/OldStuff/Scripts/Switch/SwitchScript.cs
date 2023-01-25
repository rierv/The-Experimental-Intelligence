using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SwitchScript : MonoBehaviour {
	#region Attributes
	public Collider solidCollider;
	public GameObject switchBase;
	public GameObject targetObject;
	private float maxRotation = 90f;
	public GameObject handle;
	//[Range (0,1)]
	public float maxInclination = 0.5f;
	public float stickSpeed = 2f;
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
    FlapperCore Flapper;
    bool activateLeft = false;
    bool activateRight = false;
    public VJHandler jsMovement;


   
        //public AudioClip sound;
        #endregion

    private void Start() {
    jsMovement = GameObject.Find("Joycon_container").GetComponent<VJHandler>();

    Flapper = GameObject.FindObjectOfType<FlapperCore>();

    bonesActive = true;
	handle.GetComponent<ThrowableObject>().parentBodies.Add(handle.GetComponent<Rigidbody>());

	if (camera) cameraPointer = camera.transform.GetChild(0).transform;
	}

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
				camera.transform.position = Vector3.Lerp(camera.transform.position, cameraStartingPos, cameraMovementSpeed * Time.deltaTime);
				cameraPointer.transform.LookAt(targetObject.transform);
				camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, cameraPointer.transform.rotation, cameraRotationSpeed * Time.deltaTime);
			}
			float x = jsMovement.InputDirection.x;
			float y = jsMovement.InputDirection.x;



			if (vertical && horizontal)
				cursor.localPosition = new Vector3(x * maxInclination, cursor.localPosition.y, y * maxInclination);

			//	cursor.localPosition = new Vector3(Mathf.Clamp(cursor.localPosition.x + x, -maxInclination, maxInclination), cursor.localPosition.y, Mathf.Clamp(cursor.localPosition.z + y, -maxInclination, maxInclination));
			//else if (horizontal) cursor.localPosition = new Vector3(Mathf.Clamp(cursor.localPosition.x + x,-maxInclination, maxInclination), cursor.localPosition.y, cursor.localPosition.z);
			//else if (vertical) cursor.localPosition = new Vector3(cursor.localPosition.x, cursor.localPosition.y, Mathf.Clamp(cursor.localPosition.z + y,-maxInclination, maxInclination));
			else if (horizontal) pointer.LookAt(transform.position - x * Vector3.up + Vector3.right);
			else if (vertical) pointer.LookAt(transform.position - y * Vector3.up);


		} else {
			if (!bonesActive) {

                foreach (Transform o in Flapper.gameObject.transform.GetComponentInChildren<Transform>())
                    o.rotation = Quaternion.identity;//Quaternion.Lerp(Flapper.gameObject.transform.localRotation, Quaternion.identity, Time.deltaTime);

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
			if (comingBackToVerticalPos && horizontal && vertical) cursor.localPosition = new Vector3(0, cursor.localPosition.y, 0);


		}


		if ((horizontal && !vertical) || (!horizontal && vertical)) {

			transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(pointer.rotation.x * maxInclination, pointer.rotation.y, pointer.rotation.z, pointer.rotation.w), stickSpeed * Time.deltaTime);
			if (transform.rotation.x > .12f&&!activateRight) {
				targetObject.GetComponent<I_Activable>().Activate();
				PlayClip();
                activateRight = true;
			} else if (transform.rotation.x < -.12f&&!activateLeft) {
				targetObject.GetComponent<I_Activable>().Activate(false);
				PlayClip();
                activateLeft = true;

            } else if(transform.rotation.x < .12&& transform.rotation.x > -.12f){
				targetObject.GetComponent<I_Activable>().Deactivate();
				canPlayClip = true;
                activateRight = false;
                activateLeft = false;

            }
            if (comingBackToVerticalPos) {
				if (horizontal) pointer.LookAt(transform.position + Vector3.right);
				else if (vertical) pointer.LookAt(transform.position + Vector3.forward);
			}
		}

		if (vertical && horizontal) {
			pointer.LookAt(cursor.position);
			transform.rotation = Quaternion.Lerp(transform.rotation, pointer.rotation, stickSpeed * Time.deltaTime);
		}
	}

	bool canPlayClip = true;
	void PlayClip() {
		if (canPlayClip) {
			GetComponent<AudioSource>().Play();
			canPlayClip = false;
		}
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
