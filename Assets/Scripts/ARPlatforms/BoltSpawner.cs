using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltSpawner : MonoBehaviour
{
    Vector3 StartPos;
    Transform parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
        StartPos = transform.localPosition;
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Finish")
        {
            Reset();
        }
    }
    public void Reset()
    {
        transform.parent = parent;
        transform.localPosition = StartPos;
    }
}
