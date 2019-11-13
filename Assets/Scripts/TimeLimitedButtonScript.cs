using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLimitedButtonScript : MonoBehaviour
{
    #region Attributes
    public GameObject triggeredObject;
    private bool isButtonActivated = false;
    public float buttonTime;
    private float currentTimeLeft;
    private Vector3 buttonForce = new Vector3(0f, -1.5f, 0f);
    public float maxY;
    public float minY;
    private Vector3 currentPosition;
    private float clampedY;
    #endregion


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || LayerMask.LayerToName(other.gameObject.layer) == "Movable")
        {
            ActivateFunction();
        }
    }

    private void DeactivateFunction()
    {
        isButtonActivated = false;
    }

    public void ActivateFunction()
    {
        isButtonActivated = true;
        currentTimeLeft = buttonTime;
    }

    private void Update()
    {
        currentPosition = this.transform.position;
        clampedY = Mathf.Clamp(currentPosition.y, minY, maxY);
        currentPosition.y = clampedY;
        this.transform.position = currentPosition;
    }
    private void FixedUpdate()
    {

        if (isButtonActivated)
        {
            if(currentTimeLeft <= 0f)
            {
                DeactivateFunction();
            }
            else
            {
                this.gameObject.GetComponent<Rigidbody>().AddForce(buttonForce);
                currentTimeLeft -= Time.fixedDeltaTime;
                triggeredObject.GetComponent<I_Activable>().Activate();
            }

        }

    }

}
