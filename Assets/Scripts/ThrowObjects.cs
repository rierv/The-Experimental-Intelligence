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
            
            th = other.transform.parent.gameObject.GetComponent<ThrowableObject>();
            th.enabled = true;
            if (th.isHandle)
            {
                transform.parent.SetParent(other.transform.parent.gameObject.transform);
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                ConfigurableJoint jnt = other.transform.parent.gameObject.AddComponent<ConfigurableJoint>();
                jnt.connectedBody = this.gameObject.GetComponent<Rigidbody>();
                jnt.xMotion = ConfigurableJointMotion.Limited;
                jnt.yMotion = ConfigurableJointMotion.Limited;
                jnt.zMotion = ConfigurableJointMotion.Limited;
                jnt.targetPosition = transform.position;
                obj = other.transform.parent.gameObject;

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
            }
            obj = null;
            //objBoxCollider.enabled = true;

            objBoxCollider = null;
        }
    }
}
