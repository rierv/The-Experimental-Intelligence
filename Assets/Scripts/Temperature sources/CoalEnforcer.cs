﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalEnforcer : MonoBehaviour
{
    bool active = false;
    ParticleSystem light;
    private void Start()
    {
        light = GetComponentInChildren<ParticleSystem>();
        light.Pause();
    }
    private void Update()
    {
        if (active) light.Play();
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
