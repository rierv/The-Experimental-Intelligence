using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayAction : MonoBehaviour, I_Activable
{
    public GameObject objectToDelay;
    
    bool active = false;
    public float duration;
    bool activable = true;

    public void Activate(bool type = true)
    {
        if(enabled) active = true;
    }

    public void Deactivate()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            StartCoroutine(delayAction());
            active = false;
        }

    }
    IEnumerator delayAction()
    {
        objectToDelay.GetComponent<I_Activable>().canActivate(false);
        objectToDelay.GetComponent<I_Activable>().Deactivate();
        yield return new WaitForSeconds(duration);
        objectToDelay.GetComponent<I_Activable>().canActivate(true);
        objectToDelay.GetComponent<I_Activable>().Activate();

    }

    public void canActivate(bool enabled)
    {
        activable = enabled;
    }
}
