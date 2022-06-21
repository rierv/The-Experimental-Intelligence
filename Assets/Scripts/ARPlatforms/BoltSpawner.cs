using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltSpawner : MonoBehaviour
{
    public GameObject bolt;
    // Start is called before the first frame update
    void Start()
    {
        bolt = Instantiate(bolt, transform.position + Vector3.up/20, Quaternion.identity);
        bolt.transform.localScale = Vector3.one * 0.015f;
        bolt.transform.parent = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(bolt.transform.position.y - transform.position.y) > 2f)
        {
            Destroy(bolt);
            bolt = Instantiate(bolt, transform.position + Vector3.up / 20, Quaternion.identity);
            bolt.transform.localScale = Vector3.one * 0.015f;
            bolt.transform.parent = transform;
        }
    }
    
}
