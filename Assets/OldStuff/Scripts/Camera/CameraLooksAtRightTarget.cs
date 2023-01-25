using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLooksAtRightTarget : MonoBehaviour
{
    public GameObject camera;
    public GameObject cameraPointer;
    public GameObject handle;
    public GameObject target;
    ThrowableObject th;
    private void Start()
    {
        th = handle.GetComponent<ThrowableObject>();
    }
    private void Update()
    {
        if (th.isActiveAndEnabled)
        {
            if (camera.GetComponent<CameraController>().isActiveAndEnabled)
            {
                camera.GetComponent<CameraController>().enabled = false;
                camera.transform.position = new Vector3(camera.transform.position.x +10f, camera.transform.position.y, camera.transform.position.z -3f);
            }
                cameraPointer.transform.LookAt(target.transform);
            camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, cameraPointer.transform.rotation, 10 * Time.deltaTime);
        }
        else if (!camera.GetComponent<CameraController>().isActiveAndEnabled) camera.GetComponent<CameraController>().enabled = true;
    }
}
