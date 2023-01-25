using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassageForGaseous : MonoBehaviour
{
    bool active = false;
    StateManager flapperState;
    // Start is called before the first frame update
    void Start()
    {
        flapperState = GameObject.Find("CORE").GetComponent<StateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!active && flapperState.state == FlapperState.gaseous)
        {
            active = true;
            GetComponent<BoxCollider>().enabled = false;
        }
        else if (active && flapperState.state != FlapperState.gaseous)
        {
            active = false;
            GetComponent<BoxCollider>().enabled = true;
        }
    }
}
