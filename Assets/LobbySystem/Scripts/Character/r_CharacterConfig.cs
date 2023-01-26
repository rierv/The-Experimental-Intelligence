using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class r_CharacterConfig : MonoBehaviour
{
    #region Variables
    [Header("Photon View")]
    public PhotonView m_PhotonView;

    [Header("MonoBehaviours")]
    public MonoBehaviour[] m_LocalScripts;

    [Header("Objects To Enable")]
    public GameObject[] m_LocalObjects;

    [Header("Objects To Disable")]
    public GameObject[] m_RemoteObjects;
    #endregion

    /// <summary>
    /// Here we enabling or disabling the local and remote objects and scripts.
    /// We set our username on our transform.
    /// </summary>

    #region Setup Player
    public void SetupLocalPlayer()
    {
        if (!m_PhotonView.IsMine) return;

        foreach (MonoBehaviour _LocalScript in m_LocalScripts) _LocalScript.enabled = true;

        foreach (GameObject _Object in m_LocalObjects) _Object.SetActive(true);

        foreach (GameObject _Object in m_RemoteObjects) _Object.SetActive(false);

        m_PhotonView.RPC("SetupPlayerName", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
    }

    [PunRPC]
    public void SetupPlayerName(string _PlayerName) => this.transform.name = _PlayerName;
    #endregion
}
