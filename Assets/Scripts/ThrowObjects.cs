using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObjects : MonoBehaviour
{
    GameObject obj=null;
    ThrowableObject th;
    public float strenght=500f;
    void Start()
    {
        
    }

    void Update()
    {

        if (obj && Input.GetButtonDown("Jump"))
        {
            StartCoroutine(Throw());

        }
    }
    IEnumerator Throw()
    {

        yield return new WaitForSeconds(0.2f);

        obj.GetComponent<ThrowableObject>().enabled = false;
        
        th.enabled = false;
        if (th.isHandle)
        {

            transform.parent.SetParent(null);
            this.GetComponent<PlayerMove>().canMove = true;
            this.GetComponent<PlayerMove>().shrinking = false;
            //this.GetComponent<Rigidbody>().AddForce(Vector3.up * strenght*2 + (Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward) * strenght*2f);

        }
        else
        {
            th.GetComponent<Rigidbody>().AddForce(Vector3.up * strenght + (Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward) * strenght*2);

        }

        obj = null;        
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 14&&obj==null)
        {
            th = other.transform.parent.gameObject.GetComponent<ThrowableObject>();
            th.enabled = true;
            obj = other.transform.parent.gameObject;

            if (th.isHandle)
            {
                transform.parent.SetParent(other.transform.parent.gameObject.transform);
                other.transform.parent.gameObject.GetComponent<Rigidbody>().AddForce((Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward + Vector3.down / 2) * 100000 * Time.deltaTime);
                this.GetComponent<PlayerMove>().canMove = false;

            }
        }
    }
    
}
