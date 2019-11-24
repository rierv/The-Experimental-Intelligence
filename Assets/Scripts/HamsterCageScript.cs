using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterCageScript : MonoBehaviour
{
    #region Attributes
    public GameObject targetObject;
    Rigidbody wheel;
    Transform[] dynamo;
    GameObject electricity;
    public float wheelSpeed = 200f;
    private bool isActive = false;
    private float horizontalInput;
    #endregion

    private void Start()
    {
        wheel = gameObject.GetComponentInParent<Rigidbody>();
        dynamo = gameObject.transform.parent.parent.parent.Find("bobine_idea_3").GetComponentsInChildren<Transform>();
        electricity = gameObject.transform.parent.parent.parent.Find("battery").Find("battery electricity").gameObject;
        electricity.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isActive = false;
        electricity.SetActive(false);
    }

    private void Update()
    {
        if (isActive)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            if (horizontalInput != 0)
            {
                wheel.transform.Rotate(new Vector3(0f, wheelSpeed * Time.deltaTime * -horizontalInput, 0f));
                electricity.SetActive(true);
                for (int i = 1; i < dynamo.Length; i++)
                {
                    dynamo[i].Rotate(new Vector3(0f, wheelSpeed * Time.deltaTime * -horizontalInput, 0f));
                }
                if (horizontalInput > 0)
                {
                    //Activate "Right" function
                }
                else
                {
                    //Activate "Left" function
                }
            }
            else
            {
                electricity.SetActive(false);
            }
        }
    }
}
