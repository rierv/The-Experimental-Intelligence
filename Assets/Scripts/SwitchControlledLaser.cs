using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchControlledLaser : MonoBehaviour
{
    #region Attributes
    private Vector3 currentDirection;
    private Vector3 currentEulerAngles;
    public float maxConstraintX;
    public float minConstraintX;
    public float maxConstraintZ;
    public float minConstraintZ;
    public float movementSpeed;
    public float rotationSpeed;
    private float directionThreshold = 0.2f;
    public bool type; //true = Move, false = Rotate
    private bool isActive = false;
    #endregion

    private void Start()
    {
        currentDirection = Vector3.zero;
        currentEulerAngles = Vector3.zero;
    }

    public void Action(Vector3 direction)
    {
        if (type)
            Move(direction);
        else Rotate(direction);
    }

    private void Move(Vector3 direction)
    {
        if (direction.x > directionThreshold)
            currentDirection.x = 1;
        else if (direction.x < -directionThreshold)
            currentDirection.x = -1;
        else currentDirection.x = 0;

        if (direction.z > directionThreshold)
            currentDirection.z = 1;
        else if (direction.z < -directionThreshold)
            currentDirection.z = -1;
        else currentDirection.z = 0;

        if (currentDirection.x != 0 || currentDirection.z != 0)
            isActive = true;
    }
    private void Rotate(Vector3 direction)
    {
        isActive = true;

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
            else isActive = false;
        }
    }
    private void FixedUpdate()
    {
        if (isActive)
        {
            if (type)
            {
                FixDirection();
                transform.position += movementSpeed * currentDirection * Time.fixedDeltaTime;
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(currentEulerAngles), Time.fixedDeltaTime * rotationSpeed);
            }
        }

        isActive = false;
    }

    private void FixDirection()
    {
        if ((transform.position.x >= maxConstraintX && currentDirection.x > 0) || (transform.position.x <= minConstraintX && currentDirection.x < 0))
            currentDirection.x = 0;
        if ((transform.position.z >= maxConstraintZ && currentDirection.z > 0) || (transform.position.z <= minConstraintZ && currentDirection.z < 0))
            currentDirection.z = 0;
    }

}
