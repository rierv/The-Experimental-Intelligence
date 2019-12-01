using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchHelper4TwoTargets : MonoBehaviour, I_Activable

{
    public GameObject firstTarget;
    public GameObject secondTarget;
    public void Activate(bool type)
    {
        if (type)
        {
            firstTarget.GetComponent<I_Activable>().Activate();
            secondTarget.GetComponent<I_Activable>().Deactivate();
        }
        else
        {
            secondTarget.GetComponent<I_Activable>().Activate();
            firstTarget.GetComponent<I_Activable>().Deactivate();
        }
    }

    public void Deactivate()
    {
        firstTarget.GetComponent<I_Activable>().Deactivate();
        secondTarget.GetComponent<I_Activable>().Deactivate();
    }

}
