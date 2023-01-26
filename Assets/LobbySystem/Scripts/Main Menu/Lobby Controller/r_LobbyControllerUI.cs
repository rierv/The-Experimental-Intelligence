using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class r_LobbyControllerUI : MonoBehaviour
{
    #region Variables
    [Header("Player List Content")]
    public Transform m_PlayerListContent;
    public GameObject m_PlayerListEntry;

    [Header("Lobby UI Buttons")]
    public Button m_SearchGameButton;
    public Button m_LeaveLobbyButton;

    [Header("Menu UI")]
    public GameObject m_MenuPanel;
    public GameObject m_LobbyPanel;

    [Header("Lobby Game Information UI")]
    public Image m_GameMapImage;
    public Text m_GameInformationText;
    #endregion
}
