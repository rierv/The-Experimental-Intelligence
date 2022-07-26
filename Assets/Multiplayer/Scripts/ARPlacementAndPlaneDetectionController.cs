using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class ARPlacementAndPlaneDetectionController : MonoBehaviour
{
    
    public GameObject searchForGameButton;
    //public GameObject scaleSlider;

    public TextMeshProUGUI informUIPanel_Text;


    private void Awake()
    {
  

    }


    // Start is called before the first frame update
    void Start()
    {
        //scaleSlider.SetActive(true);

        searchForGameButton.SetActive(false);
        

        informUIPanel_Text.text = "Move phone to detect GO! platform!";

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableARPlacementAndPlaneDetection()
    {


        //scaleSlider.SetActive(false);

        searchForGameButton.SetActive(true);

        informUIPanel_Text.text = "Great! You found the GO! marker.. Now, search for other players!";


    }

    public void EnableARPlacementAndPlaneDetection()
    {
        
        //scaleSlider.SetActive(true);

        
        searchForGameButton.SetActive(false);

        informUIPanel_Text.text = "Move phone to detect GO! platform!";  
    }




    
}
