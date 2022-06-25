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

    List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    private void Awake()
    {
        spawnObjects();
        m_RaycastManager = GetComponent<ARRaycastManager>();

        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

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
        foreach(ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateObject(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateObject(trackedImage);
        }
    }

    private void AddObject(ARTrackedImage trackedImage)
    {   
        placedPrefabs[trackedImage.referenceImage.name].SetActive(true);
        placedPrefabs[trackedImage.referenceImage.name].transform.SetPositionAndRotation(trackedImage.transform.position+Vector3.up/2, Quaternion.identity);
        placedPrefabs[trackedImage.referenceImage.name].transform.eulerAngles = new Vector3(0, trackedImage.transform.eulerAngles.y, 0);


    }

    private void UpdateObject(ARTrackedImage trackedImage)
    {
        GameObject obj = placedPrefabs[trackedImage.referenceImage.name];
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            
            //obj.transform.parent = trackables.transform;
            Vector3 posOnScreen = Camera.main.WorldToScreenPoint(trackedImage.transform.position);
            if (posOnScreen.x > 0
                && posOnScreen.y > 0
                && posOnScreen.x < Camera.main.pixelWidth
                && posOnScreen.y < Camera.main.pixelHeight)
            {
                if (obj.activeInHierarchy == false) obj.SetActive(true);
                obj.transform.eulerAngles = new Vector3(0, trackedImage.transform.eulerAngles.y, 0);
                obj.transform.position = trackedImage.transform.position;
                posOnScreen = Camera.main.WorldToScreenPoint(trackedImage.transform.position);

                if (m_RaycastManager.Raycast(posOnScreen, s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    if (Vector3.Distance(trackedImage.transform.position, s_Hits[0].pose.position) < .02f && obj.transform.parent != s_Hits[0].trackable.transform)
                        obj.transform.parent = s_Hits[0].trackable.transform;
                    
                    else if (Vector3.Distance(trackedImage.transform.position, s_Hits[0].pose.position) >= .02f && obj.transform.parent != trackables.transform)
                        obj.transform.parent = trackables.transform;
                }
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

