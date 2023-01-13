using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviourPun
{

    public TextMeshProUGUI playerNameText;

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            //The player is local player. 
            transform.GetComponent<PlayerMove>().enabled = true;
            //transform.GetComponent<MovementController>().joystick.gameObject.SetActive(true);
        }
        else
        {

            //The player is remote player
            transform.GetComponent<PlayerMove>().enabled = false;
            //transform.GetComponent<SphereCollider>().enabled = false;
            transform.GetComponent<ThrowObjects>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            transform.GetComponent<JellyCore>()._cohesion = 2;
            transform.GetComponent<Rigidbody>().drag = 2f;
            //transform.GetComponent<MovementController>().joystick.gameObject.SetActive(false);
        }
        SetPlayerName();

    }
    
    void SetPlayerName()
    {

        if (playerNameText != null)
        {
            if (photonView.IsMine)
            {
                playerNameText.text = "YOU";
                playerNameText.color = Color.red;
            }
            else
            {
                playerNameText.text = photonView.Owner.NickName;

            }

        }

    }

   
}
