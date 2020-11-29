using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TapToPlaceObject : MonoBehaviour
{
    public GameObject objectToPlace;
    public GameObject placementIndicator;
    private ARSessionOrigin arOrigin;
    private ARSession arSession;
    private ARPlaneManager arPlaneManager;
    private ARRaycastManager arRaycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid =false;
    public Camera myCamera;
    public GameObject GameOrigin;
    // Start is called before the first frame update
    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        arPlaneManager = FindObjectOfType<ARPlaneManager>();
        arSession = FindObjectOfType<ARSession>();
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }
    }

    private void PlaceObject()
    {
        FindObjectOfType<VJHandler>().setRotation(Quaternion.Inverse(placementPose.rotation));
        FindObjectOfType<LevelManager>().StartGame(placementPose);
        //Instantiate(objectToPlace, placementPose.position -Vector3.right/3f - Vector3.up/3f - Vector3.forward / 3f, Quaternion.identity);
        //GameOrigin.transform.SetPositionAndRotation(placementPose.position - myCamera.transform.right / 2 - myCamera.transform.up / 2, placementPose.rotation);
        placementIndicator.SetActive(false);
        arPlaneManager.enabled = false;
        arRaycastManager.enabled = false;
        this.gameObject.SetActive(false);
        
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid) {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else placementIndicator.SetActive(false);

    }

    private void UpdatePlacementPose()
    {

        Ray screenCenter = myCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (arRaycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinBounds)) ;
        
        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            Debug.Log(hits[0].hitType);
            placementPose = hits[0].pose;
            Vector3 cameraForward = myCamera.transform.forward;
            Vector3 cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
        
    }
}
