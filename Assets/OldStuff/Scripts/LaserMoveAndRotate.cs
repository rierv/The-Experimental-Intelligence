using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMoveAndRotate : MonoBehaviour
{
    #region Attributes
    private Transform laserObject;
    public bool translateFlag;
    public bool rotateFlag;
    public bool initialTranslationDirection;
    public bool initialRotationDirection;
    //public int translateAxis;
    //public int rotateAxis;
    public Vector3 maxTranslation;
    public Vector3 minTranslation;
    public Vector3 maxRotation;
    public Vector3 minRotation;
    public float translationSpeed;
    public float rotationSpeed;
    private Vector3 currPos;
    private bool translationDirection = false;
    private bool rotationDirection = false;
    #endregion

    private void Start()
    {
        laserObject = transform.parent;
        currPos = laserObject.position;
        translationDirection = initialTranslationDirection;
        rotationDirection = initialRotationDirection;
    }

    private void FixedUpdate()
    {
        if(translateFlag)
        {
            currPos = laserObject.position;

            if (translationDirection)
            {
                laserObject.position = Vector3.MoveTowards(currPos, maxTranslation, Time.fixedDeltaTime * translationSpeed);
                if (Vector3.Distance(laserObject.position, maxTranslation) < (Time.fixedDeltaTime * translationSpeed))
                    translationDirection = !translationDirection;
            } 
            else
            {
                laserObject.position = Vector3.MoveTowards(currPos, minTranslation, Time.fixedDeltaTime * translationSpeed);
                if (Vector3.Distance(laserObject.position, minTranslation) < (Time.fixedDeltaTime * translationSpeed))
                    translationDirection = !translationDirection;
            }

        }

        if(rotateFlag)
        {
            if (rotationDirection)
            {
                laserObject.rotation = Quaternion.RotateTowards(laserObject.rotation, Quaternion.Euler(maxRotation), Time.fixedDeltaTime * rotationSpeed);
                if (Vector3.Distance(laserObject.eulerAngles, maxRotation) < (Time.fixedDeltaTime * rotationSpeed))
                    rotationDirection = !rotationDirection;
            } 
            else
            {
                laserObject.rotation = Quaternion.RotateTowards(laserObject.rotation, Quaternion.Euler(minRotation), Time.fixedDeltaTime * rotationSpeed);
                if (Vector3.Distance(laserObject.eulerAngles, minRotation) < (Time.fixedDeltaTime * rotationSpeed))
                    rotationDirection = !rotationDirection;
            }
        }
    }
}