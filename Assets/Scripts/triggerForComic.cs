﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerForComic : MonoBehaviour, I_Activable
{
    public ActivateComic comicToActivate;
    bool done = false;
    public bool active = true;
    public bool justOneTime = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (active && ((other.GetComponent<StateManager>() && other.GetComponent<StateManager>().state == FlapperState.jelly) || other.gameObject.layer == 14) && !done)
        {
            if (transform.parent.gameObject.GetComponent<CameraRetargeting>()) transform.parent.gameObject.GetComponent<CameraRetargeting>().Activate();
            comicToActivate.Activate();
            done = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (active && ((other.GetComponent<StateManager>() && other.GetComponent<StateManager>().state == FlapperState.jelly) || other.gameObject.layer == 14) && done)
        {
            if (transform.parent.gameObject.GetComponent<CameraRetargeting>()) transform.parent.gameObject.GetComponent<CameraRetargeting>().Deactivate();
            comicToActivate.Deactivate();
            if(!justOneTime) done = false;
        }
    }

    public void canActivate(bool enabled)
    {
        throw new System.NotImplementedException();
    }

    public void Activate(bool type = true)
    {
        active = true;
    }

    public void Deactivate()
    {
        active = false;
    }
}
