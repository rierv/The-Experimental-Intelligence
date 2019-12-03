using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObjects : MonoBehaviour
{
    GameObject obj=null;
    ThrowableObject th;
    public float strenght=500f;
    StateManager state;
    bool ready = true;
    void Start()
    {
        state = GetComponent<StateManager>();
    }

    void Update()
    {
        //if (obj && state.state != FlapperState.solid) GetComponent<PlayerMove>().shrinking = true;
       // else GetComponent<PlayerMove>().shrinking = false;
        if (obj && state.state != FlapperState.solid && (Input.GetButtonDown("Jump")||state.state==FlapperState.gaseous))
        {
            StartCoroutine(Throw());

        }
    }
    IEnumerator Throw()
    {
        obj.GetComponent<ThrowableObject>().enabled = false;
        th.enabled = false;
        ready = false;


        if (th.isHandle)
        {

            transform.parent.SetParent(null);
            this.GetComponent<PlayerMove>().canMove = true;
            

            
        }
        else
        {
            obj.GetComponent<Rigidbody>().AddForce(Vector3.up * strenght*2 + (Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward) * strenght*4);
            obj.transform.parent = null;
        }

        obj = null;
        yield return new WaitForSeconds(0.1f);
        ready = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (ready && other.gameObject.layer == 14&&obj==null&&state.state==FlapperState.jelly)
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
            else
            {
                other.transform.parent.SetParent(transform);
            }
        }
    }
    
}
