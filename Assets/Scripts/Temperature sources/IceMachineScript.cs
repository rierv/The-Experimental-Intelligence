﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMachineScript : MonoBehaviour, I_Activable {
	#region Attributes
	public MeshRenderer topIcon;
	public Material topIconMaterialOn;
	public Material topIconMaterialOff;
	public MeshRenderer door;
	public Material doorMaterialOn;
	public Material doorMaterialOff;
	[Space]
	public Transform cubeQueue;
	public Transform machineDoor;
	public GameObject block;
	private GameObject instance;
	public float throwBlockForce = 1000f;
	public float block_melting_speed = 0.1f;
	public float block_melting_duration = 15f;
	public Vector3 doorHightDifference = new Vector3(0f, 0.5f, 0f);
	[Range(1, 5)]
	public float doorVecolicity = 2;
	//private bool isMachineActive = false;
	[Range(2, 30)]
	public float newBlockTime = 2f;
	[Header("Activable")]
	public bool active = true;
	public bool invertTrueFalse;
	public bool newBlockOnActivate;
	public bool newBlockLoop;

	private float closeDoorTime;
	private float openDoorTime;
	private bool isDoorOpen;
	Vector3 throwForce;
	Vector3 DoorStartpos;
	public bool newBlockNeeded = true;
	bool activable = true;

	#endregion

	void Start() {
		if (!active) {
			topIcon.material = topIconMaterialOff;
			door.material = doorMaterialOff;
		}
		closeDoorTime = newBlockTime + 1 / doorVecolicity;
		openDoorTime = newBlockTime - 1 / doorVecolicity;
		DoorStartpos = machineDoor.position;
		throwForce = -transform.forward * throwBlockForce;
	}

	void OpenMachineDoor() {
		isDoorOpen = true;
	}

	void CloseMachineDoor() {
		isDoorOpen = false;
	}

	void CreateNewCube() {
		if (isDoorOpen) {

			instance = Instantiate(block, cubeQueue.position, cubeQueue.rotation, cubeQueue);
			instance.GetComponentInChildren<TemperatureBlock>().melting_speed = block_melting_speed;
			instance.GetComponentInChildren<TemperatureBlock>().melting_duration = block_melting_duration;
			instance.GetComponentInChildren<Rigidbody>().AddForce(throwForce);
			instance.GetComponentInChildren<BoxCollider>().isTrigger = true;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.GetComponent<TemperatureBlock>()) {
			other.transform.parent.GetComponent<BoxCollider>().isTrigger = false;
		}
	}

	private void Update() {
		if (isDoorOpen)
			machineDoor.position = Vector3.Lerp(machineDoor.position, DoorStartpos + doorHightDifference, Time.deltaTime * doorVecolicity);
		else
			machineDoor.position = Vector3.Lerp(machineDoor.position, DoorStartpos, Time.deltaTime * doorVecolicity);
	}

	public IEnumerator newBlockCoroutine() {
		if (active && !isDoorOpen) {
			if (instance != null) {
				if (instance.GetComponentInChildren<TemperatureBlock>()) instance.GetComponentInChildren<TemperatureBlock>().fadeOut();

			}
			newBlockNeeded = false;
			yield return new WaitForSeconds(1);
			OpenMachineDoor();
			yield return new WaitForSeconds(newBlockTime - openDoorTime);
			CreateNewCube();
			yield return new WaitForSeconds(closeDoorTime - newBlockTime);
			CloseMachineDoor();
			yield return new WaitForSeconds(openDoorTime);
			newBlockNeeded = true;
			if (newBlockLoop && active) {
				yield return newBlockCoroutine();
			}
		}
	}

	public void Activate(bool type) {
		if (activable && ((type && !invertTrueFalse) || (!type && invertTrueFalse))) {
			active = true;
			topIcon.material = topIconMaterialOn;
			door.material = doorMaterialOn;
			if (newBlockNeeded && newBlockOnActivate) {
				StartCoroutine(newBlockCoroutine());
			}
		} else {
			Deactivate();
		}
	}

	public void Deactivate() {
		//StopAllCoroutines();
		foreach (Transform cube in cubeQueue) Destroy(cube.gameObject);
		active = false;
		topIcon.material = topIconMaterialOff;
		door.material = doorMaterialOff;
	}

	public void canActivate(bool enabled) {
		activable = enabled;
	}
}
