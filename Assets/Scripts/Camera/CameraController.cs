using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 1;
    public Transform pointer;
    private Transform target;
    public Transform secondTarget;

    public bool allowLookAtTarget=false;
    [Header("To lock an axis, freeze set 0 to min and max")]
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
	Vector3 initPosition;
	Quaternion initRotation;

    void Awake()
    {
		initPosition = transform.position;
		initRotation = transform.rotation;

        if(target==null) target = FindObjectOfType<JellyCore>().transform;
        //transform.position=Vector3.Lerp(transform.position, new Vector3(Mathf.Clamp(target.transform.position.x, minX, maxX)+ xOffset, Mathf.Clamp(target.transform.position.y, minY, maxY) + yOffset, Mathf.Clamp(target.transform.position.z, minZ, maxZ) + zOffset), Time.deltaTime*cameraSpeed);
    }
	
	void Start() {
		StartCoroutine(InitialCoroutine());
	}
	IEnumerator InitialCoroutine() {
		yield return new WaitForSeconds(initialDelay);
		lookAtFlapper = true;
	}

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown("joystick button 2")) { // xbox button X
			lookAtFlapper = !lookAtFlapper;
		}
		if (lookAtFlapper) {
			Vector3 newPosition = new Vector3(Mathf.Clamp(target.transform.position.x, minX, maxX) + xOffset, Mathf.Clamp(target.transform.position.y, minY, maxY) + yOffset, Mathf.Clamp(target.transform.position.z, minZ, maxZ) + zOffset);
			transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * cameraSpeed);
			if (allowLookAtTarget)
			{
				if (secondTarget != null) directionOnSecondTarget =  secondTarget.position - target.transform.position;
				pointer.transform.LookAt(target.transform.position + directionOnSecondTarget + secondTargetOffset);
				transform.rotation = Quaternion.Lerp(transform.rotation, pointer.rotation, cameraSpeed * Time.deltaTime);
			}
		} else {
			transform.position = Vector3.Lerp(transform.position, initPosition, cameraSpeed * Time.deltaTime);
			transform.rotation = Quaternion.Lerp(transform.rotation, initRotation, cameraSpeed * Time.deltaTime);
		}
    }
}
