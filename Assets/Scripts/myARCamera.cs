using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myARCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("CAMERA POSITION "+transform.position + " " + transform.localPosition + " CAMERA ROTATION " + transform.rotation + " " + transform.localRotation);
    }
    private void OnGUI()
    {
        GUI.Label(new Rect(500, 650, 200, 40), "CAMERA POSITION " + transform.position + " " + transform.localPosition + " CAMERA ROTATION " + transform.rotation + " " + transform.localRotation);
    }
}
