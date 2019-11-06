using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    #region Attributes
    public GameObject myButton;
    public GameObject triggeredObject;
    private Rigidbody rigidbody;
    private bool isButtonActivated = false;

    //private Vector3 iceCubeForce = new Vector3(10f, 0f, 0f);
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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || LayerMask.LayerToName(other.gameObject.layer) == "Movable")
        {
            DeactivateFunction();
        }
    }

    private void DeactivateFunction()
    {
        isButtonActivated = false;
    }

    public void ActivateFunction()
    {
        isButtonActivated = true;
    }

    private void FixedUpdate()
    {
        if(isButtonActivated)
        {
            //triggeredObject.GetComponent<Rigidbody>().AddForce(iceCubeForce);
            //Keep doing the action on the triggered GameObject
        }

    }

}
