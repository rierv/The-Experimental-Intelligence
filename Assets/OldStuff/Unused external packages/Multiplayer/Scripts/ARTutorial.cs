using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARTutorial : MonoBehaviour
{

    Text myText;
    public float animationSpeed = .5f;
    bool activating = false, deactivating = false;
    Quaternion startingRot;
    Quaternion finalRotx;
    float timer;
    Color startingColor, finalColor;
    Transform Flapper;
    Transform canvas;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        canvas = transform.GetChild(0);
        myText = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        startingRot = myText.rectTransform.rotation;
        finalRotx = Quaternion.LookRotation(transform.forward);
        startingColor = myText.color;
        finalColor = new Color(myText.color.r, myText.color.g, myText.color.b, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (activating || deactivating)
        {
            startingRot = Quaternion.LookRotation(Camera.main.transform.up, -Camera.main.transform.forward);
            finalRotx = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
            canvas.position = new Vector3(Flapper.position.x, canvas.position.y, Flapper.position.z);
            if (activating)
            {
                myText.rectTransform.rotation = Quaternion.Lerp(startingRot, finalRotx, timer);
                myText.color = Color.Lerp(startingColor, finalColor, timer);
                if (timer < 1)
                {
                    timer += Time.deltaTime * animationSpeed;
                }
                
            }
            if (deactivating)
            {
                myText.rectTransform.rotation = Quaternion.Lerp(startingRot, finalRotx, timer);
                myText.color = Color.Lerp(startingColor, finalColor, timer);
                if (timer > 0)
                {
                    timer -= Time.deltaTime * animationSpeed;
                }
                else
                    deactivating = false;
            }
        }
        
    }
    float stayTimer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            stayTimer = 0;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 9 && !activating && !deactivating )
        {
            stayTimer += Time.deltaTime;
            if (stayTimer > 1f)
            {
                Flapper = other.transform;
                activating = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            stayTimer = 0;
            activating = false;
            deactivating = true;
        }
    }

   
}
