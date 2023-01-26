using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class r_RoomBrowserItem : MonoBehaviour
{
    #region Variables
    [Header("Room Browser UI")]
    public Text m_RoomNameText;
    public Text m_MapNameText;
    public Text m_GameModeText;
    public Text m_PlayersText;

    [Header("Join Room UI")]
    public Button m_JoinRoomButton;

    [Header("Room Configuration")]
    public RoomInfo m_RoomInfo;
    #endregion

    #region Unity Calls
    private void Awake() => m_JoinRoomButton.onClick.AddListener(delegate { JoinRoom(); r_AudioController.instance.PlayClickSound(); });
    #endregion

    /// <summary>
    /// In this section below we setup the UI with the room information for our room browser.
    /// There is a join button on the room browser item, if we press on this button, it checks if the room has place for another player.
    /// </summary>
    #region Actions
    public void SetupRoom(RoomInfo _RoomInfo)
    {
        m_RoomInfo = _RoomInfo;

        m_RoomNameText.text = m_RoomInfo.Name;
        m_MapNameText.text = m_RoomInfo.CustomProperties["GameMap"].ToString();
        m_GameModeText.text = m_RoomInfo.CustomProperties["GameMode"].ToString();
        m_PlayersText.text = m_RoomInfo.PlayerCount + "/" + m_RoomInfo.MaxPlayers;
    }

    public void JoinRoom()
    {
        if (m_RoomInfo.PlayerCount == m_RoomInfo.MaxPlayers)
        {
            Debug.Log("Room is full");
            return;
        }

        PhotonNetwork.JoinRoom(m_RoomInfo.Name);
    }
    #endregion
}