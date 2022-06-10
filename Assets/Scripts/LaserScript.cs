using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LaserScript : MonoBehaviour {
    StateManager state;
    BoxCollider mybox;
    private void Start()
    {
        mybox = GetComponent<BoxCollider>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            mybox.enabled = false;
            state = collision.gameObject.GetComponent<StateManager>();
        }
    }
    private void Update()
    {
        if (mybox.enabled == false && state && state.state == FlapperState.jelly) mybox.enabled = true;
    }
}
