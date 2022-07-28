using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MySynchronizationScript : MonoBehaviour, IPunObservable
{

    Rigidbody rb;
    PhotonView photonView;
    Transform oldParent;
    Vector3 networkedPosition;
    //Quaternion networkedRotation;

    public bool synchronizeVelocity = true;
    public bool synchronizeAngularVelocity = true;
    public bool isTeleportEnabled = true;
    public float teleportIfDistanceGreaterThan = 1.0f;

    private float distance;
    private float angle;

    private Transform startTransform;
    Dictionary<string, GameObject> placedPrefabs;

    float currentTime = 0;
    double currentPacketTime = 0;
    double lastPacketTime = 0;

    private void Awake()
    {
        if (synchronizeVelocity || synchronizeAngularVelocity) 
            rb = GetComponent<Rigidbody>();
        photonView = GetComponentInParent<PhotonView>();

        networkedPosition = new Vector3();
        //networkedRotation = new Quaternion();
        placedPrefabs = GameObject.FindObjectOfType<ImageTrackingMultiplayer>().placedPrefabs;
        startTransform = placedPrefabs["Start"].transform;

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            double timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;

            //Update remote player
            transform.localPosition = Vector3.Lerp(transform.localPosition, networkedPosition, Time.deltaTime*2);//distance *(float)(currentTime / timeToReachGoal));
            //transform.rotation = Quaternion.Lerp(rotationAtLastPacket, latestRot, (float)(currentTime / timeToReachGoal));
        }
    }

    private void FixedUpdate()
    {

        if (!photonView.IsMine)
        {
            //rb.transform.localPosition = Vector3.Lerp(rb.transform.localPosition, networkedPosition, distance * (10.0f / PhotonNetwork.SerializationRate));
            //rb.transform.localPosition = Vector3.MoveTowards(rb.transform.localPosition, networkedPosition, distance * (.01f / PhotonNetwork.SerializationRate));
            //rb.rotation = Quaternion.RotateTowards(rb.rotation, networkedRotation, angle * (1.0f / PhotonNetwork.SerializationRate));

            //Lag compensation
            }

    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //Then, photonView is mine and I am the one who controls this player.
            //should send position, velocity etc. data to the other players
            stream.SendNext(transform.localPosition);
            if (oldParent != transform.parent.parent)
            {
                stream.SendNext(transform.parent.parent.name);
                oldParent = transform.parent.parent;
            }
            else
                stream.SendNext(null);

            if (synchronizeVelocity)
            {
                stream.SendNext(rb.velocity);
            }

            if (synchronizeAngularVelocity)
            {
                stream.SendNext(rb.angularVelocity);
            }
        }
        else
        {

            //Called on my player gameobject that exists in remote player's game
            networkedPosition = (Vector3)stream.ReceiveNext();
            string newParentName = (string)stream.ReceiveNext();
            if(newParentName!=null) transform.parent.parent = placedPrefabs[newParentName].transform;

            currentTime = 0.0f;
            lastPacketTime = currentPacketTime;
            currentPacketTime = info.SentServerTime;

            if (isTeleportEnabled)
            {
                if (Vector3.Distance(transform.localPosition, networkedPosition) > teleportIfDistanceGreaterThan)
                {
                    transform.localPosition = networkedPosition;

                }
            }

            if (synchronizeVelocity || synchronizeAngularVelocity)
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));

                if (synchronizeVelocity)
                {
                    rb.velocity = (Vector3)stream.ReceiveNext();

                    networkedPosition += rb.velocity * lag;

                    distance = Vector3.Distance(transform.localPosition, networkedPosition);
                }

                if (synchronizeAngularVelocity)
                {
                    rb.angularVelocity = (Vector3)stream.ReceiveNext();

                    //networkedRotation = Quaternion.Euler(rb.angularVelocity * lag) * networkedRotation;

                    //angle = Quaternion.Angle(rb.rotation, networkedRotation);
                }
            }
        }
    }






}
