using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageRepulsion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)                          
    {
        if (collision.gameObject.name == "CORE") collision.gameObject.GetComponent<Rigidbody>().AddForce(collision.contacts[0].normal * 3000);
    }
}
