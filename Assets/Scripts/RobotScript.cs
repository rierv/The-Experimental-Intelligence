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
    private Vector3 tmp;
    public float rotationScale = 50f;
    public float stunnTime = 5f;
    bool stop = false;
    public float speed = 10f;
    private FlapperState state;
    #endregion

    private void Start()
    {
        flapper = GameObject.Find("CORE").GetComponent<Transform>();
        state = GameObject.Find("Flapper model").GetComponent<JellyBone>().state;
        robot = gameObject.transform;
        ResetRobotPosition();
    }

    private void Update()
    {
        if (!stop)
        {
            tmp = robot.position;
            currPos.z = Mathf.Clamp(flapper.position.z, minLockerZ, maxLockerZ);
            robot.position = Vector3.Lerp(tmp, currPos, Time.deltaTime * speed);
            tmp = currPos - tmp;
            //robot.Translate(0f, 0f, positionDifference, Space.World);
            wheel.Rotate(0f, 0f, -rotationScale * tmp.z, Space.Self);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 12)
        {
            stop = true;
            StartCoroutine(RobotStop());
        }
        /*else if (collision.gameObject.CompareTag("Player"))
        {
            state = GameObject.Find("Flapper model").GetComponent<JellyBone>().state;
            if (state == FlapperState.solid)
            {
                robot.GetComponent<Rigidbody>().isKinematic = false;
            }
        }*/
    }

    private void OnCollisionExit(Collision collision)
    {
          //robot.GetComponent<Rigidbody>().isKinematic = true;
          //ResetRobotPosition();
    }
    IEnumerator RobotStop()
    {
        yield return new WaitForSeconds(stunnTime);
        stop = false;
    }
   private void ResetRobotPosition()
    {
        tmp = robot.position;
        currPos = robot.position;
        currPos.z = Mathf.Clamp(flapper.position.z, minLockerZ, maxLockerZ);
        robot.position = Vector3.Lerp(tmp, currPos, Time.deltaTime * speed);
        tmp = currPos - tmp;
        wheel.Rotate(0f, 0f, -rotationScale * tmp.z, Space.Self);
    }
}