using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchControlledPlatformScript : MonoBehaviour, I_Activable
{
    #region Attributes
    public Vector3 firstTarget;
    public Vector3 secondTarget;
    public float speed;

    #endregion
    public void Activate()
    {
    }

    public void Activate(bool twoFunctions)
    {
        if (twoFunctions)
        {
            transform.position = Vector3.MoveTowards(transform.position, firstTarget, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, secondTarget, speed * Time.deltaTime);
        }
    }

    public void Deactivate()
    {
    }


}
