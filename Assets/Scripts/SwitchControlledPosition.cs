using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchControlledPosition : MonoBehaviour, I_Activable
{
    #region Attributes
    public bool xLock=false;
    public bool zLock = false;
    public float maxConstraintX;
    public float minConstraintX;
    public float maxConstraintZ;
    public float minConstraintZ;
    public float speed;
    private Vector3 dir;
    private bool isActive = false;
    #endregion
    public void Activate()
    {
        isActive = true;
    }


    public void Deactivate()
    {
        isActive = false;
    }

    private void FixedUpdate()
    {
        if(isActive)
        {
            if(!xLock&&!zLock)
                dir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            else if (!xLock)
                dir = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
            else
                dir = new Vector3(0f , 0f, Input.GetAxis("Vertical"));
            FixDirection();
            transform.Translate(dir * speed * Time.fixedDeltaTime, Space.Self);
        }
    }

    private void FixDirection()
    {
        if ((transform.position.x >= maxConstraintX && dir.x > 0) || (transform.position.x <= minConstraintX && dir.x < 0))
            dir.x = 0;
        if ((transform.position.z >= maxConstraintZ && dir.z > 0) || (transform.position.z <= minConstraintZ && dir.z < 0))
            dir.z = 0;
    }
}
