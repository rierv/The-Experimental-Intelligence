using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotScript : MonoBehaviour
{

    #region Attributes
    public Transform flapper;
    public Transform wheel;
    public float maxLockerZ;
    public float minLockerZ;
    private Transform robot;
    private Vector3 currPos;
    private float positionDifference;
    private float rotationScale = 100f;
    #endregion

    private void Start()
    {
        robot = gameObject.transform;
        ResetRobotPosition();
    }

    private void Update()
    {
        positionDifference = currPos.z;
        currPos.z = Mathf.Clamp(flapper.position.z, minLockerZ, maxLockerZ);
        positionDifference = currPos.z - positionDifference;
        robot.Translate(0f,0f,positionDifference, Space.World);
        wheel.Rotate(0f, 0f, rotationScale * positionDifference, Space.Self);
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

   private void ResetRobotPosition()
    {
        currPos = robot.position;
        currPos.z = Mathf.Clamp(flapper.position.z, minLockerZ, maxLockerZ);
        robot.position = currPos;
        positionDifference = 0f;
    }
}