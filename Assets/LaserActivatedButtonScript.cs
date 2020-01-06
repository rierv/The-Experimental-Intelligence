using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserActivatedButtonScript : MonoBehaviour
{
    public GameObject targetObject;
    private bool active;

    private void Start()
    {
        active = false;
    }

    private void Update()
    {
        if (active)
        {
            targetObject.GetComponent<I_Activable>().Activate();
            active = false;
        }
        else
            targetObject.GetComponent<I_Activable>().Deactivate();

    }

    public void ActivateButton()
    {
        active = true;
    }
}
