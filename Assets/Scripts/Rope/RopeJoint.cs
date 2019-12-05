using UnityEngine;

public class RopeJoint : MonoBehaviour
{

    public Transform ParentTransform;
    public bool DetermineDistanceOnStart = true;
    public float Distance;
    public float Spring = 5f;
    public float Damper = 5f;
    GameObject pointer;
    GameObject cursor;
    protected Rigidbody Rigidbody;
    public float movementSpeed = 5f;
    public float rotationSpeed = 5f;
    public float cursorSpeed=1000f;
    public float maxRotationOfBone=1f;
    Quaternion originalQuaternion;
    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        if (DetermineDistanceOnStart && ParentTransform != null)
            Distance = Vector3.Distance(Rigidbody.position, ParentTransform.position);
        cursor = new GameObject();
        pointer = new GameObject();
        pointer.transform.position = transform.position;
        pointer.transform.parent = transform;
        cursor.transform.position = transform.position;
        cursor.transform.parent = transform;
        originalQuaternion=transform.rotation;
    }

    void Update()
    {
        
        var connection = Rigidbody.position - ParentTransform.position;
        var distanceDiscrepancy = Distance - connection.magnitude;
        cursor.transform.position= Vector3.Lerp(transform.position, transform.position + new Vector3(-connection.normalized.x, -(Mathf.Sqrt(connection.normalized.x* connection.normalized.x) + Mathf.Sqrt(connection.normalized.z* connection.normalized.z))*maxRotationOfBone, -connection.normalized.z)*distanceDiscrepancy*100, Time.deltaTime * cursorSpeed);
        Rigidbody.position = Vector3.Lerp(Rigidbody.position, Rigidbody.position+ distanceDiscrepancy * connection.normalized, Time.deltaTime*movementSpeed);
        if (Mathf.Abs(connection.normalized.x) < 0.1 && Mathf.Abs(connection.normalized.z) < 0.1) pointer.transform.rotation = Quaternion.Lerp(pointer.transform.rotation, originalQuaternion, Time.deltaTime*rotationSpeed); 
        else pointer.transform.LookAt(cursor.transform.position);
        transform.rotation = Quaternion.Lerp(cursor.transform.rotation, pointer.transform.rotation, Time.deltaTime*rotationSpeed);
        Debug.DrawRay(transform.position, transform.forward, Color.green, 20, true);
        var velocityTarget = connection + (Rigidbody.velocity + Physics.gravity * Spring);
        var projectOnConnection = Vector3.Project(velocityTarget, connection);
        Rigidbody.velocity = (velocityTarget - projectOnConnection) / (1 + Damper * Time.fixedDeltaTime);


    }
}