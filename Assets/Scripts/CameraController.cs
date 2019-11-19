using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 1;
    public Transform pointer;
    public bool allowLookAtPlayer=false;
    [Header("To lock an axis, freeze set 0 to min and max")]
    public float minX = -100;
    public float maxX = 100;
    public float xOffset = 0;
    [Space]
    public float minY = -100;
    public float maxY = 100;
    public float yOffset = 5;

    [Space]
    public float minZ = 0;
    public float maxZ = 0;
    public float zOffset = -10;

    public Vector3 targetOffset=Vector3.zero;

    JellyCore jellyCore;

    void Awake()
    {

        jellyCore = FindObjectOfType<JellyCore>();
        transform.position=Vector3.Lerp(transform.position, new Vector3(Mathf.Clamp(jellyCore.transform.position.x, minX, maxX)+ xOffset, Mathf.Clamp(jellyCore.transform.position.y, minY, maxY) + yOffset, Mathf.Clamp(jellyCore.transform.position.z, minZ, maxZ) + zOffset), Time.deltaTime*cameraSpeed);
    }

    void Update()
    {
        Vector3 newPosition = new Vector3(Mathf.Clamp(jellyCore.transform.position.x, minX, maxX) + xOffset, Mathf.Clamp(jellyCore.transform.position.y, minY, maxY) + yOffset, Mathf.Clamp(jellyCore.transform.position.z, minZ, maxZ) + zOffset);
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * cameraSpeed);
        if (allowLookAtPlayer)
        {
            pointer.transform.LookAt(jellyCore.transform.position + targetOffset);
            transform.rotation = Quaternion.Lerp(transform.rotation, pointer.rotation, cameraSpeed * Time.deltaTime);
        }
    }
}
