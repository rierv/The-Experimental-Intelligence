﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
public class SpawnManager : MonoBehaviourPunCallbacks
{

    public GameObject[] playerPrefabs;
    public Transform[] spawnPositions;
    public Transform trackables;
    Vector3 startPosition;
    public ImageTrackingMultiplayer imageTracking;

    public enum RaiseEventCodes
    {
        PlayerSpawnEventCode = 0,
        PlatformFoundEventCode = 1,
        NewPlatformFoundEventCode = 2
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }




    #region Photon Callback Methods
    void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)RaiseEventCodes.PlayerSpawnEventCode)
        {
            startPosition = imageTracking.placedPrefabs["Start"].transform.position;
            object[] data = (object[])photonEvent.CustomData;
            
            int receivedPlayerSelectionData = (int)data[3];
            Vector3 receivedPosition = (Vector3)data[0];
            Quaternion receivedLocalRotation = (Quaternion)data[1];
            
            GameObject player = Instantiate(playerPrefabs[receivedPlayerSelectionData]);
            player.transform.rotation = imageTracking.placedPrefabs["Start"].transform.rotation;
            player.transform.parent = imageTracking.placedPrefabs["Start"].transform;
            player.transform.localPosition = receivedPosition;
            PhotonView _photonView = player.GetComponent<PhotonView>();
            _photonView.ViewID = (int)data[2];


        }
        else if (photonEvent.Code == (byte)RaiseEventCodes.PlatformFoundEventCode)
        {
            startPosition = imageTracking.placedPrefabs["Start"].transform.position;
            object[] data = (object[])photonEvent.CustomData;
            Vector3 receivedPosition = (Vector3)data[0];
            Quaternion receivedLocalRotation = (Quaternion)data[1];
            string receivedPlatformName = (string)data[2];
            GameObject platform = imageTracking.placedPrefabs[receivedPlatformName];
            platform.transform.localPosition = receivedPosition;
            platform.transform.localRotation = receivedLocalRotation;
            //platform.transform.SetPositionAndRotation(receivedPosition + startPosition, receivedLocalRotation);
            
            platform.SetActive(true);

        }
        else if (photonEvent.Code == (byte)RaiseEventCodes.NewPlatformFoundEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            string receivedPlatformName = (string)data[1];
            GameObject platform = imageTracking.placedPrefabs[receivedPlatformName];
            PhotonView _photonView = platform.GetComponent<PhotonView>();
            _photonView.ViewID = (int)data[0];

        }
    }



    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {

            //object playerSelectionNumber;
            //if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerARSpinnerTopGame.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
            //{
            //    Debug.Log("Player selection number is "+ (int)playerSelectionNumber);
            //    startPosition = trackables.Find("Start").transform.position;
            //    int randomSpawnPoint = Random.Range(0, spawnPositions.Length - 1);
            //    Vector3 instantiatePosition = spawnPositions[randomSpawnPoint].localPosition + startPosition;


            //    PhotonNetwork.Instantiate(playerPrefabs[(int)playerSelectionNumber].name, instantiatePosition, Quaternion.identity);


            //}
            SpawnPlayer();


        }



    }
    #endregion


    #region Private Methods
    private void SpawnPlayer()
    {
        object playerSelectionNumber;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerARSpinnerTopGame.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
        {
            Debug.Log("Player selection number is " + (int)playerSelectionNumber);
            
            startPosition = imageTracking.placedPrefabs["Start"].transform.position;
            //startPosition = trackables.Find("Start").transform.position;

            int randomSpawnPoint = Random.Range(0, spawnPositions.Length - 1);
            Vector3 instantiatePosition = spawnPositions[randomSpawnPoint].localPosition;

            GameObject playerGameobject = Instantiate(playerPrefabs[(int)playerSelectionNumber], instantiatePosition + startPosition, imageTracking.placedPrefabs["Start"].transform.rotation);
            playerGameobject.transform.parent = imageTracking.placedPrefabs["Start"].transform;
            PhotonView _photonView = playerGameobject.GetComponent<PhotonView>();

            if (PhotonNetwork.AllocateViewID(_photonView))
            {

                object[] data = new object[]
                {
                    playerGameobject.transform.localPosition, playerGameobject.transform.rotation, _photonView.ViewID, playerSelectionNumber
                };


                RaiseEventOptions raiseEventOptions = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others,
                    CachingOption = EventCaching.AddToRoomCache

                };


                SendOptions sendOptions = new SendOptions
                {
                    Reliability = true
                };

                //Raise Events!
                PhotonNetwork.RaiseEvent((byte)RaiseEventCodes.PlayerSpawnEventCode, data, raiseEventOptions, sendOptions);


            }
            else
            {

                Debug.Log("Failed to allocate a viewID");
                Destroy(playerGameobject);
            }



        }
    }



    #endregion




}