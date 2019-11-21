﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour, I_Activable {
	#region Attributes
	public float upperLockerY;
	public float lowerLockerY;
	public float doorOpenTime;
	private bool isActive = false;
	private Vector3 translateVector;
	#endregion

	private void Start() {
		translateVector = Vector3.up;
	}

	private void Update() {
		if (isActive) {
			if ((upperLockerY - transform.position.y) > Time.deltaTime) {
				transform.position=Vector3.Lerp(transform.position, transform.position+translateVector, Time.deltaTime*doorOpenTime);
			}
		} else if ((transform.position.y - lowerLockerY) > Time.deltaTime) {
                transform.position = Vector3.Lerp(transform.position, transform.position - translateVector, Time.deltaTime * doorOpenTime);
        }


	}

	public void Activate() {
		isActive = true;
	}

	public void Deactivate() {
        isActive = false;
    }
}
