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


    void Start()
    {
        laser = GetComponent<LineRenderer>();
        laser.startWidth = laserWidth;
        laser.endWidth = laserWidth;
        laser.material = new Material(Shader.Find("Sprites/Default"));
        laser.startColor = startColor;
        laser.endColor = endColor;
        positionOffset = Vector3.zero;
        laserOrigin = transform.position;
    }

    void FixedUpdate()
    {
        laserOrigin = transform.position;
        RenderLaser();
    }

    private void RenderLaser()
    {
        CalculateLength();

        laserPositions[0] = transform.position;

        for (int i = 0; i < laserLength; i++)
        {
            positionOffset.x = laserOrigin.x + i * transform.forward.x;
            positionOffset.z = laserOrigin.z + i * transform.forward.z;
            positionOffset.y = laserOrigin.y + i * transform.forward.y;
            laserPositions[i] = positionOffset;
        }
        laser.SetPositions(laserPositions);
    }

    private void CalculateLength()
    {
        RaycastHit[] hit;
        hit = Physics.RaycastAll(laserOrigin, transform.forward, maxLength);
        
        for(int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider)
            {
                if (hit[i].collider.CompareTag("Player"))
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                laserLength = (int)Mathf.Round(hit[i].distance) + 2;
                laserPositions = new Vector3[laserLength];
                laser.positionCount = laserLength;
                return;
            }
            i++;
        }
        laserLength = (int)maxLength;
        laserPositions = new Vector3[laserLength];
        laser.positionCount = laserLength;


    }
}
