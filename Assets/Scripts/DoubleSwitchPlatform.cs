using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleSwitchPlatform : MonoBehaviour, I_Activable {
	#region Attributes
	public bool xLock = false;
	public bool zLock = false;
	public bool yLock = false;

	public float maxConstraintX;
	public float minConstraintX;
	public float maxConstraintZ;
	public float minConstraintZ;
	public float maxConstraintY;
	public float minConstraintY;
	private Vector3 dir;
	public bool isActive = false;
	List<Transform> platforms;
	[Range(0, 3)]
	public int numberOfPlatforms = 2;
	public float[] velocities = new float[3];
	public GameObject obstacle;
	public Material activeMaterial;
	public Material notActiveMaterial;

	bool activable = true;

	#endregion
	private void Start() {
		platforms = new List<Transform>();
		foreach (Transform child in transform.GetComponentsInChildren<Transform>()) {
			if (child.gameObject.tag == "Platform") platforms.Add(child);
		}

	}
	public void Activate(bool type = true) {
		if (activable) isActive = true;
	}


	public void Deactivate() {
		isActive = false;
	}

	private void FixedUpdate() {
		if (isActive) {
			for (int i = 0; i < numberOfPlatforms; i++) {
				Transform platform = platforms[i];
				if (!xLock && !zLock)
					yLock = true;
				if (yLock) {
					if (!xLock && !zLock)
						dir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
					else if (!xLock)
						dir = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
					else if (!zLock)
						dir = new Vector3(0f, 0f, Input.GetAxis("Vertical"));
				} else {
					if (xLock && zLock)
						dir = new Vector3(0f, Input.GetAxis("Horizontal") + Input.GetAxis("Vertical"), 0);
					else if (!xLock)
						dir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
					else if (!zLock)
						dir = new Vector3(0f, Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
				}
				dir = dir * velocities[i];
				FixDirection(platform);
				if (Vector3.Distance(platform.transform.position, obstacle.transform.position) > 2) {
					if (i == 0) platform.position = Vector3.Lerp(platform.position, platform.position + dir, Time.fixedDeltaTime);
					if (i == 1) platform.position = Vector3.Lerp(platform.position, platform.position + dir / 2, Time.fixedDeltaTime);
				}
			}
		}
		/*foreach (Transform platform in platforms)
        {
            if (Vector3.Distance(platform.transform.position, obstacle.transform.position) > 2)
                platform.GetComponent<MeshRenderer>().material = activeMaterial;

            else platform.GetComponent<MeshRenderer>().material = notActiveMaterial;
        }*/
	}

	private void FixDirection(Transform platform) {
		if ((platform.position.x >= maxConstraintX && dir.x > 0) || (platform.position.x <= minConstraintX && dir.x < 0))
			dir.x = 0;
		if ((platform.position.z >= maxConstraintZ && dir.z > 0) || (platform.position.z <= minConstraintZ && dir.z < 0))
			dir.z = 0;
		if ((platform.position.y >= maxConstraintY && dir.y > 0) || (platform.position.y <= minConstraintY && dir.y < 0))
			dir.y = 0;
	}

	public void canActivate(bool enabled) {
		activable = enabled;
	}
}
