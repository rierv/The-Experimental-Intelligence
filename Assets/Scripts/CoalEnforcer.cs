using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalEnforcer : MonoBehaviour
{
    bool active = false;
    Light light;
    private void Start()
    {
        light = GetComponentInChildren<Light>();
    }
    private void Update()
    {
        if (active) light.intensity = Mathf.Lerp(light.intensity, 25, Time.deltaTime/2);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coal"|| other.gameObject.tag == "Ice")
        {
            other.GetComponent<TemperatureBlock>().enabled = false;
            active = true;
        }
    }
}
