using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    #region Attributes
    public GameObject door;
    private Vector3 doorVelocity = new Vector3(0f, 2f, 0f);
    private float doorPositionY;
    #endregion
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            door.GetComponent<Rigidbody>().velocity = doorVelocity;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            door.GetComponent<Rigidbody>().velocity = -doorVelocity;
        }
    }

    private void FixedUpdate()
    {
        /*doorPositionY = door.GetComponent<Rigidbody>().position.y;
        if (doorPositionY == -2.5 || doorPositionY == 2.5)
        {
            door.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }*/
    }
}
