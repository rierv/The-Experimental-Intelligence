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
    private Dictionary<string, GameObject> placeblePrefabs = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> placedPrefabs = new Dictionary<string, GameObject>();
    public GameObject Flapper;
    private ARTrackedImageManager trackedImageManager;

    private void Awake()
    {
        foreach(GameObject o in Prefabs)
        {
            placeblePrefabs.Add(o.name, o);
        }

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

        GameObject newPrefab = Instantiate(placeblePrefabs[trackedImage.referenceImage.name], trackedImage.transform.position, Quaternion.identity);
        newPrefab.name = trackedImage.referenceImage.name;
        placedPrefabs.Add(newPrefab.name, newPrefab);
        if (trackedImage.referenceImage.name == "Start")
        {
            Instantiate(Flapper, trackedImage.transform.position+Vector3.up/2, Quaternion.identity).transform.parent=newPrefab.transform;
        }
    }

    private void UpdateObject(ARTrackedImage trackedImage)
    {
        placedPrefabs[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
    }

}
