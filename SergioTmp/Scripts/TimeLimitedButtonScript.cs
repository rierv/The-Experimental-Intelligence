using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLimitedButtonScript : MonoBehaviour
{
    #region Attributes
    public GameObject myButton;
    public GameObject triggeredObject;
    private Rigidbody rigidbody;
    private bool isButtonActivated = false;
    private float buttonTime = 1.0f;
    private float currentTimeLeft;

    //private Vector3 iceCubeForce = new Vector3(5f, 0f, 0f);
    private Vector3 buttonForce = new Vector3(0f, -10f, 0f);
    #endregion


    private void Start()
    {
        rigidbody = myButton.GetComponent<Rigidbody>();
        
    }

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

    private void FixedUpdate()
    {
        if(isButtonActivated)
        {
            if(currentTimeLeft <= 0f)
            {
                DeactivateFunction();
            }
            else
            {
                rigidbody.AddForce(buttonForce);
                currentTimeLeft -= Time.fixedDeltaTime;
                //triggeredObject.GetComponent<Rigidbody>().AddForce(iceCubeForce);
                //Keep doing the action on the triggered GameObject
            }

        }

    }

}
