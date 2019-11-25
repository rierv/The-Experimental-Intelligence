using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotScript : MonoBehaviour
{

    #region Attributes
    private Transform robot;
    private Transform flapper;
    public Transform wheel;
    public float maxLockerZ;
    public float minLockerZ;
    private Vector3 currPos;
    private Vector3 tmp;
    public float rotationScale = 12.5f;
    public float rotationScaleSolid = 2.5f;
    public float stunnTime = 5f;
    public float speed = 5f;
    public float speedSolid = 1f;
    bool stop = false;
    private float currentSpeed;
    private float currentRotationScale;
    private FlapperState state;
    private bool collidingWithSolid;
    #endregion

    private void Start()
    {
        collidingWithSolid = false;
        flapper = GameObject.Find("CORE").GetComponent<Transform>();
        state = GameObject.Find("Flapper model").GetComponent<JellyBone>().state;
        robot = gameObject.transform;
        currentRotationScale = rotationScale;
        currentSpeed = speed;
        currPos = robot.position;
        ManageRobotPosition();
    }

    private void Update()
    {
        if (collidingWithSolid)
        {
            state = GameObject.Find("Flapper model").GetComponent<JellyBone>().state;
            if (state != FlapperState.solid)
            {
                collidingWithSolid = false;
                currentSpeed = speed;
                currentRotationScale = rotationScale;
            }
        }

        if (!stop)
        {
            ManageRobotPosition();
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 12)
        {
            stop = true;
            StartCoroutine(RobotStop());
        }
        else if (collision.gameObject.layer == 9)
        {
            state = GameObject.Find("Flapper model").GetComponent<JellyBone>().state;
            if (state == FlapperState.solid)
            {
                collidingWithSolid = true;
                currentSpeed = speedSolid;
                currentRotationScale = rotationScaleSolid;
            }
        }
    }

    IEnumerator RobotStop()
    {
        yield return new WaitForSeconds(stunnTime);
        stop = false;
    }
   private void ManageRobotPosition()
    {
        tmp = robot.position;
        if (collidingWithSolid)
        {
            if (Mathf.Abs(tmp.z - minLockerZ) < Mathf.Abs(tmp.z - maxLockerZ))
                currPos.z = minLockerZ;
            else
                currPos.z = maxLockerZ;
        }
        else
        {
            currPos.z = Mathf.Clamp(flapper.position.z, minLockerZ, maxLockerZ);
        }
        robot.position = Vector3.Lerp(tmp, currPos, Time.deltaTime * currentSpeed);
        tmp = currPos - tmp;
        wheel.Rotate(0f, 0f, currentRotationScale * tmp.z, Space.Self);
    }
}