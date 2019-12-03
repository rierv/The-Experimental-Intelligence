using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        gameObject.GetComponent<Canvas>().worldCamera = FindObjectOfType<Camera>();
        gameObject.GetComponent<Canvas>().planeDistance = 5f;
    }
}
