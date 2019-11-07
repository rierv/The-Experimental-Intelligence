using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObjects : MonoBehaviour
{
    GameObject obj=null;
    BoxCollider objBoxCollider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(obj && Input.GetButtonDown("Jump"))
        {
            
            StartCoroutine(Throw());

        }
    }
    IEnumerator Throw()
    {
        yield return new WaitForSeconds(0.2f);

        obj.GetComponent<ThrowableObject>().enabled = false;
        if (Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward != Vector3.zero)
            obj.GetComponent<Rigidbody>().AddForce((Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward+Vector3.up/2.5f) * 3000);
        else
            obj.GetComponent<Rigidbody>().AddForce(Vector3.up * 2500);
        obj = null;

        yield return new WaitForSeconds(0.2f);
        
        

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 12&&obj==null)
        {
            other.gameObject.GetComponent<ThrowableObject>().enabled = true;
            obj = other.gameObject;
            objBoxCollider = obj.GetComponent<BoxCollider>();
            objBoxCollider.enabled = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 12 && obj != null)
        {
            objBoxCollider.enabled = true;
            obj.GetComponent<ThrowableObject>().enabled = false;
            obj = null;
            objBoxCollider = null;
        }
    }
}
