using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SwitchScript : MonoBehaviour
{
    #region Attributes
    public GameObject switchBase;
    public GameObject targetObject;
    private float minRotation = 270;
    private float maxRotation = 90;
    private int switchState = (int) SwitchState.NonActive;
    Vector3 currentRotation;
    Vector3 platformVelocity = new Vector3(0f, 0f, 1f);
    Vector3 validPos;
    public float yOffset = 1;
    public GameObject handle;
    #endregion
    // Start is called before the first frame update

    private void Start()
    {
        handle.GetComponent<ThrowableObject>().parentBodies.Add(handle.GetComponent<Rigidbody>());
    }

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
            switchBase.transform.localRotation = Quaternion.Euler(currentRotation);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        currentRotation = switchBase.transform.rotation.eulerAngles;

        if (handle.GetComponent<ThrowableObject>().isActiveAndEnabled)
        {
            transform.LookAt(transform.position + new Vector3(0, handle.transform.localPosition.z, handle.transform.localPosition.y) + (- Input.GetAxis("Vertical") * Vector3.up*5));
        }
        else transform.LookAt(transform.position + new Vector3(0, -handle.transform.localPosition.z, handle.transform.localPosition.y));

        handle.transform.localPosition = new Vector3(0, 1, 0);

        if (currentRotation.x > 70 || currentRotation.x < -70)
        {
            handle.transform.localPosition = validPos;
            switchBase.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        else
            validPos = handle.transform.localPosition;



        currentRotation.x = ClampAngle(currentRotation.x, -maxRotation, maxRotation);

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
        /*switch (switchState)
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
        }*/
    }
    private void FixedUpdate()
    {
        
        
    }

    private void ActivateFirstFunction()
    {
        switchState = (int) SwitchState.ActiveFirst;
        Debug.DrawRay(transform.position, handle.transform.localPosition * 10, Color.red, 20, true);
        Debug.DrawRay(transform.position, transform.forward * 10, Color.blue, 20, true);
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
