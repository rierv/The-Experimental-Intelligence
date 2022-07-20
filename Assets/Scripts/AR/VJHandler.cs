﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class VJHandler : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image jsContainer;
    private Image joystick;
    public Vector2 InputDirection;
    Quaternion startingRotation;
    public GameObject pointer;
    private void Awake()
    {
        jsContainer = GetComponent<Image>();
        joystick = transform.GetChild(0).GetComponent<Image>(); //this command is used because there is only one child in hierarchy
        InputDirection = Vector3.zero;
    }
    private void Update()
    {

        pointer.transform.rotation = Camera.main.transform.rotation;

        //pointer.transform.LookAt(new Vector3(pointer.transform.forward.x, pointer.transform.position.y, pointer.transform.forward.z));
        if (pointer.transform.up.y > .7f)
            pointer.transform.rotation = Quaternion.LookRotation(new Vector3(pointer.transform.forward.x, pointer.transform.position.y, pointer.transform.forward.z));
        else if (pointer.transform.up.y < .7f && Mathf.Abs(pointer.transform.right.y)<.7f)
            pointer.transform.rotation = Quaternion.LookRotation(new Vector3(pointer.transform.position.x, pointer.transform.forward.y, pointer.transform.position.z), pointer.transform.up);
        else
            pointer.transform.rotation = Quaternion.LookRotation(new Vector3(pointer.transform.up.x, pointer.transform.position.y, pointer.transform.up.z));


    }

    void OnGUI()
    {
        //Output the rotation rate, attitude and the enabled state of the gyroscope as a Label
        //GUI.Label(new Rect(700, 350, 500, 150), "pointer  transform right " + pointer.transform.right);
        //GUI.Label(new Rect(700, 400, 500, 150), "pointer  transform forward " + pointer.transform.forward);

    }

    internal void setRotation(Quaternion quaternion)
    {
        startingRotation = quaternion;
    }

    public void OnDrag(PointerEventData ped)
    {
        Vector2 position = Vector2.zero;

        //To get InputDirection
        RectTransformUtility.ScreenPointToLocalPointInRectangle
                (jsContainer.rectTransform,
                ped.position,
                ped.pressEventCamera,
                out position);

        position.x = ((position.x) / jsContainer.rectTransform.sizeDelta.x ) - .5f;
        position.y = ((position.y) / jsContainer.rectTransform.sizeDelta.y ) - .5f;

        //float x = (jsContainer.rectTransform.pivot.x == 1f) ? position.x * 2 + 1 : position.x * 2 - 1;
        //float y = (jsContainer.rectTransform.pivot.y == 1f) ? position.y * 2 + 1 : position.y * 2 - 1;

        Vector3 Direction;
        if (pointer.transform.up.y < .7f && Mathf.Abs(pointer.transform.right.y) < .7f)
            Direction = pointer.transform.up  * position.y;
        else
            Direction = pointer.transform.forward * position.y;

        Direction += pointer.transform.right * position.x ;

        Vector2 setDir = new Vector2(Direction.x, Direction.z);// new Vector2 (Direction.x, Direction.z);

        if (setDir.magnitude>.03f && setDir.magnitude<.3f)
            InputDirection = (setDir).normalized * setDir.magnitude*2.2f;
        else if (setDir.magnitude>=.3f)
            InputDirection = (setDir).normalized * .3f * 2.2f;
        
        //InputDirection = (InputDirection.magnitude > 1) ? InputDirection.normalized : InputDirection;

        //to define the area in which joystick can move around
        if (new Vector2(position.x * jsContainer.rectTransform.sizeDelta.x
                                                               , position.y * jsContainer.rectTransform.sizeDelta.y).magnitude > 80)
        {
            joystick.rectTransform.anchoredPosition = new Vector3(position.x * jsContainer.rectTransform.sizeDelta.x
                                                               , position.y * jsContainer.rectTransform.sizeDelta.y).normalized * 80;
        }
        else
        {
            joystick.rectTransform.anchoredPosition = new Vector3(position.x * jsContainer.rectTransform.sizeDelta.x
                                                               , position.y * jsContainer.rectTransform.sizeDelta.y);
        }
    }

    public void OnPointerDown(PointerEventData ped)
    {

        OnDrag(ped);
    }

    public void OnPointerUp(PointerEventData ped)
    {

        InputDirection = Vector3.zero;
        joystick.rectTransform.anchoredPosition = Vector3.zero;
    }
}