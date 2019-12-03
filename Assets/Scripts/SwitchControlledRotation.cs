using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchControlledRotation : MonoBehaviour, I_Activable
{
    #region Attributes
    private Vector3 forward;
    private Vector3 up;
    private Quaternion myRot;
    public float speed;
    private bool isActive = false;
    bool activable = true;

    #endregion

    public void Activate(bool type = true)
    {
        if(activable)isActive = true;
    }
    
    public void Deactivate()
    {
        isActive = false;
    }

    private void FixedUpdate()
    {
        if (isActive)
        {
            forward = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            if (forward != Vector3.zero)
            {
                myRot = Quaternion.LookRotation(forward, transform.up);
                transform.rotation = myRot;
            }
        }
    }

    public void canActivate(bool enabled)
    {
        activable = enabled;
    }

    /*private void Rotate(Vector3 direction)
    {
        if (direction.x > directionThreshold)
        {
            if (direction.z > directionThreshold)
                currentEulerAngles.y = 45;
            else if (direction.z < -directionThreshold)
                currentEulerAngles.y = 135;
            else currentEulerAngles.y = 90;
        }
        else if (direction.x < -directionThreshold)
        {
            if (direction.z > directionThreshold)
                currentEulerAngles.y = 315;
            else if (direction.z < -directionThreshold)
                currentEulerAngles.y = 225;
            else currentEulerAngles.y = 270;
        }
        else
        {
            if (direction.z > directionThreshold)
                currentEulerAngles.y = 0;
            else if (direction.z < -directionThreshold)
                currentEulerAngles.y = 180;
        }
    }*/
}
