using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

/// <summary>
/// This serializable class is used to add different maps with game modes.
/// </summary>


public class r_CreateRoomController : MonoBehaviour
{
    public static r_CreateRoomController instance;

    #region Variables
    [Header("Player Limit")]
    public byte[] m_PlayerLimit; [HideInInspector] public int m_CurrentPlayerLimit;

    [Header("Game Modes")]
    public string[] m_GameModes; [HideInInspector] public int m_CurrentGameMode;

    [Header("Game Modifiers")]
    public string[] m_Modifiers; [HideInInspector] public int m_CurrentModifiers;

    [Header("UI")]
    public r_CreateRoomControllerUI m_RoomUI;
    #endregion

    #region Unity Calls
    private void Awake()
    {
        if (instance)
        {
            Destroy(instance.gameObject);
            Destroy(instance);
        }

        instance = this;

        HandleButtons();
        UpdateUI();
    }
    #endregion

    /// <summary>
    /// In this section we are handling what de buttons does, and updating the UI information which display in the main menu.
    /// </summary>
    #region Handle UI
    private void HandleButtons()
    {
        //Change Map Buttons
        m_RoomUI.m_NextModifiersButton.onClick.AddListener(delegate { NextModifier(true); r_AudioController.instance.PlayClickSound(); });
        m_RoomUI.m_PreviousModifiersButton.onClick.AddListener(delegate { NextModifier(false); r_AudioController.instance.PlayClickSound(); });

        //Change Game Mode Buttons
        m_RoomUI.m_NextGameModeButton.onClick.AddListener(delegate { NextGameMode(true); r_AudioController.instance.PlayClickSound(); });
        m_RoomUI.m_PreviousGameModeButton.onClick.AddListener(delegate { NextGameMode(false); r_AudioController.instance.PlayClickSound(); });

        //Change Player Limit Buttons 
        m_RoomUI.m_NextPlayerLimitButton.onClick.AddListener(delegate { NextPlayerLimit(true); r_AudioController.instance.PlayClickSound(); });
        m_RoomUI.m_PreviousPlayerLimitButton.onClick.AddListener(delegate { NextPlayerLimit(false); r_AudioController.instance.PlayClickSound(); });

        //Create Room Button
        m_RoomUI.m_CreateRoomButton.onClick.AddListener(delegate { r_PhotonHandler.instance.CreateRoom(m_RoomUI.m_RoomNameInput.text, SetRoomOptions(false)); r_AudioController.instance.PlayClickSound(); m_RoomUI.m_CreateRoomButton.interactable = false; });
    }

    private void UpdateUI()
    {
        if (m_RoomUI == null) return;

        m_RoomUI.m_GameModifiersText.text = m_Modifiers[m_CurrentModifiers].ToString();
        m_RoomUI.m_GameModesText.text = m_GameModes[m_CurrentGameMode].ToString();
        m_RoomUI.m_PlayerLimitText.text = m_PlayerLimit[m_CurrentPlayerLimit].ToString() + " Players";
    }
    #endregion

    /// <summary>
    /// In this section we are applying the room settings and properties.
    /// </summary>
    #region Set Room Options
    public RoomOptions SetRoomOptions(bool _RandomRoomOptions)
    {
        RoomOptions _RoomOptions = new RoomOptions
        {
            IsVisible = true,
            IsOpen = true,  
            MaxPlayers = _RandomRoomOptions ? (byte) 8 : m_PlayerLimit[m_CurrentPlayerLimit],
        };

        _RoomOptions.CustomRoomProperties = new Hashtable();

        int _RandomModifierID = Random.Range(0, m_Modifiers.Length);

        _RoomOptions.CustomRoomProperties.Add("GameModifier", _RandomRoomOptions ? m_Modifiers[_RandomModifierID] : m_Modifiers[m_CurrentModifiers]);
        _RoomOptions.CustomRoomProperties.Add("GameMode", _RandomRoomOptions ? m_GameModes[Random.Range(0, m_GameModes.Length)] : m_GameModes[m_CurrentGameMode]);
        _RoomOptions.CustomRoomProperties.Add("RoomState", "InLobby");

        string[] _CustomLobbyProperties = new string[3];

        _CustomLobbyProperties[0] = "GameModifier";
        _CustomLobbyProperties[1] = "GameMode";
        _CustomLobbyProperties[2] = "RoomState";

        _RoomOptions.CustomRoomPropertiesForLobby = _CustomLobbyProperties;

        return _RoomOptions;
    }
    #endregion

    /// <summary>
    /// In the sections below we are updating the UI display.
    /// </summary>
    #region Change Game Map
    private void NextModifier(bool _Next)
    {
        if (_Next)
        {
            m_CurrentModifiers++;
            if (m_CurrentModifiers >= m_Modifiers.Length) m_CurrentModifiers = 0;
        }
        else
        {
            m_CurrentModifiers--;
            if (m_CurrentModifiers < 0) m_CurrentModifiers = m_Modifiers.Length - 1;
        }

        UpdateUI();
    }
    #endregion

    #region Change Game Mode
    private void NextGameMode(bool _Next)
    {
        if (_Next)
        {
            m_CurrentGameMode++;
            if (m_CurrentGameMode >= m_GameModes.Length) m_CurrentGameMode = 0;
        }
        else
        {
            m_CurrentGameMode--;
            if (m_CurrentGameMode < 0) m_CurrentGameMode = m_GameModes.Length - 1;
        }
        if (m_GameModes[m_CurrentGameMode] == "Casual") UpdatePlayerNumber(-1);
        else UpdatePlayerNumber(1);
        UpdateUI();
    }
    #endregion

    void UpdatePlayerNumber(int sum)
    {
        for (int i = 0; i< m_PlayerLimit.Length; i++)
        {
            m_PlayerLimit[i] += (byte)sum;
        }
    }

    #region Change Player Limit
    private void NextPlayerLimit(bool _Next)
    {
        if (_Next)
        {
            m_CurrentPlayerLimit++;
            if (m_CurrentPlayerLimit >= m_PlayerLimit.Length) m_CurrentPlayerLimit = 0;
        }
        else
        {
            m_CurrentPlayerLimit--;
            if (m_CurrentPlayerLimit < 0) m_CurrentPlayerLimit = m_PlayerLimit.Length - 1;
        }

        UpdateUI();
    }
    #endregion
}
