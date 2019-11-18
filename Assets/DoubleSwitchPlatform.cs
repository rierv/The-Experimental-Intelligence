using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleSwitchPlatform : MonoBehaviour, I_Activable
{
    #region Attributes
    public bool xLock = false;
    public bool zLock = false;
    public bool yLock = false;

    public float maxConstraintX;
    public float minConstraintX;
    public float maxConstraintZ;
    public float minConstraintZ;
    public float maxConstraintY;
    public float minConstraintY;
    private Vector3 dir;
    private bool isActive = false;
    List<Transform> platforms;
    [Range(0,3)]
    public int numberOfPlatforms=2;
    public float[] velocities = new float[3];
    #endregion
    private void Start()
    {
        platforms = new List<Transform>();
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            platforms.Add(child);
        }
       
    }
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
        if (isActive)
        {
            for (int i= 0; i < numberOfPlatforms; i++)
            {
                Transform platform = platforms[i];
                if (!xLock && !zLock)
                    yLock = true;
                if (yLock)
                {
                    if (!xLock && !zLock)
                        dir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
                    else if (!xLock)
                        dir = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
                    else if (!zLock)
                        dir = new Vector3(0f, 0f, Input.GetAxis("Vertical"));
                }
                else
                {
                    if (!xLock)
                        dir = new Vector3(0f, Input.GetAxis("Horizontal"), 0f);
                    else if (!zLock)
                        dir = new Vector3(0f, Input.GetAxis("Vertical"), 0f);
                }
                FixDirection(platform);
                Debug.Log(platform.gameObject.name + velocities[i]);
                dir = dir * velocities[i];
                platform.Translate( dir* Time.fixedDeltaTime, Space.Self);
            }
        }
    }

    private void FixDirection(Transform platform)
    {
        if ((platform.position.x >= maxConstraintX && dir.x > 0) || (platform.position.x <= minConstraintX && dir.x < 0))
            dir.x = 0;
        if ((platform.position.z >= maxConstraintZ && dir.z > 0) || (platform.position.z <= minConstraintZ && dir.z < 0))
            dir.z = 0;
        if ((platform.position.y >= maxConstraintY && dir.y > 0) || (platform.position.y <= minConstraintY && dir.y < 0))
            dir.y = 0;
    }
}
