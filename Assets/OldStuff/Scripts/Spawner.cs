using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour, I_Activable
{
    public GameObject objectToSpawn;
    public Vector3 sizeOfTheObject = new Vector3(1, 1, 1);
    bool activable = true;

    GameObject instance;
    bool ready = true;
    public void Activate(bool type = true)
    {
        if (ready&&activable)
        {
            ready = false;
            if (instance != null) Destroy(instance);
            instance = Instantiate(objectToSpawn, transform.position, transform.rotation);
            instance.transform.localScale = sizeOfTheObject;
        }
    }

    public void canActivate(bool enabled)
    {
        activable = enabled;
    }

    public void Deactivate()
    {
        ready = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
