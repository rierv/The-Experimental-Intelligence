using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerChanger : MonoBehaviour, I_Activable
{
    public GameObject objectToChange;
    StateManager sm;
    public int layer;
    int tmpLayer;

    public void Activate(bool type = true)
    {
        if(sm.state==FlapperState.jelly) objectToChange.layer = layer;
    }

    public void canActivate(bool enabled)
    {
        throw new System.NotImplementedException();
    }

    public void Deactivate()
    {
        objectToChange.layer = tmpLayer;
    }

    // Start is called before the first frame update
    void Start()
    {
        sm = GameObject.FindObjectOfType<StateManager>();
        tmpLayer = objectToChange.layer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
