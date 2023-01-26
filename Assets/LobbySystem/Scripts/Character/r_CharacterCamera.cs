using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class r_CharacterCamera : MonoBehaviour
{
    #region Variables
    [Header("Transforms")]
    public Transform m_CharacterTransform;
    public Transform m_CameraTransform;

    [Header("Camera Settings")]
    public float m_Sensitivity;
    public float m_MouseClampY;

    [Header("Mouse Inputs")]
    private float m_MouseX;
    private float m_MouseY;
    #endregion

    #region Unity Calls
    private void Update() => HandleCamera();
    #endregion

    /// <summary>
    /// We are handling the first person camera from here using mouse X and Y.
    /// </summary>

    #region Camera
    private void HandleCamera()
    {
        m_MouseX += Input.GetAxis("Mouse X") * m_Sensitivity;
        m_MouseY += Input.GetAxis("Mouse Y") * m_Sensitivity;

        m_MouseX %= 360f;
        m_MouseY = Mathf.Clamp(this.m_MouseY, -m_MouseClampY, m_MouseClampY);

        m_CharacterTransform.rotation = Quaternion.Euler(0f, m_MouseX, 0f);
        m_CameraTransform.localRotation = Quaternion.Euler(-m_MouseY, 0, 0f);
    }
    #endregion
}
