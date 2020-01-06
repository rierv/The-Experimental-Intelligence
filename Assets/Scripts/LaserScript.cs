using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



[RequireComponent(typeof(LineRenderer))]

public class LaserScript : MonoBehaviour
{
    LineRenderer laser;
    public float laserWidth = 0.2f;
    public float maxLength;
    public Color startColor = new Color(1, 0f, 0f, 0.8f);
    public Color endColor = new Color(1, 0f, 0f, 0.5f);
    private int laserLength;
    private Vector3[] laserPositions;
    private Vector3 laserOrigin;
    private Vector3 positionOffset;
    //private MeshCollider meshCollider;


    void Awake()
    {
        laser = GetComponent<LineRenderer>();
        laser.startWidth = laserWidth;
        laser.endWidth = laserWidth;
        laser.material = new Material(Shader.Find("Sprites/Default"));
        laser.startColor = startColor;
        laser.endColor = endColor;
        positionOffset = Vector3.zero;
        laserOrigin = transform.position;
        //meshCollider = gameObject.AddComponent<MeshCollider>();
    }

    void Update()
    {
        laserOrigin = transform.position;
        RenderLaser();
    }

    private void RenderLaser()
    {
        CalculateLength();

        laserPositions[0] = transform.position;

        for (int i = 1; i < laserLength; i++)
        {
            positionOffset.x = laserOrigin.x + i * transform.forward.x;
            positionOffset.z = laserOrigin.z + i * transform.forward.z;
            positionOffset.y = laserOrigin.y + i * transform.forward.y;
            laserPositions[i] = positionOffset;
        }
        laser.SetPositions(laserPositions);
        /*Mesh mesh = new Mesh();
        laser.BakeMesh(mesh, true);
        meshCollider.sharedMesh = mesh;*/
    }

    private void CalculateLength()
    {
        RaycastHit[] hit;
        hit = Physics.RaycastAll(laserOrigin, transform.forward, maxLength);
        hit = SortRaycastAll(hit);
        
        for (int i = 0; i < hit.Length; i++)
        {
            if (!hit[i].collider.isTrigger || hit[i].collider.gameObject.layer == 10)
            {
                if (hit[i].collider.gameObject.layer == 9)
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                else if (hit[i].collider.gameObject.layer == 10)
                {
                    LaserActivatedButtonScript buttonScript = hit[i].collider.gameObject.GetComponent<LaserActivatedButtonScript>();
                    buttonScript.ActivateButton();
                }
                laserLength = (int)Mathf.Round(hit[i].distance) + 2;
                laserPositions = new Vector3[laserLength];
                laser.positionCount = laserLength;
                return;
            }
        }
        laserLength = (int)maxLength;
        laserPositions = new Vector3[laserLength];
        laser.positionCount = laserLength;


    }

    private RaycastHit[] SortRaycastAll(RaycastHit[] myArray)
    {
        RaycastHit[] raycastHits = (RaycastHit[]) myArray.Clone();
        RaycastHit tmp;

        for(int k = 0; k < raycastHits.Length; k++)
            for(int j = 1; j < raycastHits.Length; j++)
                if (raycastHits[j-1].distance > raycastHits[j].distance)
                {
                    tmp = raycastHits[j-1];
                    raycastHits[j-1] = raycastHits[j];
                    raycastHits[j] = tmp;
                }
        return raycastHits;
    }
}
