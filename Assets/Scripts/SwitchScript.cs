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
