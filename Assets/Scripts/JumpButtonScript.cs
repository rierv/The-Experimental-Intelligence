using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JumpButtonScript : EventTrigger
{
    public bool jumpButtonHold = false;
    public bool jumpButtonRelease = false;

    public override void OnPointerUp(PointerEventData data)
    {
        jumpButtonHold = false;
        jumpButtonRelease = true;
    }
    public override void OnPointerDown(PointerEventData data)
    {
        jumpButtonHold = true;
    }
}
