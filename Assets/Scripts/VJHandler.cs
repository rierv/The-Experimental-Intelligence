using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class VJHandler : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image jsContainer;
    private Image joystick;
    public Vector2 InputDirection;
    public GameObject camera;
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
        
        /*if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            InputDirection = Camera.main.transform.right * Input.GetAxis("Horizontal") + Camera.main.transform.forward * Input.GetAxis("Vertical");
            
            Debug.Log("ZIMBAWUE");
        }*/
    }

    void OnGUI()
    {
        //Output the rotation rate, attitude and the enabled state of the gyroscope as a Label
        GUI.Label(new Rect(700, 350, 500, 150), "pointer  transform right " + pointer.transform.right);
        GUI.Label(new Rect(700, 400, 500, 150), "pointer  transform forward " + pointer.transform.forward);

        GUI.Label(new Rect(700, 450, 500, 150), "input direction   " + InputDirection.x +"   "+ InputDirection.y);
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

        if(pointer.transform.up.y<.5f)
            Direction = pointer.transform.up * position.y;
        else
            Direction = pointer.transform.forward  * position.y;

        Direction += pointer.transform.right * position.x ;

        Vector2 setDir = new Vector2(Direction.x, Direction.z);// new Vector2 (Direction.x, Direction.z);

        if (setDir.magnitude>.04f && setDir.magnitude<.4f)
            InputDirection = (setDir).normalized * setDir.magnitude*1.7f;
        else if (setDir.magnitude>=.4f)
            InputDirection = (setDir).normalized * .4f * 1.7f;
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