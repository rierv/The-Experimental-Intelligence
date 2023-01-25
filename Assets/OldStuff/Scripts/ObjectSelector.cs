using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour, I_Activable
{
    public List<GameObject> activableObjects;
    int activeObject;
    public void Activate(bool type = true)
    {
        activableObjects[activeObject].GetComponent<I_Activable>().Deactivate();
        if (activeObject < activableObjects.Count-1)
        {
            activeObject++;
        }
        else activeObject = 0;
        activableObjects[activeObject].GetComponent<I_Activable>().Activate();

    }

    public void canActivate(bool enabled)
    {
        throw new System.NotImplementedException();
    }

    public void Deactivate()
    {
        throw new System.NotImplementedException();
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
