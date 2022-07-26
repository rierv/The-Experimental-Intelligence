using System.Collections;
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
        PlatformFoundEventCode = 1
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
            Vector3 receivedPosition = (Vector3)data[0];
            Quaternion receivedRotation = (Quaternion)data[1];
            int receivedPlayerSelectionData = (int)data[3];

            GameObject player = Instantiate(playerPrefabs[receivedPlayerSelectionData], receivedPosition + startPosition, receivedRotation);
            PhotonView _photonView = player.GetComponent<PhotonView>();
            _photonView.ViewID = (int)data[2];


        }
        else if (photonEvent.Code == (byte)RaiseEventCodes.PlatformFoundEventCode)
        {
            startPosition = imageTracking.placedPrefabs["Start"].transform.position;
            object[] data = (object[])photonEvent.CustomData;
            Vector3 receivedPosition = (Vector3)data[0];
            Vector3 receivedForward = (Vector3)data[1];
            Vector3 receivedUp = (Vector3)data[2];
            string receivedPlatformName = (string)data[4];
            GameObject platform = imageTracking.placedPrefabs[receivedPlatformName];
            platform.transform.position = receivedPosition + startPosition;
            platform.transform.rotation = Quaternion.LookRotation(receivedForward, receivedUp);
            PhotonView _photonView = platform.GetComponent<PhotonView>();
            _photonView.ViewID = (int)data[3];
            platform.SetActive(true);

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
            Debug.Log(trackables.childCount+"figliii");
            Debug.Log(GameObject.Find("Start"));
            startPosition = imageTracking.placedPrefabs["Start"].transform.position;
            //startPosition = trackables.Find("Start").transform.position;
            Debug.Log("spawnoPlayer");

            int randomSpawnPoint = Random.Range(0, spawnPositions.Length - 1);
            Vector3 instantiatePosition = spawnPositions[randomSpawnPoint].localPosition;

            GameObject playerGameobject = Instantiate(playerPrefabs[(int)playerSelectionNumber], instantiatePosition + startPosition, Quaternion.identity);

            PhotonView _photonView = playerGameobject.GetComponent<PhotonView>();

            if (PhotonNetwork.AllocateViewID(_photonView))
            {

                object[] data = new object[]
                {
                    playerGameobject.transform.position- startPosition, playerGameobject.transform.rotation, _photonView.ViewID, playerSelectionNumber
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
