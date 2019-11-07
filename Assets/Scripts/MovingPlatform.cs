using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    #region Attributes
    public Rigidbody platform;
    private bool direction; //true is forward, false is backward
    private bool isPlatformMoving;
    private bool slowDown; //it's true when you have to slow down
    private float maxVelocity = 4f;
    private float minVelocity = 1f;
    private float timeScale = 0.1f;
    private Vector3 platformVelocity;
    private float platformTimer;
    private float platformStopTime = 1.5f;
    #endregion

    private void Start()
    {
        direction = false;
        platformTimer = platformStopTime;
        platform.velocity = Vector3.zero;
        isPlatformMoving = false;
        platform.isKinematic = true;
        slowDown = false;
    }

    private void FixedUpdate()
    {
        if (!isPlatformMoving) //The platform is not moving, so you have to wait for a short time and then start moving
        {
            if (platformTimer > 0f)
            {
                platformTimer -= Time.fixedDeltaTime;
            }
            else
            {
                isPlatformMoving = true;
                platform.isKinematic = false;
                if (direction)
                    platform.velocity = new Vector3(0f, 0f, Time.fixedDeltaTime);
                else
                    platform.velocity = new Vector3(0f, 0f, -Time.fixedDeltaTime);
            }
        }
        else //The platform is moving, you may have to slow down or speed up
        {
            platformVelocity = platform.velocity;
            if (!slowDown)
            {
                if (Mathf.Abs(platformVelocity.z) < maxVelocity)
                {
                    if (direction)
                        platformVelocity.z += Time.fixedDeltaTime * timeScale;
                    else
                        platformVelocity.z -= Time.fixedDeltaTime * timeScale;
                }
                else
                {
                    if (direction)
                        platformVelocity.z = maxVelocity;
                    else
                        platformVelocity.z = -maxVelocity;
                }
                    
            }
            else
            {
                if (Mathf.Abs(platformVelocity.z) > minVelocity)
                {
                    if (direction)
                        platformVelocity.z -= Time.fixedDeltaTime * timeScale;
                    else
                        platformVelocity.z += Time.fixedDeltaTime * timeScale;
                }
                else
                {
                    if (direction)
                        platformVelocity.z = minVelocity;
                    else
                        platformVelocity.z = -minVelocity;
                }
            }

            platform.velocity = platformVelocity;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlatformSlowdown"))
        {
            slowDown = true;
        }
        else if (other.CompareTag("PlatformTarget"))
        {
            ChangeDirection();
        }

    }

    private void ChangeDirection()
    {
        direction = !direction;
        isPlatformMoving = false;
        platform.isKinematic = true;
        platformTimer = platformStopTime;
        slowDown = false;
    }
}

