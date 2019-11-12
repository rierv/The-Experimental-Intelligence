using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour, I_Activable
{
    #region Attributes
    public Vector3 upperLockerPosition;
    public Vector3 lowerLockerPosition;
    private bool isActive = false;
    #endregion

    private void FixedUpdate()
    {
        if (isActive && Vector3.Distance(transform.position, upperLockerPosition) > Time.fixedDeltaTime)
            transform.position = Vector3.MoveTowards(transform.position, upperLockerPosition, Time.fixedDeltaTime);
        else if(!isActive && Vector3.Distance(transform.position, lowerLockerPosition) > Time.fixedDeltaTime)
            transform.position = Vector3.MoveTowards(transform.position, lowerLockerPosition, Time.fixedDeltaTime);
    }

    public void Activate()
    {
        isActive = true;
    }
}
