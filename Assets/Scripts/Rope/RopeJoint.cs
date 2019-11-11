using UnityEngine;

public class RopeJoint : MonoBehaviour
{

    public Transform ConnectedRigidbody;
    public bool DetermineDistanceOnStart = true;
    public float Distance;
    public float Spring = 0.1f;
    public float Damper = 5f;
    Vector3 startPos;
    protected Rigidbody rigidbody;
    public RopeJoint postJoint;
    Vector3 postJointPosition;
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        if (DetermineDistanceOnStart && ConnectedRigidbody != null)
            Distance = Vector3.Distance(rigidbody.position, ConnectedRigidbody.position);
        //postJointPosition = postJoint.transform.position;
        startPos=transform.position;
    }

    void FixedUpdate()
    {

        var connection = rigidbody.position - ConnectedRigidbody.position;
        var distanceDiscrepancy = Distance - connection.magnitude;

        rigidbody.position += distanceDiscrepancy * connection.normalized;

        var velocityTarget = connection + (rigidbody.velocity + Physics.gravity * Spring);
        var projectOnConnection = Vector3.Project(velocityTarget, connection);
        
        if (rigidbody.velocity.magnitude > 0)
        {
            Vector3 vel = rigidbody.velocity;
            Transform father=ConnectedRigidbody;
            while (vel.magnitude > 0.1f && father && father.gameObject.name != "Bone" && father.gameObject.name != "Armature") ;
            {
                vel = vel / 1.5f;
                father.gameObject.GetComponent<Rigidbody>().velocity = vel;
                father = father.gameObject.GetComponent<RopeJoint>().ConnectedRigidbody;
            }
        }
        rigidbody.velocity = (velocityTarget - projectOnConnection) / (1 + Damper * Time.fixedDeltaTime);
    }
    
}