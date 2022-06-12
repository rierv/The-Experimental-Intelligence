using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]

public class ImageTracking : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Prefabs;
    private Dictionary<string, GameObject> placedPrefabs = new Dictionary<string, GameObject>();
    public GameObject Flapper, trackables;
    private ARTrackedImageManager trackedImageManager;

    private void Awake()
    {
        spawnObjects();

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
                Flapper = Instantiate(Flapper, Vector3.up / 2, Quaternion.identity);
                Flapper.transform.parent = newPrefab.transform;
            }
            newPrefab.SetActive(false);

        }
    }
    private void ImageChanged (ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach(ARTrackedImage trackedImage in eventArgs.added)
        {
            AddObject(trackedImage);
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
        if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking )
        {
            if(obj.activeInHierarchy == false) obj.SetActive(true);
            obj.transform.SetPositionAndRotation(trackedImage.transform.position, Quaternion.identity);
            obj.transform.eulerAngles = new Vector3(0, trackedImage.transform.eulerAngles.y, 0);
        }
    }
    public void Reset()
    {
        foreach(GameObject o in Prefabs)
        {
            Destroy(placedPrefabs[o.name]);
        }
        placedPrefabs = new Dictionary<string, GameObject>();
        spawnObjects();
        Flapper.transform.parent = placedPrefabs["Start"].transform;
        Flapper.gameObject.SetActive(true);
        Flapper.transform.localPosition = Vector3.up / 2;
        Time.timeScale = 1f;
        GameManager.isGamePaused = false;
    }
}
