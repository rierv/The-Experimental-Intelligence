using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class r_CharacterMovement : MonoBehaviour
{
    #region Variables
    [Header("Character Controller")]
    public CharacterController m_CharacterController;

    [Header("Movement Settings")]
    public float m_MovementSpeed;
    public float m_GravityMultiplier;

    [Header("Movement Direction")]
    private Vector3 m_MoveDirection;
    #endregion

    #region Unity Calls
    private void Update() => HandleMovement();
    #endregion

    /// <summary>
    /// Handling the horizontal and vertical movement of the player.
    /// </summary>

    #region Movement
    private void HandleMovement()
    {
        if (m_CharacterController.isGrounded)
        {
            m_MoveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            m_MoveDirection = transform.TransformDirection(m_MoveDirection);

            m_MoveDirection = m_MoveDirection.normalized * m_MovementSpeed;
        }

        m_MoveDirection += Physics.gravity * m_GravityMultiplier * Time.deltaTime;
        m_CharacterController.Move(m_MoveDirection * Time.deltaTime);
    }
    #endregion
}
