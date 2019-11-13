using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour, I_Activable
{
    #region Attributes
    public float upperLockerY;
    public float lowerLockerY;
    public float doorOpenTime;
    private bool isActive = false;
    private bool takeDoorUp = false;
    private float doorTimer;
    private Vector3 translateVector;
    #endregion

    private void Start()
    {
        translateVector = new Vector3(0f, Time.fixedDeltaTime, 0f);
    }

    private void Update()
    {
        if(takeDoorUp)
        {
            if ((upperLockerY - transform.position.y) > Time.deltaTime)
            {
                transform.Translate(translateVector);
            }
            else
            {
                takeDoorUp = false;
                doorTimer = doorOpenTime;
            }
        }
        else
        {
            if (isActive)
            {
                takeDoorUp = true;
            }
            else if (doorTimer > 0f)
            {
                doorTimer -= Time.deltaTime;
            }
            else if ((transform.position.y - lowerLockerY) > Time.deltaTime)
            {
                transform.Translate(-translateVector);
            }
        }

        isActive = false;

    }

    public void Activate()
    {
        isActive = true;
    }
}
