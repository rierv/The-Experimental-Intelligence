﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneResizer : MonoBehaviour
{
    bool active = false;
    bool waitingForJellyState=false;
    GameObject Flapper;
    StateManager state;
    Vector3 oldScale;
    public Vector3 newScale;
    List<Transform> bones;
    GameObject Root;
    GameObject Core;
    public float scaleVelocity = 5;
    bool transformation=true;
    bool starting = false;
    // Start is called before the first frame update
    void Start()
    {
        Flapper = GameObject.Find("Flapper");
        Root = GameObject.Find("Root");
        Core = GameObject.Find("CORE");
        state = Core.GetComponent<StateManager>();
        oldScale = Flapper.transform.localScale;
        bones = new List<Transform>();

        for(int i=0; i<Root.transform.childCount; i++)
        {
            bones.Add(Root.transform.GetChild(i));
        }
    }
    private void Update()
    {
        Flapper.transform.localPosition = Core.transform.position;
        Core.transform.localPosition = new Vector3(0, 0, 0);

        if (active||waitingForJellyState) Flapper.transform.localScale = Vector3.Lerp(Flapper.transform.localScale, newScale, Time.deltaTime * scaleVelocity);
        else if ( Flapper.transform.localScale != oldScale) Flapper.transform.localScale = Vector3.Lerp(Flapper.transform.localScale, oldScale, Time.deltaTime * scaleVelocity);
        if (waitingForJellyState && state.state == FlapperState.jelly) waitingForJellyState = false;
        if (state.state == FlapperState.solid && transformation&&active)
        {
            transformation = false; 
            foreach(SphereCollider sc in Core.GetComponents<SphereCollider>())
            {
                sc.enabled = false;
            }
        }
        if (state.state == FlapperState.jelly && !transformation)
        {
            transformation = true;
            freeFlapperBones();
            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player"&&state.state==FlapperState.jelly)
        {
            Core.GetComponent<PlayerMove>().jumping = true;
            freezeFlapperBones();
            starting = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        
        if (other.tag == "Player"&&starting)
        {
            starting = false;
            Core.GetComponent<PlayerMove>().jumping = false;
            if (state.state == FlapperState.solid) waitingForJellyState = true;
            else
            {
                freeFlapperBones();
            }
        }
    }
    
    private void freeFlapperBones()
    {
        Core.GetComponent<BoxCollider>().enabled = false;
        Core.GetComponent<SphereCollider>().radius = 0.5f;
        GameObject.Find("Flapper model").GetComponent<SphereCollider>().radius = 0.5f;
        foreach (Transform bone in bones)
        {
            bone.GetComponent<SphereCollider>().radius = 0.5f;
            bone.GetComponent<JellyBone>().enabled = true;
            bone.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
        active = false;
    }
    private void freezeFlapperBones()
    {
        Core.GetComponent<SphereCollider>().radius /= 2;
        Core.GetComponent<BoxCollider>().enabled = true;
        GameObject.Find("Flapper model").GetComponent<SphereCollider>().radius /= 2;

        foreach (Transform bone in bones)
        {
            bone.GetComponent<SphereCollider>().radius /= 2;
            bone.GetComponent<JellyBone>().enabled = false;
            bone.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }

        active = true;
    }
}