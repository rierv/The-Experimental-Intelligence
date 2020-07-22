using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    JellyCore core;
    Quaternion baseRotation;
    StateManager state;
    public bool isHandle;
    public float SpineStrenght = 50000f;
    public List<Rigidbody> parentBodies;
    public float handle_hight=1f;
    Vector3 startPos;
    public VJHandler jsMovement;
    
       
    private void Awake()
    {
        parentBodies = new List<Rigidbody>();

    }
    void Start()
    {
        jsMovement = GameObject.Find("Joycon_container").GetComponent<VJHandler>();
        core = FindObjectOfType<JellyCore>();
        baseRotation = transform.rotation;
        startPos = transform.position;
        state = core.GetComponent<StateManager>();
    }

    void Update()
    {
        if (state.state == FlapperState.solid)
        {
                transform.position = core.transform.position;
                transform.rotation = core.transform.rotation;
            
        }
        else
        {
            
            
            if (isHandle)
            {
                
                int counter = 1;
                foreach (Rigidbody bone in parentBodies)
                {
                    bone.AddForce((jsMovement.InputDirection.x * Vector3.right + jsMovement.InputDirection.y * Vector3.forward + Vector3.down / 2) * (SpineStrenght / ((core.transform.position - startPos).magnitude + 1)) * Time.deltaTime/counter);


                }
                core.transform.position = new Vector3(this.transform.position.x, this.transform.position.y+handle_hight, this.transform.position.z);
                

            }
            else
            {
                
                transform.localPosition=Vector3.Lerp(transform.localPosition, Vector3.zero, Vector3.Distance(core.transform.position, transform.position)*SpineStrenght*Time.deltaTime);

                transform.rotation = Quaternion.Lerp(transform.rotation, baseRotation, Time.deltaTime);
            }
        }
        
    }
    
}
