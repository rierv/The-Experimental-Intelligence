using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotScript : MonoBehaviour {

	#region Attributes
	Transform pointer;
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
	ParticleSystem[] particles;
	bool rotate = false;
	float timer = 0;
	public bool moveOnXAxis = false;
	Vector3 StartPos;
	#endregion

	private void Start() {
		pointer = new GameObject("pointer").transform;
		pointer.parent = this.transform.parent;
		pointer.position = transform.position;
		GameObject o = GameObject.Find("CORE");
		if (o != null) flapper = o.GetComponent<Transform>();
		particles = GetComponentsInChildren<ParticleSystem>();
		robot = gameObject.transform;
		currentRotationScale = rotationScale;
		currentSpeed = speed;
		currPos = robot.localPosition;
		StartPos = transform.localPosition;
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
				GameObject o = GameObject.Find("CORE");
				if (o != null)
				{
					flapper = o.GetComponent<Transform>();
					pointer.position = flapper.position;
					if (!moveOnXAxis)
					{
						if (pointer.localPosition.z < StartPos.z)
						{
							rotate = false;
							body.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
						}
						else if (pointer.localPosition.z > StartPos.z)
						{
							rotate = true;
							body.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
						}
					}
                    else
                    {
						if (pointer.localPosition.x < StartPos.x)
						{
							rotate = false;
							body.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
						}
						else if (pointer.localPosition.x > StartPos.x)
						{
							rotate = true;
							body.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
						}
					}
				}
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
		float oldDistance = Vector3.Distance(robot.position, flapper.position);
		
		if (!moveOnXAxis)
		{
			pointer.localPosition = robot.localPosition + Vector3.right / 80;
			if (Vector3.Distance(pointer.position, flapper.position) > oldDistance)
				pointer.localPosition = robot.localPosition - Vector3.right / 40;
			if (Vector3.Distance(pointer.position, flapper.position) < oldDistance)
			{
				currPos.x = Mathf.Clamp(pointer.localPosition.x, minLockerZ, maxLockerZ);
				robot.localPosition = Vector3.Lerp(tmp, currPos, Time.deltaTime * currentSpeed);
				tmp = currPos - tmp;
				wheel.Rotate(currentRotationScale * tmp.x, 0f, 0f, Space.Self);
			}
			pointer.position = flapper.position;
			if (pointer.localPosition.z < StartPos.z && rotate)
			{
				timer += Time.deltaTime;
				body.localRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(180, 0, timer * rotationSpeed)));
				if (body.localRotation.eulerAngles.z == 0)
				{
					rotate = false;
					timer = 0;
				}
			}
			else if (pointer.localPosition.z > StartPos.z && !rotate)
			{
				timer += Time.deltaTime;
				body.localRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(0, 180, timer * rotationSpeed)));
				if (body.localRotation.eulerAngles.z == 180)
				{
					rotate = true;
					timer = 0;
				}
			}
		}
        else
        {
			pointer.localPosition = robot.localPosition + Vector3.forward / 80;
			if (Vector3.Distance(pointer.position, flapper.position) > oldDistance)
				pointer.localPosition = robot.localPosition - Vector3.forward / 40;
			if (Vector3.Distance(pointer.position, flapper.position) < oldDistance)
			{
				currPos.z = Mathf.Clamp(pointer.localPosition.z, minLockerZ, maxLockerZ);
				robot.localPosition = Vector3.Lerp(tmp, currPos, Time.deltaTime * currentSpeed);
				tmp = currPos - tmp;
				wheel.Rotate(currentRotationScale * tmp.z, 0f, 0f, Space.Self);
			}
			pointer.position = flapper.position;
			if (pointer.localPosition.x < StartPos.x && rotate)
			{
				timer += Time.deltaTime;
				body.localRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(180, 0, timer * rotationSpeed)));
				if (body.localRotation.eulerAngles.z == 0)
				{
					rotate = false;
					timer = 0;
				}
			}
			else if (pointer.localPosition.x > StartPos.x && !rotate)
			{
				timer += Time.deltaTime;
				body.localRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(0, 180, timer * rotationSpeed)));
				if (body.localRotation.eulerAngles.z == 180)
				{
					rotate = true;
					timer = 0;
				}
			}
		}

	}
}