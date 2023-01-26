using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class r_RoomBrowserController : MonoBehaviour
{
    public static r_RoomBrowserController instance;

    #region Variables
    [Header("Room Browser")]
    public List<RoomInfo> m_RoomBrowserList = new List<RoomInfo>();

    [Header("Room Browser Content")]
    public Transform m_RoomBrowserContent;

    [Header("Room Browser Item")]
    public r_RoomBrowserItem m_RoomBrowserItem;

    [Header("Room Browser Refresh")]
    public Button m_RoomBrowserRefreshButton;
    #endregion

    #region Unity Calls
    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
            Destroy(this.gameObject);
        }

        instance = this;
        HandleButtons();
    }
    #endregion

    /// <summary>
    /// We are also handling refreshing the room browser by a button. 
    /// </summary>
    #region Handle UI
    private void HandleButtons() => m_RoomBrowserRefreshButton.onClick.AddListener(delegate { PhotonNetwork.JoinLobby(TypedLobby.Default); r_AudioController.instance.PlayClickSound(); });
    #endregion

    /// <summary>
    /// In the section below we are refreshing the room browser.
    /// First we remove all room browser items, then add them again. Otherwise it will duplicate them.
    /// </summary>
    #region Handle Room Browser
    public void RefreshRoomBrowser()
    {
        RemoveRoomsBrowserItems();

        foreach (RoomInfo _RoomInfo in m_RoomBrowserList)
        {
            if (!_RoomInfo.IsVisible)
              return;

            if (_RoomInfo.MaxPlayers > 0)
            {
                r_RoomBrowserItem _RoomBrowserItem = (r_RoomBrowserItem)Instantiate(m_RoomBrowserItem, m_RoomBrowserContent.transform);
                _RoomBrowserItem.SetupRoom(_RoomInfo);
            }
        }
    }

    public void RemoveRoomsBrowserItems()
    {
        if (m_RoomBrowserContent.childCount == 0)
            return;

        foreach (Transform _RoomBrowserItem in m_RoomBrowserContent)
            Destroy(_RoomBrowserItem.gameObject);
    }
    #endregion
}