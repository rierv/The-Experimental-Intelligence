using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using static SpawnManager;

[RequireComponent(typeof(ARTrackedImageManager))]

public class ImageTrackingMultiplayer : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Prefabs;
    public Dictionary<string, GameObject> placedPrefabs = new Dictionary<string, GameObject>();
    public GameObject trackables;
    private ARTrackedImageManager trackedImageManager;

    ARRaycastManager m_RaycastManager;
    ARAnchorManager m_AnchorManager;
    ARPlaneManager m_PlaneManager;
    float CameraWMin, CameraHMin;
    List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    ARPlacementAndPlaneDetectionController toStartMatchmaking;

    Transform startTransform = null;
    private void Awake()
    {

        spawnObjects();
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_PlaneManager = GetComponent<ARPlaneManager>();
        m_AnchorManager = GetComponent<ARAnchorManager>();

        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        toStartMatchmaking = GetComponent<ARPlacementAndPlaneDetectionController>();
        CameraWMin = Camera.main.pixelWidth / 5;
        CameraHMin = Camera.main.pixelHeight / 5;

    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }
    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    private void spawnObjects()
    {
        foreach (GameObject o in Prefabs)
        {
            GameObject newPrefab = Instantiate(o, Vector3.zero, Quaternion.identity);
            newPrefab.name = o.name;
            placedPrefabs.Add(o.name, newPrefab);
            if (newPrefab.name == "Start") newPrefab.transform.parent = trackables.transform;
            else newPrefab.transform.parent = placedPrefabs["Start"].transform;
            newPrefab.SetActive(false);
        }
    }
    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateObject(trackedImage);
        }
    }


    private void UpdateObject(ARTrackedImage trackedImage)
    {
        GameObject obj = placedPrefabs[trackedImage.referenceImage.name];
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            //obj.transform.parent = trackables.transform;
            Vector3 posOnScreen = Camera.main.WorldToScreenPoint(trackedImage.transform.position);

            if (Vector3.Distance(Camera.main.velocity, Vector3.zero) < .01f && Vector3.Distance(Input.gyro.userAcceleration, Vector3.zero) < .01f &&
                Vector3.Distance(Input.gyro.rotationRate, Vector3.zero) < .01f
                && posOnScreen.x > CameraWMin
                && posOnScreen.y > CameraHMin
                && posOnScreen.x < Camera.main.pixelWidth - CameraWMin
                && posOnScreen.y < Camera.main.pixelHeight - CameraHMin)
            {
                if (obj.activeInHierarchy == false)
                {
                    if (obj.name == "Start")
                    {
                        startTransform = placedPrefabs["Start"].transform;
                        toStartMatchmaking.DisableARPlacementAndPlaneDetection();
                    }
                    if (startTransform != null) {
                        obj.SetActive(true);
                        PhotonView _photonView = obj.GetComponent<PhotonView>();
                        PhotonNetwork.AllocateViewID(_photonView);
                    }
                }
                obj.transform.eulerAngles = new Vector3(0, trackedImage.transform.eulerAngles.y, 0);

                GameObject CurrentParent = obj.transform.parent.gameObject;

                if (obj.name == "Start" && m_RaycastManager.Raycast(posOnScreen, s_Hits, TrackableType.PlaneWithinPolygon))
                {

                    if (!CurrentParent.GetComponent<ARAnchor>())
                    {
                        obj.transform.parent = m_AnchorManager.AttachAnchor(m_PlaneManager.GetPlane(s_Hits[0].trackableId), s_Hits[0].pose).transform;
                    }
                    else if (Vector3.Distance(s_Hits[0].pose.position, trackedImage.transform.position) < Vector3.Distance(CurrentParent.transform.position, trackedImage.transform.position) / 7)

                    {
                        obj.transform.parent = m_AnchorManager.AttachAnchor(m_PlaneManager.GetPlane(s_Hits[0].trackableId), s_Hits[0].pose).transform;
                        Destroy(CurrentParent);
                    }
                }
                if (obj.name != "Start" && obj.transform.position != trackedImage.transform.position)
                {
                    PhotonView _photonView = obj.GetComponent<PhotonView>();
                    object[] data = new object[]
                    {
                        obj.transform.localPosition, obj.transform.localRotation, _photonView.ViewID, obj.name
                    };


                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions
                    {
                        Receivers = ReceiverGroup.Others,
                        CachingOption = EventCaching.AddToRoomCache

                    };


                    SendOptions sendOptions = new SendOptions
                    {
                        Reliability = true
                    };

                    //Raise Events!
                    PhotonNetwork.RaiseEvent((byte)RaiseEventCodes.PlatformFoundEventCode, data, raiseEventOptions, sendOptions);
                        
                }
                

                obj.transform.position = trackedImage.transform.position;
                    
                
            }

        }
    }
    public void Reset(Transform transform)
    {
        transform.position = placedPrefabs["Start"].transform.position + Vector3.up * .03f;
    }
}

