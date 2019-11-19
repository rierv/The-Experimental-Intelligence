using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotScript : MonoBehaviour
{

    #region Attributes
    private Transform flapper;
    public Transform wheel;
    public float maxLockerZ;
    public float minLockerZ;
    private Transform robot;
    private Vector3 currPos;
    private float positionDifference;
    private float rotationScale = 100f;
    public float stunnTime = 5f;
    bool stop = false;
    #endregion

    private void Start()
    {
        flapper = GameObject.Find("CORE").GetComponent<Transform>();
        robot = gameObject.transform;
        ResetRobotPosition();
    }

    private void Update()
    {
        if (!stop)
        {
            positionDifference = currPos.z;
            currPos.z = Mathf.Clamp(flapper.position.z, minLockerZ, maxLockerZ);
            positionDifference = currPos.z - positionDifference;
            robot.Translate(0f, 0f, positionDifference, Space.World);
            wheel.Rotate(0f, 0f, rotationScale * positionDifference, Space.Self);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 12)
        {
            stop = true;
            StartCoroutine(RobotStop());
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        /*if(collision.gameObject.CompareTag("Player"){
         * if(flapperState == "Solid")
         * robot.getComponent<Rigidbody>().isKinematic = false;
         * }*/
    }

    private void OnCollisionExit(Collision collision)
    {
        /*
         * robot.getComponent<Rigidbody>().isKinematic = true;
         * ResetRobotPosition();
         */
    }
    IEnumerator RobotStop()
    {
        yield return new WaitForSeconds(stunnTime);
        stop = false;
    }
   private void ResetRobotPosition()
    {
        currPos = robot.position;
        currPos.z = Mathf.Clamp(flapper.position.z, minLockerZ, maxLockerZ);
        robot.position = currPos;
        positionDifference = 0f;
    }
}