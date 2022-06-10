using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotScript : MonoBehaviour {

	#region Attributes
	public Transform pointer;
	private Transform robot;
	private Transform flapper;
	public Transform body;
	public float rotationSpeed;
	public Transform wheel;
	public float maxLockerZ;
	public float minLockerZ;
	private Vector3 currPos;
	private Vector3 tmp;
	public Light light;
	public float rotationScale = 12.5f;
	public float rotationScaleSolid = 2.5f;
	public float stunnTime = 5f;
	float stunTimeTemp = 0;
	public float speed = 5f;
	public float speedSolid = 1f;
	bool stop = false;
	private float currentSpeed;
	private float currentRotationScale;
	public bool zAxis_xAxis = true;
	ParticleSystem[] particles;
	#endregion

	private void Start() {
		flapper = GameObject.Find("CORE").GetComponent<Transform>();
		particles = GetComponentsInChildren<ParticleSystem>();
		robot = gameObject.transform;
		currentRotationScale = rotationScale;
		currentSpeed = speed;
		currPos = robot.localPosition;
		ManageRobotPosition();
	}

	private void Update() {
		
		

		if (stop) {
			stunTimeTemp -= Time.deltaTime;
			if (stunTimeTemp <= 0) {
				stop = false;
				light.enabled = true;
			}
		} else {
            if (flapper) ManageRobotPosition();
			else
            {
				flapper = GameObject.Find("CORE").GetComponent<Transform>();
			}
		}
	}

	private void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.layer == 12) {
			if (!stop) {
				GetComponent<AudioSource>().Play();
			}
			stop = true;
			light.enabled = false;
			stunTimeTemp = stunnTime;
			foreach (ParticleSystem p in particles) {
				p.Play();
			}
			//StartCoroutine(RobotStop());
			/*} else if (collision.gameObject.layer == 9) {
				state = GameObject.Find("Flapper model").GetComponent<JellyBone>().state;
				if (state == FlapperState.solid) {
					collidingWithSolid = true;
					currentSpeed = speedSolid;
					currentRotationScale = rotationScaleSolid;
				}*/
		}
	}

	/*IEnumerator RobotStop() {
		yield return new WaitForSeconds(stunnTime);
		stop = false;
		light.enabled = true;
	}*/
	private void ManageRobotPosition() {
		tmp = robot.localPosition;
		if (zAxis_xAxis) {
			float oldDistance = Vector3.Distance(robot.position, flapper.position);

			pointer.localPosition = robot.localPosition+Vector3.forward/80;
			if(Vector3.Distance(pointer.position, flapper.position)>oldDistance)
				pointer.localPosition = robot.localPosition - Vector3.forward / 40;
			if (Vector3.Distance(pointer.position, flapper.position) < oldDistance)
			{
				currPos.z = Mathf.Clamp(pointer.localPosition.z, minLockerZ, maxLockerZ);
				robot.localPosition = Vector3.Lerp(tmp, currPos, Time.deltaTime * currentSpeed);
				tmp = currPos - tmp;
				wheel.Rotate(0f, 0f, currentRotationScale * tmp.z, Space.Self);
			}
			if (flapper.position.x < transform.position.x) {
				body.localRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(body.localRotation.eulerAngles.z, 0, rotationSpeed * Time.deltaTime)));
			} else {
				body.localRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(body.localRotation.eulerAngles.z, 180, rotationSpeed * Time.deltaTime)));
			}
		} else {
			
			currPos.x = Mathf.Clamp(flapper.position.x, minLockerZ, maxLockerZ);
			
			robot.position = Vector3.Lerp(tmp, currPos, Time.deltaTime * currentSpeed);
			tmp = currPos - tmp;
			wheel.Rotate(0f, 0f, currentRotationScale * tmp.z, Space.Self);

			if (flapper.position.z < transform.position.z) {
				body.localRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(body.localRotation.eulerAngles.z, 0, rotationSpeed * Time.deltaTime)));
			} else {
				body.localRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(body.localRotation.eulerAngles.z, 180, rotationSpeed * Time.deltaTime)));
			}
		}
	}
}