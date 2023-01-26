using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class r_CreateRoomControllerUI : MonoBehaviour
{
    #region Variables
    
    [Header("Room Name Field")]
    public InputField m_RoomNameInput;

    [Header("Create Room Button")]
    public Button m_CreateRoomButton;

    [Header("Room Name Field")]
    public Text m_GameModifiersText;
    public Text m_GameModesText;
    public Text m_PlayerLimitText;

    [Header("Room modifiers UI")]
    public Button m_NextModifiersButton;
    public Button m_PreviousModifiersButton;

    [Header("Game Mode UI")]
    public Button m_NextGameModeButton;
    public Button m_PreviousGameModeButton;

    [Header("Player Limit UI")]
    public Button m_NextPlayerLimitButton;
    public Button m_PreviousPlayerLimitButton;
    #endregion
}
