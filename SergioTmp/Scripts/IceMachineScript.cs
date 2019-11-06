using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMachineScript : MonoBehaviour
{
    #region Attributes
    public Transform cubeQueue;
    public Rigidbody machineDoor;
    public Rigidbody blockRigidbody;
    private Rigidbody instance;
    private Vector3 temperatureBlockForce = new Vector3(100f, 0f, 0f);
    private Vector3 doorPositionDifference = new Vector3(0f, 0.5f, 0f);
    private bool isMachineActive = false;
    private float machineTime = 10f;
    private float doorTime = 8f;
    private float currentTimeLeft;
    private bool isDoorOpen;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        ActivateMachine();
        OpenMachineDoor();
        CreateNewCube();
    }

    void ActivateMachine()
    {
        isMachineActive = true;
        currentTimeLeft = machineTime;
    }

    void OpenMachineDoor()
    {
        machineDoor.MovePosition(machineDoor.position + doorPositionDifference);
        isDoorOpen = true;
    }

    void CloseMachineDoor()
    {
        machineDoor.MovePosition(machineDoor.position - doorPositionDifference);
        isDoorOpen = false;
    }

    void CreateNewCube()
    {
        if (isDoorOpen)
        {
            instance = Instantiate(blockRigidbody, cubeQueue.position, cubeQueue.rotation, cubeQueue);
            instance.AddForce(temperatureBlockForce);
            currentTimeLeft = machineTime;
        }
    }


    private void FixedUpdate()
    {
        if(isMachineActive)
        {
            if(currentTimeLeft <= 0f)
            {
                OpenMachineDoor();
                CreateNewCube();
            }
            else
            {
                if (currentTimeLeft < doorTime && isDoorOpen)
                    CloseMachineDoor();
                currentTimeLeft -= Time.fixedDeltaTime;

            }
        }
        
    }
}
