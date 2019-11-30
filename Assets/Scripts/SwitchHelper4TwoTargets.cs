using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchHelper4TwoTargets : MonoBehaviour, I_Activable

{
    public GameObject firstTarget;
    public GameObject secondTarget;
    public void Activate(bool type)
    {
        if (type) firstTarget.GetComponent<I_Activable>().Activate();
        else secondTarget.GetComponent<I_Activable>().Activate();
    }

    public void Deactivate()
    {
        firstTarget.GetComponent<I_Activable>().Deactivate();
        secondTarget.GetComponent<I_Activable>().Deactivate();
    }

}
