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
    GameObject pointer;
    private void Awake()
    {
        pointer  = new GameObject();
        jsContainer = GetComponent<Image>();
        joystick = transform.GetChild(0).GetComponent<Image>(); //this command is used because there is only one child in hierarchy
        InputDirection = Vector3.zero;
    }
    private void Update()
    {

        pointer.transform.rotation = Camera.main.transform.rotation;
        OnGUI();
        /*if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            InputDirection = Camera.main.transform.right * Input.GetAxis("Horizontal") + Camera.main.transform.forward * Input.GetAxis("Vertical");
            
            Debug.Log("ZIMBAWUE");
        }*/
    }
    
    void OnGUI()
    {
        //Output the rotation rate, attitude and the enabled state of the gyroscope as a Label
        GUI.Label(new Rect(500, 350, 200, 60), "pointer  transform right " + pointer.transform.right);
        GUI.Label(new Rect(500, 400, 200, 60), "pointer  transform forward " + pointer.transform.forward);

        GUI.Label(new Rect(500, 450, 200, 60), "starting rotation   " + startingRotation);
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
        
        Vector3 Direction = pointer.transform.right * position.x  + pointer.transform.forward * position.y + pointer.transform.up*position.y;
        InputDirection = (new Vector2 (Direction.x, Direction.z)*10).normalized;
        
        //InputDirection = (InputDirection.magnitude > 1) ? InputDirection.normalized : InputDirection;

        //to define the area in which joystick can move around
        joystick.rectTransform.anchoredPosition = new Vector3(position.x * jsContainer.rectTransform.sizeDelta.x
                                                               , position.y* jsContainer.rectTransform.sizeDelta.y);

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