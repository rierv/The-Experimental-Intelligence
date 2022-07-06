using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MarkerlessLevelPositioning : MonoBehaviour
{
    
    [SerializeField]
    private GameObject [] LevelPrefabs;
    private GameObject Level;
    [SerializeField]
    private GameObject Cursor;

    ARRaycastManager m_RaycastManager;

    List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    Vector2 posOnScreen = Vector2.zero;
    ARAnchorManager m_AnchorManager;
    ARPlaneManager m_PlaneManager;
    private void Awake()
    {
        m_AnchorManager = GetComponent<ARAnchorManager>();
        m_PlaneManager = GetComponent<ARPlaneManager>();
        Level = Instantiate(LevelPrefabs[LevelSelectionManager.currentLevel]);
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
            if(Input.touchCount > 0 && Input.GetTouch(0).phase==TouchPhase.Began)
            {
                Level.transform.SetPositionAndRotation(s_Hits[0].pose.position + Vector3.up*.01f, s_Hits[0].pose.rotation);
                Level.transform.parent = m_AnchorManager.AttachAnchor(m_PlaneManager.GetPlane(s_Hits[0].trackableId), s_Hits[0].pose).transform;
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
        /*FindObjectOfType<NextLevel>().Stars =0;
        FindObjectOfType<StarCollector>().Reset();
        Destroy(Level);
        Level = Instantiate(LevelPrefabs[LevelSelectionManager.currentLevel]);
        Level.SetActive(false);*/
        FindObjectOfType<NextLevel>().Stars = 0;
        FindObjectOfType<StarCollector>().Reset();
        Transform tmp = Level.transform.parent;
        Vector3 tmpPos = Level.transform.localPosition;
        Quaternion tmpRot = Level.transform.rotation;
        Destroy(Level);
        Level = Instantiate(LevelPrefabs[LevelSelectionManager.currentLevel]);
        Level.transform.parent = tmp;
        Level.transform.localPosition = tmpPos;
        Level.transform.rotation = tmpRot;
    }

    bool restarting = false;
    public IEnumerator ResetAfter(float time)
    {
        if (!restarting)
        {
            restarting = true;
            yield return new WaitForSeconds(time);
            Reset();
            restarting = false;
        }
    }

    public void NextLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelScore");
    }
}

