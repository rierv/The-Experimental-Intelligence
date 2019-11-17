using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMoveAndRotate : MonoBehaviour
{
    #region Attributes
    public bool translateFlag;
    public bool rotateFlag;
    //public int translateAxis;
    //public int rotateAxis;
    public Vector3 maxTranslation;
    public Vector3 minTranslation;
    public Vector3 maxRotation;
    public Vector3 minRotation;
    public float translationSpeed;
    public float rotationSpeed;
    private Vector3 currPos;
    private Vector3 currEulerAngles;
    private bool translationDirection = false;
    private bool rotationDirection = false;
    #endregion

    private void Start()
    {
        currPos = transform.position;
        currEulerAngles = transform.eulerAngles;

        if (Vector3.Distance(currPos, maxTranslation) < Vector3.Distance(currPos, minTranslation))
            translationDirection = true;

        if (Vector3.Distance(currEulerAngles, maxRotation) < Vector3.Distance(currEulerAngles, minRotation))
            rotationDirection = true;

        
    }

    private void FixedUpdate()
    {
        if(translateFlag)
        {
            currPos = transform.position;

            if (translationDirection)
            {
                transform.position = Vector3.MoveTowards(currPos, maxTranslation, Time.fixedDeltaTime * translationSpeed);
                if (Vector3.Distance(transform.position, maxTranslation) < (Time.fixedDeltaTime * translationSpeed))
                    translationDirection = !translationDirection;
            } 
            else
            {
                transform.position = Vector3.MoveTowards(currPos, minTranslation, Time.fixedDeltaTime * translationSpeed);
                if (Vector3.Distance(transform.position, minTranslation) < (Time.fixedDeltaTime * translationSpeed))
                    translationDirection = !translationDirection;
            }

        }

        if(rotateFlag)
        {
            if (rotationDirection)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(maxRotation), Time.fixedDeltaTime * rotationSpeed);
                if (Vector3.Distance(transform.eulerAngles, maxRotation) < (Time.fixedDeltaTime * rotationSpeed))
                    rotationDirection = !rotationDirection;
            } 
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(minRotation), Time.fixedDeltaTime * rotationSpeed);
                if (Vector3.Distance(transform.eulerAngles, minRotation) < (Time.fixedDeltaTime * rotationSpeed))
                    rotationDirection = !rotationDirection;
            }
        }
    }
}