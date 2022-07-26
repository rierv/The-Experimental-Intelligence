using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class SpinningTopsGameManager :MonoBehaviourPunCallbacks
{

    [Header("UI")]
    public GameObject uI_InformPanelGameobject;
    public TextMeshProUGUI uI_InformText;
    public GameObject searchForGamesButtonGameobject;


    // Start is called before the first frame update
    void Start()
    {
        uI_InformPanelGameobject.SetActive(true);
        //uI_InformText.text = "Search For Games to BATTLE!";

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region UI Callback Methods
    public void JoinRandomRoom()
    {
        uI_InformText.text = "Searching for available rooms...";

        PhotonNetwork.JoinRandomRoom();

        searchForGamesButtonGameobject.SetActive(false);


    }


    public void OnQuitMatchButtonClicked()
    {

        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();

        }
        else
        {
            SceneLoader.Instance.LoadScene("Scene_Lobby");
        }
        


    }
    #endregion


    #region PHOTON Callback Methods
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
      
        Debug.Log(message);
        uI_InformText.text = message;

        CreateAndJoinRoom();
    }


    public override void OnJoinedRoom()
    {

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            uI_InformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name + ". Waiting for other players...";
            StartCoroutine(DeactivateAfterSeconds(uI_InformPanelGameobject, 2.0f));

        }
        else
        {
            uI_InformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name;
            StartCoroutine(DeactivateAfterSeconds(uI_InformPanelGameobject, 2.0f));
        }

        Debug.Log( " joined to "+ PhotonNetwork.CurrentRoom.Name);
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined to "+ PhotonNetwork.CurrentRoom.Name+ " Player count "+ PhotonNetwork.CurrentRoom.PlayerCount);
        uI_InformText.text = newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " Player count " + PhotonNetwork.CurrentRoom.PlayerCount;

        StartCoroutine(DeactivateAfterSeconds(uI_InformPanelGameobject, 2.0f));


    }


    public override void OnLeftRoom()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }


    #endregion


    #region PRIVATE Methods
    void CreateAndJoinRoom()
    {
        string randomRoomName = "Room" + Random.Range(0,1000);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;

        //Creatin the room
        PhotonNetwork.CreateRoom(randomRoomName,roomOptions);

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("non sono riuscito a creare la stanza");
        base.OnCreateRoomFailed(returnCode, message);
    }

    IEnumerator DeactivateAfterSeconds(GameObject _gameObject, float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        _gameObject.SetActive(false);

    }


    #endregion



}
