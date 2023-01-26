using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class r_AudioController : MonoBehaviour
{
    public static r_AudioController instance;

    /// <summary>
    /// This script is used for the button click sounds.
    /// </summary>

    #region Variables
    [Header("Audio Source")]
    public AudioSource m_AudioSource;

    [Header("Audio Clip")]
    public AudioClip m_ClickSound;
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
    }
    #endregion

    #region Play Sound
    public void PlayClickSound()
    {
        if (m_AudioSource != null && m_ClickSound != null)
            m_AudioSource.PlayOneShot(m_ClickSound);
    }
    #endregion
}
