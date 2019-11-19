using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalEnforcer : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coal")
        {
            other.GetComponent<TemperatureBlock>().enabled = false;
        }
    }
}
