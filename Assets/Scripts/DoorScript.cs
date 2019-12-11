﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour, I_Activable {
	#region Attributes
	public float upperLockerY;
	public float lowerLockerY;
	public float doorOpenTime;
	private bool isActive = false;
    public bool goingUp = true;
	private Vector3 translateVector;
    bool activable = true;
    #endregion

    private void Start() {
		translateVector = Vector3.up;
	}

	private void Update() {
        
        if (isActive)
        {
            if (goingUp && (upperLockerY - transform.position.y) > 0)
            {
                transform.position = Vector3.Lerp(transform.position, transform.position + translateVector, Time.deltaTime * doorOpenTime);
            }
            else if (!goingUp && (transform.position.y - upperLockerY) > 0)
            {
                transform.position = Vector3.Lerp(transform.position, transform.position - translateVector, Time.deltaTime * doorOpenTime);
            }
        }
        else if (goingUp && (transform.position.y - lowerLockerY) > 0)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position - translateVector, Time.deltaTime * doorOpenTime);
        }
        else if (!goingUp && (lowerLockerY - transform.position.y) > 0)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + translateVector, Time.deltaTime * doorOpenTime);
        }
        

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<StateManager>() && collision.gameObject.GetComponent<StateManager>().state == FlapperState.solid) isActive = false;
    }
    
    public void Activate(bool type = true) {
		if(activable) isActive = true;
	}

	public void Deactivate() {
        isActive = false;
    }

    public void canActivate(bool enabled)
    {
        activable = enabled;
    }
}
