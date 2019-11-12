using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObjects : MonoBehaviour
{
    GameObject obj=null;
    BoxCollider objBoxCollider;
    ThrowableObject th;
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
        //if (Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward != Vector3.zero)
        //obj.GetComponent<Rigidbody>().AddForce((Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward+Vector3.up/2.5f)*3000f);
        //else
        //obj.GetComponent<Rigidbody>().AddForce(Vector3.up * 2500);
        th.enabled = false;
        if (th.isHandle)
        {
            transform.parent.SetParent(null);
            this.GetComponent<PlayerMove>().canMove = true;
            this.GetComponent<PlayerMove>().shrinking = false;
            this.GetComponent<Rigidbody>().AddForce(Vector3.up * 700+ (Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward) * 200);

        }
        obj = null;
        //objBoxCollider.enabled = true;

        objBoxCollider = null;
        obj = null;

        yield return new WaitForSeconds(0.2f);
        
        

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 14&&obj==null)
        {
            Debug.Log("cisono");
            th = other.transform.parent.gameObject.GetComponent<ThrowableObject>();
            th.enabled = true;
            if (th.isHandle)
            {
                transform.parent.SetParent(other.transform.parent.gameObject.transform);
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                this.GetComponent<PlayerMove>().canMove = false;
                obj = other.transform.parent.gameObject;
                this.GetComponent<PlayerMove>().shrinking = true;

            }
            //objBoxCollider = obj.GetComponent<BoxCollider>();
            //objBoxCollider.enabled = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 14 && obj != null&&false)
        {
            th = other.transform.parent.gameObject.GetComponent<ThrowableObject>();
            th.enabled = false;
            if (th.isHandle)
            {
                transform.parent.SetParent(null);
                this.GetComponent<PlayerMove>().canMove = true;
                this.GetComponent<PlayerMove>().shrinking = false;
                //this.GetComponent<Rigidbody>().AddForce(Vector3.up * 5000000);

            }
            obj = null;
            //objBoxCollider.enabled = true;

            objBoxCollider = null;
        }
    }
}
