﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerForComic : MonoBehaviour
{
    public ActivateComic comicToActivate;
    bool done = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<JellyBone>() && !done)
        {
            
            comicToActivate.Activate();
            done = true;
        }
    }
}
