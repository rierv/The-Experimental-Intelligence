using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// The Menu types is used to find the right panels.
/// </summary>
#region Menu Type
[System.Serializable]
public enum r_MenuType
{
    Empty,
    Username,
    RoomBrowser,
    CreateRoom
}
#endregion

/// <summary>
/// We are using serializable class to have each panel with the components organized.
/// </summary>
#region Menu Item
[System.Serializable]
public class m_MenuItem
{
    [Header("Panel Type")]
    public r_MenuType m_MenuType;

    [Header("Panel")]
    public GameObject m_Panel;

    [Header("Buttons")]
    public Button m_OpenButton;
    public Button m_CloseButton;

    [Header("Start Setup")]
    public bool m_OnStart;
}
#endregion

public class r_MenuController : MonoBehaviour
{
    #region Variables
    [Header("Menu Panels")]
    public List<m_MenuItem> m_MenuPanels = new List<m_MenuItem>();

    [Header("Menu Buttons Panel")]
    public GameObject m_MenuButtonsPanel;

    [Header("Username UI")]
    public InputField m_UsernameInput;
    public Button m_ApplyUsernameButton;
    #endregion

    #region Unity Calls
    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
            r_PhotonHandler.instance.ConnectToPhoton();

        DisableAllPanels();
        SetupMenuPanel(true);
        CheckUsername();
        HandleButtons();
    }
    #endregion

    /// <summary>
    /// We are setting the username of the player.
    /// </summary>
    #region Set Username
    private void CheckUsername()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (PlayerPrefs.HasKey("username"))
            {
                m_MenuButtonsPanel.SetActive(true);
                SetupMenuPanel(false);

                PhotonNetwork.LocalPlayer.NickName = PlayerPrefs.GetString("username");
                m_UsernameInput.text = PlayerPrefs.GetString("username");
            }
            else
                m_UsernameInput.text = "Player" + Random.Range(1, 999);
        }
    }

    private void SaveUsername(string _Username)
    {
        PlayerPrefs.SetString("username", _Username);

        if (PhotonNetwork.IsConnected)
            PhotonNetwork.LocalPlayer.NickName = PlayerPrefs.GetString("username");
    }
    #endregion

    /// <summary>
    /// In this section we are handling enabling and disabling of the panels.
    /// </summary>
    #region Handle UI
    private void SetupMenuPanel(bool _State)
    {
        foreach (m_MenuItem _MenuItem in m_MenuPanels)
        {
            if (_MenuItem.m_OnStart)
                _MenuItem.m_Panel.SetActive(_State);
        }
    }

    private void HandleButtons()
    {
        m_ApplyUsernameButton.onClick.AddListener(delegate { if (!string.IsNullOrEmpty(m_UsernameInput.text)) SaveUsername(m_UsernameInput.text); r_AudioController.instance.PlayClickSound(); });

        foreach (m_MenuItem _MenuItem in m_MenuPanels)
        {
            if (_MenuItem.m_OpenButton != null)
            {
                _MenuItem.m_OpenButton.onClick.AddListener(delegate
                {
                    DisableAllPanels();
                    m_MenuButtonsPanel.SetActive(false);
                    r_AudioController.instance.PlayClickSound();

                    if (_MenuItem.m_Panel != null)
                        _MenuItem.m_Panel.SetActive(true);
                });
            }

            if (_MenuItem.m_CloseButton != null)
                _MenuItem.m_CloseButton.onClick.AddListener(delegate { if (_MenuItem.m_Panel != null) DisableAllPanels(); r_AudioController.instance.PlayClickSound(); m_MenuButtonsPanel.SetActive(true); });
        }
    }

    private void DisableAllPanels()
    {
        foreach (m_MenuItem _Panel in m_MenuPanels)
            _Panel.m_Panel.SetActive(false);
    }
    #endregion
}