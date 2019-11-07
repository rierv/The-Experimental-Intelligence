using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SwitchScript : MonoBehaviour
{
    #region Attributes
    public GameObject gameObject;
    public GameObject targetObject;
    private float minRotation = 315;
    private float maxRotation = 45;
    private int switchState = (int) SwitchState.NonActive;
    Vector3 currentRotation;
    Vector3 platformVelocity = new Vector3(0f, 0f, 1f);
    Vector3 validPos;
    public float yOffset = 1;
    public GameObject handle;
    #endregion
    // Start is called before the first frame update


    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(currentRotation.x > 25 && currentRotation.x < 50)
            {
                currentRotation.x = maxRotation;
            }
            else if(currentRotation.x > 310 && currentRotation.x < 335)
            {
                currentRotation.x = minRotation;
            }
            gameObject.transform.localRotation = Quaternion.Euler(currentRotation);
        }
    }
    
    // Update is called once per frame
    private void FixedUpdate()
    {
        
        currentRotation = gameObject.transform.rotation.eulerAngles;
        currentRotation.x = ClampAngle(currentRotation.x, -maxRotation, maxRotation);
        gameObject.transform.rotation = Quaternion.Euler(currentRotation);
        
        transform.LookAt(transform.position+new Vector3(0, -handle.transform.localPosition.z, handle.transform.localPosition.y));
        Debug.DrawRay(transform.position, handle.transform.localPosition * 10, Color.red, 20, true);


        if (handle.transform.localPosition.z > 1)
            handle.transform.localPosition = validPos;
        else if (handle.transform.localPosition.z < -1)
            handle.transform.localPosition = validPos;
        else
            validPos = handle.transform.localPosition;


        if (currentRotation.x > 25 && currentRotation.x < 50)
        {
            ActivateFirstFunction();
        }
        else if (currentRotation.x > 310 && currentRotation.x < 335)
        {
            ActivateSecondFunction();
        }
        else
        {
            DeactivateFunction();
        }
        switch (switchState)
        {
            case 1:
                targetObject.GetComponent<Rigidbody>().isKinematic = false;
                targetObject.GetComponent<Rigidbody>().velocity = platformVelocity;
                break;
            case 2:
                targetObject.GetComponent<Rigidbody>().isKinematic = false;
                targetObject.GetComponent<Rigidbody>().velocity = -platformVelocity;
                break;
            default:
                targetObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                targetObject.GetComponent<Rigidbody>().isKinematic = true;
                break;
        }
    }

    private void ActivateFirstFunction()
    {
        switchState = (int) SwitchState.ActiveFirst;
    }

    private void ActivateSecondFunction()
    {
        switchState = (int) SwitchState.ActiveSecond;
    }

    private void DeactivateFunction()
    {
        switchState = (int) SwitchState.NonActive;
    }

    public enum SwitchState
    {
        NonActive,
        ActiveFirst,
        ActiveSecond
    }

    public static float ClampAngle(float angle, float from, float to)
    {
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }
}
