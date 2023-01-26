using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class r_LobbyController : MonoBehaviour
{
    public static r_LobbyController instance;

    #region Variables
    [Header("PhotonView")]
    public PhotonView m_PhotonView;

    [Header("Lobby UI")]
    public r_LobbyControllerUI m_LobbyUI;

    [Header("Rooms List")]
    private List<RoomInfo> m_RoomList = new List<RoomInfo>();

    [Header("Matchmaking Time")]
    public float m_SearchGameTime;
    public float m_StartGameTime;
    public float m_RejoinLobbyStartTime;

    [Header("Matchmaking Configuration")]
    public int m_RequiredPlayers;

    private bool m_StartingGame;
    private bool m_RejoinLobby;
    #endregion

    #region Unity Calls
    private void Awake()
    {
        if (instance)
        {
            Destroy(instance);
            Destroy(instance.gameObject);
        }

        instance = this;
        HandleButtons();
    }

    private void Start() => HandleRejoinLobby();

    private void Update()
    {
        //if (InGameLobby())
        //    HandleLobbySearching();
    }
    #endregion

    /// <summary>
    /// We are receiving the room list to check if there are rooms. If there any, we are joining the room.
    /// </summary>

    #region Room List
    public void ReceiveRoomList(List<RoomInfo> _RoomList) => m_RoomList = _RoomList;
    #endregion

    /// <summary>
    /// Handling all the functions
    /// </summary>

    #region Handle Lobby
    public void HandleRejoinLobby()
    {
        if (InGameLobby())
        {
            m_RejoinLobby = true;
            EnterLobby();
        }
    }

    public void EnterLobby()
    {
        Debug.Log("We entered the lobby");

        SetLobbyMenu(true);
        ListLobbyPlayers();
        DisplayGameInformation();
    }

    public void LeaveLobby()
    {
        if (InGameLobby())
            PhotonNetwork.LeaveRoom();

        CleanLocalPlayerList();
        SetLobbyMenu(false);
    }
    #endregion

    /// <summary>
    /// In the section below we are trying to find or creating a room if no rooms exists. 
    /// //If there is some one waiting in a lobby, we are joining that lobby and if there are enough players we are starting the game with a delay.
    /// </summary>

    #region Matchmaking
    public void HandleLobbySearching()
    {
        if ((string)PhotonNetwork.CurrentRoom.CustomProperties["GameMode"] == "Casual")
        {

            if (PhotonNetwork.CurrentRoom.PlayerCount >= m_RequiredPlayers && !m_StartingGame)
                StartCoroutine(StartGame());
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount >= m_RequiredPlayers + 1 && !m_StartingGame)
            StartCoroutine(StartGame());
    }

    public bool InGameLobby()
    {
        if (PhotonNetwork.InRoom && ((string)PhotonNetwork.CurrentRoom.CustomProperties["RoomState"] == "InLobby"))
            return true; else return false;
    }

    public IEnumerator SearchGame()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);

        yield return new WaitForSeconds(m_SearchGameTime);

        if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InLobby)
        {
            if (m_RoomList.Count > 0)
            {
                PhotonNetwork.JoinRandomRoom();
                DisplayGameInformation();
            }
            else
            {
                r_PhotonHandler.instance.CreateRoom("Room " + Random.Range(0, 999), r_CreateRoomController.instance.SetRoomOptions(true));

                yield return new WaitForSeconds(2f);

                DisplayGameInformation();
            }
        }
    }

    public IEnumerator StartGame()
    {
        Debug.Log("Starting Game");

        m_StartingGame = true;

        //yield return new WaitForSeconds(m_RejoinLobby ? m_RejoinLobbyStartTime : m_StartGameTime);
        yield return new WaitForSeconds(.2f);


        if (PhotonNetwork.CurrentRoom.PlayerCount < m_RequiredPlayers)
        {
            m_StartingGame = false;
            yield break;
        }

        Hashtable _State = new Hashtable(); _State.Add("RoomState", "InGame");
        PhotonNetwork.CurrentRoom.SetCustomProperties(_State);

        r_PhotonHandler.instance.LoadGame();
        m_StartingGame = true;
    }
    #endregion

    /// <summary>
    /// We are using this local player list before we joined a room. It is not possible to call RPC's when you are not in a room.
    /// </summary>

    #region Local PlayerList
    public void AddLocalPlayerToList(Player _PhotonPlayer)
    {
        GameObject _PlayerListEntry = (GameObject)Instantiate(m_LobbyUI.m_PlayerListEntry.gameObject, m_LobbyUI.m_PlayerListContent);
        _PlayerListEntry.GetComponent<r_LobbyEntry>().SetupLobbyPlayer(_PhotonPlayer);
    }

    public void CleanLocalPlayerList()
    {
        if (m_LobbyUI.m_PlayerListContent.childCount > 0)
        {
            foreach (Transform _PhotonPlayer in m_LobbyUI.m_PlayerListContent)
                Destroy(_PhotonPlayer.gameObject);
        }
    }
    #endregion

    /// <summary>
    /// We are listing the lobby players in our player list when we joined a room/lobby.
    /// </summary>

    #region Lobby PlayerList
    public void ListLobbyPlayers() => m_PhotonView.RPC("ListLobbyPlayersRPC", RpcTarget.All);

    [PunRPC]
    public void ListLobbyPlayersRPC()
    {
        if (m_LobbyUI.m_PlayerListContent.childCount > 0)
        {
            foreach (Transform _PhotonPlayer in m_LobbyUI.m_PlayerListContent)
                Destroy(_PhotonPlayer.gameObject);
        }

        foreach (Player _PhotonPlayer in PhotonNetwork.PlayerList)
        {
            GameObject _LobbyPlayer = Instantiate(m_LobbyUI.m_PlayerListEntry, m_LobbyUI.m_PlayerListContent);
            _LobbyPlayer.GetComponent<r_LobbyEntry>().SetupLobbyPlayer(_PhotonPlayer);
        }
    }
    #endregion

    /// <summary>
    /// In the sections below we are handling the lobby buttons. 
    /// Displaying the room information like image, map name and game mode.
    /// </summary>

    #region Handle UI
    private void HandleButtons()
    {
        m_LobbyUI.m_LeaveLobbyButton.onClick.AddListener(delegate { StopAllCoroutines(); LeaveLobby(); r_AudioController.instance.PlayClickSound(); });
        m_LobbyUI.m_SearchGameButton.onClick.AddListener(delegate { StartCoroutine(SearchGame()); AddLocalPlayerToList(PhotonNetwork.LocalPlayer); SetLobbyMenu(true); DisplayGameInformation(); r_AudioController.instance.PlayClickSound(); });
    }

    public void DisplayGameInformation()
    {
        m_LobbyUI.m_GameInformationText.text = PhotonNetwork.InRoom ? PhotonNetwork.CurrentRoom.CustomProperties["GameModifier"].ToString() + " / " + PhotonNetwork.CurrentRoom.CustomProperties["GameMode"].ToString() : "Searching...";

    }

    public void SetLobbyMenu(bool _State)
    {
        m_LobbyUI.m_LobbyPanel.SetActive(_State ? true : false);
        m_LobbyUI.m_MenuPanel.SetActive(_State ? false : true);
    }
    #endregion
}