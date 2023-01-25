using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchControlledPlatformScript : MonoBehaviour
{
    #region Attributes
    private Vector3 currentDirection;
    public float maxConstraintX;
    public float minConstraintX;
    public float maxConstraintZ;
    public float minConstraintZ;
    public float speed;
    private float movementThreshold = 0.2f;
    private bool isPlatformActive = false;
    #endregion

    private void Start()
    {
        currentDirection = Vector3.zero;
    }
    public void Action(Vector3 direction)
    {


        if (direction.x > movementThreshold)
            currentDirection.x = 1;
        else if (direction.x < -movementThreshold)
            currentDirection.x = -1;
        else currentDirection.x = 0;

        if (direction.z > movementThreshold)
            currentDirection.z = 1;
        else if (direction.z < -movementThreshold)
            currentDirection.z = -1;
        else currentDirection.z = 0;

        if(currentDirection.x != 0 || currentDirection.z != 0)
            isPlatformActive = true;
    }

    private void FixedUpdate()
    {
        if (isPlatformActive)
        {
            FixDirection();
            this.transform.position += speed * currentDirection * Time.fixedDeltaTime;
        }

        isPlatformActive = false;
    }

    private void FixDirection()
    {
        if ((transform.position.x >= maxConstraintX && currentDirection.x > 0) || (transform.position.x <= minConstraintX && currentDirection.x < 0))
            currentDirection.x = 0;
        if ((transform.position.z >= maxConstraintZ && currentDirection.z > 0) || (transform.position.z <= minConstraintZ && currentDirection.z < 0))
            currentDirection.z = 0;
    }
}