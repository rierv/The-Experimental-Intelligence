using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchControlledPosition : MonoBehaviour, I_Activable
{
    #region Attributes
    public bool xLock=false;
    public bool zLock = false;
    public bool yLock = false;

    public float maxConstraintX;
    public float minConstraintX;
    public float maxConstraintZ;
    public float minConstraintZ;
    public float maxConstraintY;
    public float minConstraintY;
    public float speed;
    private Vector3 dir;
    private bool isActive = false;
    bool activable = true;

    #endregion
    public VJHandler jsMovement;

    void Start()
    {
        jsMovement = GameObject.Find("Joycon_container").GetComponent<VJHandler>();

    }
    public void Activate(bool type = true)
    {
        if(activable) isActive = true;
    }


    public void Deactivate()
    {
        isActive = false;
    }

    private void FixedUpdate()
    {
        if(isActive)
        {
            if (!xLock && !zLock)
                yLock = true;
            if (yLock)
            {
                if (!xLock && !zLock)
                    dir = new Vector3(jsMovement.InputDirection.x, 0f, jsMovement.InputDirection.y);
                else if (!xLock)
                    dir = new Vector3(jsMovement.InputDirection.x, 0f, 0f);
                else if (!zLock)
                    dir = new Vector3(0f, 0f, jsMovement.InputDirection.y);
            }
            else
            {
                if (xLock && zLock)
                    dir = new Vector3(0f, jsMovement.InputDirection.y, 0);
                else if (!xLock)
                {
                    
                    dir = new Vector3(jsMovement.InputDirection.x, jsMovement.InputDirection.y, 0f);
                }
                else if (!zLock)
                    dir = new Vector3(0f, jsMovement.InputDirection.x, jsMovement.InputDirection.y);
            }
            FixDirection();
            transform.position = Vector3.Lerp(transform.position, transform.position + dir, speed * Time.fixedDeltaTime);
            Debug.Log(dir);

            //transform.Translate(dir * speed * Time.fixedDeltaTime, Space.Self);
        }
    }

    private void FixDirection()
    {
        if ((transform.position.x >= maxConstraintX && dir.x > 0) || (transform.position.x <= minConstraintX && dir.x < 0))
            dir.x = 0;
        if ((transform.position.z >= maxConstraintZ && dir.z > 0) || (transform.position.z <= minConstraintZ && dir.z < 0))
            dir.z = 0;
        if ((transform.position.y >= maxConstraintY && dir.y > 0) || (transform.position.y <= minConstraintY && dir.y < 0))
            dir.y = 0;
    }

    public void canActivate(bool enabled)
    {
        activable = enabled;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 13)
        {
            collision.gameObject.transform.parent = this.transform;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 13) collision.gameObject.transform.parent = null;
    }
}
