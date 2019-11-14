using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SwitchScript : MonoBehaviour
{
    #region Attributes
    public GameObject switchBase;
    public GameObject targetObject;
    private float maxRotation = 90f;
    private float firstMinRotation; //Min rotation to activate first function
    private float firstMaxRotation; //Max rotation to activate first function; note this is necessary since angles are between 0 and 360
    private float secondMinRotation;
    private float secondMaxRotation;
    Vector3 currentRotation;
    Vector3 validPos;
    public float yOffset = 1;
    public GameObject handle;
    public float maxInclination = 2f;
    public bool comingBackToVerticalPos = true;
    public bool vertical = true;
    public bool horizontal = true;
    bool bonesActive = true;
    public GameObject pointer;
    #endregion
    // Start is called before the first frame update

    private void Start()
    {
        handle.GetComponent<ThrowableObject>().parentBodies.Add(handle.GetComponent<Rigidbody>());
        firstMaxRotation = maxRotation;
        firstMinRotation = maxRotation / 2;
        secondMinRotation = 360 - firstMaxRotation;
        secondMaxRotation = 360 - firstMinRotation;
    }

    // Update is called once per frame
    private void Update()
    {
        currentRotation = switchBase.transform.rotation.eulerAngles;

        if (handle.GetComponent<ThrowableObject>().isActiveAndEnabled)
        {
            if (bonesActive)
            {
                SphereCollider[] bones = GameObject.Find("Root").GetComponentsInChildren<SphereCollider>();
                foreach (SphereCollider bone in bones)
                {
                    bone.enabled = false;
                    bone.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                }
                bonesActive = false;
            }
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            //WITHOUT POINTER
            /*
            if (vertical && horizontal)
            {
                if (Mathf.Abs(Input.GetAxis("Vertical")) - Mathf.Abs(Input.GetAxis("Horizontal"))  > 0)
                {
                    if (Input.GetAxis("Vertical") > 0) transform.LookAt(transform.position + Vector3.forward + (Input.GetAxis("Horizontal") * Vector3.right - Input.GetAxis("Vertical") * Vector3.up * maxInclination) );
                    else transform.LookAt(transform.position + Vector3.forward + (Input.GetAxis("Horizontal") * -Vector3.right - Input.GetAxis("Vertical") * Vector3.up * maxInclination) );
                }
                else if (Mathf.Abs(Input.GetAxis("Vertical")) - Mathf.Abs(Input.GetAxis("Horizontal")) < 0)
                {
                    if (Input.GetAxis("Horizontal") > 0) transform.LookAt(transform.position + Vector3.right + (-Input.GetAxis("Horizontal") * Vector3.up * maxInclination + Input.GetAxis("Vertical") * Vector3.forward));
                    else transform.LookAt(transform.position + Vector3.right + (Input.GetAxis("Horizontal") * -Vector3.up * maxInclination - Input.GetAxis("Vertical") * Vector3.forward) );
                }
            }
            else if (horizontal) transform.LookAt(transform.position + Vector3.right - Input.GetAxis("Horizontal") * Vector3.up * maxInclination);
            else if (vertical) transform.LookAt(transform.position + Vector3.forward - Input.GetAxis("Vertical") * Vector3.up * maxInclination);

        }*/
            if (y != 0 || x != 0)
                if (vertical && horizontal)
                    if (Mathf.Abs(y) - Mathf.Abs(x) > 0)
                        if (y > 0)
                            if (x > 0)
                                pointer.transform.LookAt(transform.position + Vector3.forward + (x * Vector3.right - (x + y) * Vector3.up * maxInclination));
                            else
                                pointer.transform.LookAt(transform.position + Vector3.forward + (x * Vector3.right - (-x + y) * Vector3.up * maxInclination));
                        else
                            if (x > 0)
                                pointer.transform.LookAt(transform.position + Vector3.forward + (x * -Vector3.right - (-x + y) * Vector3.up * maxInclination));

                            else
                                pointer.transform.LookAt(transform.position + Vector3.forward + (x * -Vector3.right - (x + y) * Vector3.up * maxInclination));
                    else
                        if (x > 0)
                            if (y > 0)
                                pointer.transform.LookAt(transform.position + Vector3.right + (-(x+ y) * Vector3.up * maxInclination + y * Vector3.forward));
                            else
                                pointer.transform.LookAt(transform.position + Vector3.right + (-(x - y) * Vector3.up * maxInclination + y * Vector3.forward));
                        else
                            if (y > 0)
                                pointer.transform.LookAt(transform.position + Vector3.right + ((x - y) * -Vector3.up * maxInclination - y * Vector3.forward));
                            else
                                pointer.transform.LookAt(transform.position + Vector3.right + ((x + y) * -Vector3.up * maxInclination - y* Vector3.forward));
                else if (horizontal) pointer.transform.LookAt(transform.position + Vector3.right - x * Vector3.up * maxInclination);
                else if (vertical) pointer.transform.LookAt(transform.position + Vector3.forward - y * Vector3.up * maxInclination);
            else pointer.transform.rotation = Quaternion.identity;

            transform.rotation = Quaternion.Lerp(transform.rotation, pointer.transform.rotation, 10f * Time.deltaTime);

        }
        else 
        {
            if (!bonesActive)
            {
                SphereCollider[] bones = GameObject.Find("Root").GetComponentsInChildren<SphereCollider>();
                foreach (SphereCollider bone in bones)
                {
                    bone.enabled = true;
                    bone.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                }
                bonesActive = true;
            }
            if (comingBackToVerticalPos) transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, 10 * Time.deltaTime);
        }
        


        if (currentRotation.x > firstMinRotation && currentRotation.x < firstMaxRotation)
        {
            ActivateFirstFunction();
        }
        else if (currentRotation.x > secondMinRotation && currentRotation.x < secondMaxRotation)
        {
            ActivateSecondFunction();
        }

    }

    private void ActivateFirstFunction()
    {
        targetObject.GetComponent<I_Activable>().Activate(true);
    }

    private void ActivateSecondFunction()
    {
        targetObject.GetComponent<I_Activable>().Activate(false);
    }

    public enum SwitchState
    {
        NonActive,
        ActiveFirst,
        ActiveSecond
    }
}
