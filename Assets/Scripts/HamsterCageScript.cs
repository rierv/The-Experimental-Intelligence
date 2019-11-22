using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterCageScript : MonoBehaviour
{
    #region Attributes
    Rigidbody wheel;
    public float speed;
    private bool isActive = false;
    #endregion

    private void Start()
    {
        wheel = gameObject.GetComponentInParent<Rigidbody>();
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
    }

    private void Update()
    {
        if (isActive)
        {
            wheel.transform.Rotate(new Vector3(0f, speed * Time.deltaTime * -Input.GetAxis("Horizontal"), 0f));
        }
    }
}
