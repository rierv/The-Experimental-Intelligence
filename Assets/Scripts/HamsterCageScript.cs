using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterCageScript : MonoBehaviour
{
    #region Attributes
    public GameObject targetObject;
    Rigidbody wheel;
    Transform[] dynamo;
    GameObject[] electricity;
    public float wheelSpeed = 200f;
    private bool isActive = false;
    private float horizontalInput;
    public GameObject bobine;
    #endregion

    private void Start()
    {
        wheel = gameObject.GetComponentInParent<Rigidbody>();
        dynamo = bobine.GetComponentsInChildren<Transform>();
        electricity = new GameObject[3];
        electricity[0] = gameObject.transform.parent.parent.parent.Find("Battery").Find("BatteryElectricity").gameObject;
        electricity[1] = gameObject.transform.parent.parent.parent.Find("Cable").Find("CableElectricity1").gameObject;
        electricity[2] = gameObject.transform.parent.parent.parent.Find("Cable").Find("CableElectricity2").gameObject;
        foreach (GameObject e in electricity)
            e.SetActive(false);
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
        foreach (GameObject e in electricity)
            e.SetActive(false);
    }

    private void Update()
    {
        if (isActive)
        {
            horizontalInput = Input.GetAxis("Horizontal");

            if (horizontalInput != 0)
            {
                wheel.transform.Rotate(new Vector3(0f, wheelSpeed * Time.deltaTime * -horizontalInput, 0f));
                foreach (GameObject e in electricity)
                    e.SetActive(true);

                for (int i = 1; i < dynamo.Length; i++)
                {
                    dynamo[i].Rotate(new Vector3(0f, wheelSpeed * Time.deltaTime * -horizontalInput, 0f));
                }

                if (horizontalInput > 0)
                {
                    targetObject.GetComponent<I_Activable>().Deactivate();
                    targetObject.GetComponent<I_Activable>().Activate(true);
                }
                else
                {
                    Debug.Log(horizontalInput);
                    targetObject.GetComponent<I_Activable>().Deactivate();
                    targetObject.GetComponent<I_Activable>().Activate(false);
                }
            }
            else
            {
                foreach (GameObject e in electricity)
                    e.SetActive(false);
                targetObject.GetComponent<I_Activable>().Deactivate();
            }
        }
    }
}
