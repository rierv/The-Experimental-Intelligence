using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour
{
    public Quaternion myRotation;
    // Start is called before the first frame update
    void Awake()
    {
        myRotation = Input.gyro.attitude;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Inverse(myRotation) * Input.gyro.attitude;

    }
}
