using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour, I_Activable
{
    #region Attributes
    public float upperLockerY;
    public float lowerLockerY;
    private bool isActive = false;
    private Vector3 translateVector;
    #endregion

    private void Start()
    {
        translateVector = new Vector3(0f, Time.fixedDeltaTime, 0f);
    }

    private void FixedUpdate()
    {
        if (isActive && (upperLockerY - transform.position.y) > Time.fixedDeltaTime)
            transform.Translate(translateVector);
        else if (!isActive && (transform.position.y - lowerLockerY) > Time.fixedDeltaTime)
            transform.Translate(-translateVector);

        isActive = false;
    }

    public void Activate()
    {
        isActive = true;
    }
}
