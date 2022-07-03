using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]

public class ImageTracking : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Prefabs;
    private Dictionary<string, GameObject> placedPrefabs = new Dictionary<string, GameObject>();
    public GameObject Flapper, trackables;
    private ARTrackedImageManager trackedImageManager;

    ARRaycastManager m_RaycastManager;
    ARAnchorManager m_AnchorManager;
    ARPlaneManager m_PlaneManager;
    float CameraWMin, CameraHMin;
    List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    private void Awake()
    {
        spawnObjects();
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_PlaneManager = GetComponent<ARPlaneManager>();
        m_AnchorManager = GetComponent<ARAnchorManager>();

        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        CameraWMin = Camera.main.pixelWidth / 4;
        CameraHMin = Camera.main.pixelHeight / 4;

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
        foreach(GameObject o in Prefabs)
        {
            GameObject newPrefab = Instantiate(o, Vector3.zero, Quaternion.identity);
            newPrefab.name = o.name;
            placedPrefabs.Add(o.name, newPrefab);
            newPrefab.transform.parent = trackables.transform;
            if (newPrefab.name == "Start")
            {
                Flapper = Instantiate(Flapper, newPrefab.transform.position + Vector3.up / 5, Quaternion.identity);
                Flapper.transform.parent = newPrefab.transform;
            }
            newPrefab.SetActive(false);

        }
    }
    private void ImageChanged (ARTrackedImagesChangedEventArgs eventArgs)
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
            if (posOnScreen.x > CameraWMin
                && posOnScreen.y > CameraHMin
                && posOnScreen.x < Camera.main.pixelWidth - CameraWMin
                && posOnScreen.y < Camera.main.pixelHeight - CameraHMin)
            {
                if (obj.activeInHierarchy == false) obj.SetActive(true);
                obj.transform.eulerAngles = new Vector3(0, trackedImage.transform.eulerAngles.y, 0);

                if (m_RaycastManager.Raycast(posOnScreen, s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    GameObject tmp = obj.transform.parent.gameObject;

                    if (!tmp.GetComponent<ARAnchor>())
                    {
                        obj.transform.parent = m_AnchorManager.AttachAnchor(m_PlaneManager.GetPlane(s_Hits[0].trackableId), s_Hits[0].pose).transform;
                    }
                    else if(Vector3.Distance(s_Hits[0].pose.position, trackedImage.transform.position) < Vector3.Distance(obj.transform.parent.position, trackedImage.transform.position) /5)
                    {
                        obj.transform.parent = m_AnchorManager.AttachAnchor(m_PlaneManager.GetPlane(s_Hits[0].trackableId), s_Hits[0].pose).transform;
                        Destroy(tmp);
                    }

                }
                
                obj.transform.position = trackedImage.transform.position;
            }
            
        }
    }
    public void Reset()
    {
        /*foreach(GameObject o in Prefabs)
        {
            Destroy(placedPrefabs[o.name]);
        }
        placedPrefabs = new Dictionary<string, GameObject>();
        spawnObjects();
        */
        //Flapper.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        foreach (Transform t in Flapper.GetComponentsInChildren<Transform>())
        {
            t.localPosition = Vector3.zero;
            
        }
        Flapper.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        Flapper.transform.position = placedPrefabs["Start"].transform.position + Vector3.up / 5;
        Flapper.transform.parent = placedPrefabs["Start"].transform;
    }
}

