using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MarkerlessLevelPositioning : MonoBehaviour
{
    
    [SerializeField]
    private GameObject Level;
    [SerializeField]
    private GameObject Cursor;

    ARRaycastManager m_RaycastManager;

    List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    Vector3 StartPos;
    Vector2 posOnScreen = Vector2.zero;
    private void Awake()
    {
        Level = Instantiate(Level);
        Cursor = Instantiate(Cursor);
        Level.SetActive(false);
        Cursor.SetActive(false);
        m_RaycastManager = GetComponent<ARRaycastManager>();
        posOnScreen.x = Camera.main.pixelWidth/2;
        posOnScreen.y = Camera.main.pixelHeight/2;
    }  

    private void Update()
    {
        if (!Level.activeInHierarchy && m_RaycastManager.Raycast(posOnScreen, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            if(Input.touchCount > 0)
            {
                StartPos = s_Hits[0].pose.position;
                Level.transform.SetPositionAndRotation(s_Hits[0].pose.position, s_Hits[0].pose.rotation);
                Level.SetActive(true);
                Cursor.SetActive(false);
            }
            else
            {
                if(!Cursor.activeInHierarchy) Cursor.SetActive(true);
                Cursor.transform.SetPositionAndRotation(s_Hits[0].pose.position, s_Hits[0].pose.rotation);
            }
        }
        /*if (Level.activeInHierarchy && m_RaycastManager.Raycast(Camera.main.WorldToScreenPoint(Level.transform.position), s_Hits, TrackableType.PlaneWithinPolygon))
        {
            if(Vector3.Distance(StartPos, s_Hits[0].pose.position) < .1f)
            {
                Level.transform.position = s_Hits[0].pose.position;
            }
        }*/
    }
    public void Reset()
    {
        
    }
}

