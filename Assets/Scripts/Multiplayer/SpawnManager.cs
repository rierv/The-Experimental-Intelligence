using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
public class SpawnManager : MonoBehaviourPunCallbacks
{

    public GameObject playerPrefab;
    public Transform[] spawnPositions;
    public Transform trackables;
    Vector3 startPosition;
    public ImageTrackingMultiplayer imageTracking;
    public Dictionary<int, GameObject> placedPlayers = new Dictionary<int, GameObject>();

    public enum RaiseEventCodes
    {
        PlayerSpawnEventCode = 0,
        PlatformFoundEventCode = 1,
        NewPlatformFoundEventCode = 2,
        BoltOwner = 3,
        BoltThrow = 4,
        GaseousTransformation = 5,
        StartFound = 6
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
            //Material myColor = (Material)data[3];
            GameObject player = Instantiate(playerPrefab);
            
            //player.GetComponentInChildren<Renderer>().material = myColor;
            player.transform.rotation = imageTracking.placedPrefabs["Start"].transform.rotation;
            player.transform.parent = imageTracking.placedPrefabs["Start"].transform;
            player.transform.localPosition = receivedPosition;
            PhotonView _photonView = player.GetComponent<PhotonView>();
            _photonView.ViewID = (int)data[2];
            placedPlayers.Add(_photonView.ViewID, player);


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
        else if (photonEvent.Code == (byte)RaiseEventCodes.BoltOwner)
        {
            object[] data = (object[])photonEvent.CustomData;
            int _playerphotonView;
            _playerphotonView = (int)data[0];
            GameObject platform = imageTracking.placedPrefabs["Bolt"];
            ThrowableObject th = platform.GetComponentInChildren<ThrowableObject>();
            th.enabled = true;
            th.core = placedPlayers[_playerphotonView].GetComponentInChildren<JellyCore>();
            th.state = placedPlayers[_playerphotonView].GetComponentInChildren<StateManager>();
        }
        else if (photonEvent.Code == (byte)RaiseEventCodes.BoltThrow)
        {
            object[] data = (object[])photonEvent.CustomData;
            GameObject platform = imageTracking.placedPrefabs["Bolt"];
            Vector3 _playerSpeed;
            _playerSpeed = (Vector3)data[0];
            ThrowableObject th = platform.GetComponentInChildren<ThrowableObject>();
            int _playerphotonView;
            _playerphotonView = (int)data[1];
            placedPlayers[_playerphotonView].GetComponentInChildren<Rigidbody>().velocity = _playerSpeed; 
            th.enabled = false;
        }
        else if (photonEvent.Code == (byte)RaiseEventCodes.GaseousTransformation)
        {
            object[] data = (object[])photonEvent.CustomData;
            int _playerphotonView;
            _playerphotonView = (int)data[0];
            placedPlayers[_playerphotonView].GetComponentInChildren<StateManager>().temperature = 1;
        }
        else if (photonEvent.Code == (byte)RaiseEventCodes.StartFound)
        {
            GameObject.FindObjectOfType<ARPlacementAndPlaneDetectionController>().PlayersThatFoundGoPlatform++;
        }
    }



    
    #endregion


    #region Private Methods
    public void SpawnPlayer()
    {
        startPosition = imageTracking.placedPrefabs["Start"].transform.position;
        //startPosition = trackables.Find("Start").transform.position;
            
        int randomSpawnPoint = Random.Range(0, spawnPositions.Length - 1);
        Vector3 instantiatePosition = spawnPositions[randomSpawnPoint].localPosition;

        GameObject playerGameobject = Instantiate(playerPrefab, instantiatePosition + startPosition, imageTracking.placedPrefabs["Start"].transform.rotation);
        playerGameobject.transform.parent = imageTracking.placedPrefabs["Start"].transform;
        PhotonView _photonView = playerGameobject.GetComponent<PhotonView>();
        //Material myColor = new Material(Shader.Find("Standard"));
        //myColor.color = new Color(PlayerPrefs.GetInt("colorR"), PlayerPrefs.GetInt("colorG"), PlayerPrefs.GetInt("colorB"));
        if (PhotonNetwork.AllocateViewID(_photonView))
        {
            placedPlayers.Add(_photonView.ViewID, playerGameobject);
            object[] data = new object[]
            {
                playerGameobject.transform.localPosition, playerGameobject.transform.rotation, _photonView.ViewID, //myColor
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



    #endregion




}
