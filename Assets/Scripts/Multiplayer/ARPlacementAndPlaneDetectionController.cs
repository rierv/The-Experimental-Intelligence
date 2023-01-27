using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;
using Photon.Pun;

public class ARPlacementAndPlaneDetectionController : MonoBehaviour
{
    
    public GameObject searchForGameButton;
    //public GameObject scaleSlider;
    public int PlayersThatFoundGoPlatform;
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

        //searchForGameButton.SetActive(true);
        //FindObjectOfType<MultiplayerGameManager>().JoinRandomRoom();
        if (PhotonNetwork.CurrentRoom.PlayerCount > PlayersThatFoundGoPlatform)
        {
            informUIPanel_Text.text = "Great! You found the GO! marker.. Now wait for other players!";
            StartCoroutine(WaitForOtherPlayersToFindGoPlatform());
        }
        else
        {
            StartCoroutine(DeactivateAfterSeconds(informUIPanel_Text.transform.parent.gameObject, 2));
        }

    }

    public void EnableARPlacementAndPlaneDetection()
    {
        
        //scaleSlider.SetActive(true);

        
        searchForGameButton.SetActive(false);

        informUIPanel_Text.text = "Move phone to detect GO! platform!";  
    }

    IEnumerator WaitForOtherPlayersToFindGoPlatform()
    {
        while (PhotonNetwork.CurrentRoom.PlayerCount < PlayersThatFoundGoPlatform)
        {
            yield return new WaitForSeconds(.5f);
            Debug.Log("number of players that found platform" + PlayersThatFoundGoPlatform + " vs " + PhotonNetwork.CurrentRoom.PlayerCount + " tot");
        }

        StartCoroutine(DeactivateAfterSeconds(informUIPanel_Text.transform.parent.gameObject, 2));
        
    }

    IEnumerator DeactivateAfterSeconds(GameObject _gameObject, float _seconds)
    {
        informUIPanel_Text.text = "Ready to start!";
        yield return new WaitForSeconds(_seconds);
        GameObject.FindObjectOfType<SpawnManager>().SpawnPlayer();
        _gameObject.SetActive(false);
        this.enabled=false;
    }


}
