using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour, I_Activable
{
    bool active;
    Quaternion startRotation;
    public Quaternion aimedRotation;
    public float speed=1;
    public void Activate()
    {
        active = true;
    }

    public void Deactivate()
    {
        active = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (active) transform.rotation = Quaternion.Lerp(transform.rotation, aimedRotation, Time.deltaTime * speed);
        else transform.rotation = Quaternion.Lerp(transform.rotation, startRotation, Time.deltaTime * speed);
    }
}
