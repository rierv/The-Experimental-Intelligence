using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    #region Attributes
    public GameObject triggeredObject;
    private bool isButtonActivated = false;
    public float maxY;
    public float minY;
    private Vector3 currentPosition;
    private float clampedY;
    #endregion



    private void OnTriggerStay(Collider other)
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
            triggeredObject.GetComponent<I_Activable>().Activate();
        }
    }

}
