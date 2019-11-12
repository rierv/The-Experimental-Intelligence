using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour, I_Activable
{
    private Vector3 force = new Vector3(10f, 0f, 0f);
    public void Activate()
    {
        this.gameObject.GetComponent<Rigidbody>().AddForce(force);
    }
}
