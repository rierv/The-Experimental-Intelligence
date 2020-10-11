using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseTouchScript : EventTrigger
{
    public bool pauseButtonHold = false;
    public bool pauseButtonRelease = false;
    
    public override void OnPointerUp(PointerEventData data)
    {
        pauseButtonHold = false;
        pauseButtonRelease = true;
    }
    public override void OnPointerDown(PointerEventData data)
    {
        pauseButtonHold = true;
    }
}
