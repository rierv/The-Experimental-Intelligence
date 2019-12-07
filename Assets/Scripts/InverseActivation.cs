using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseActivation : MonoBehaviour, I_Activable
{
    bool active = false;
    public GameObject triggeredObject;
    public void Activate(bool type = true)
    {
        active = true;
        Debug.Log("attivoo");

    }

    public void canActivate(bool enabled)
    {
        throw new System.NotImplementedException();
    }

    public void Deactivate()
    {
        triggeredObject.GetComponent<I_Activable>().Activate();
        Debug.Log("attivoo");
        active = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
