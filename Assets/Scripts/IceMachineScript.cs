using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMachineScript : MonoBehaviour
{
    #region Attributes
    public Transform cubeQueue;
    public Transform machineDoor;
    public GameObject block;
    private GameObject instance;
    public float throwBlockForce = 1000f;
    public float blockDuration = 10f;
    public Vector3 doorHightDifference = new Vector3(0f, 0.5f, 0f);
    [Range(1, 5)]
    public float doorVecolicity = 2;
    private bool isMachineActive = false;
    [Range (2, 10)]
    public float newBlockTime = 2f;
    private float closeDoorTime;
    private float openDoorTime;
    private float currentTimeLeft;
    private bool isDoorOpen;
    Vector3 throwForce;
    Vector3 DoorStartpos;
    bool newBlockNeeded=true;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        closeDoorTime = newBlockTime + 1 / doorVecolicity;
        openDoorTime = newBlockTime - 1 / doorVecolicity;
        DoorStartpos = machineDoor.position;
        throwForce = -transform.forward * throwBlockForce;
        ActivateMachine();
    }

    void ActivateMachine()
    {
        isMachineActive = true;
    }

    void OpenMachineDoor()
    {
        isDoorOpen = true;
    }

    void CloseMachineDoor()
    {
        isDoorOpen = false;
    }

    void CreateNewCube()
    {
        if (isDoorOpen)
        {
            
            instance = Instantiate(block, cubeQueue.position, cubeQueue.rotation, cubeQueue);
            instance.GetComponent<TemperatureBlock>().duration = blockDuration;
            instance.GetComponent<Rigidbody>().AddForce(throwForce);
        }
    }


    private void FixedUpdate()
    {
        if(isMachineActive&&newBlockNeeded)
        {
            StartCoroutine(openDoor());
            StartCoroutine(newBlockCoroutine());
            StartCoroutine(closeDoor());
            StartCoroutine(restart());

        }

    }
    IEnumerator openDoor()
    {
        yield return new WaitForSeconds(openDoorTime);
        OpenMachineDoor();
    }
    IEnumerator newBlockCoroutine()
    {
        yield return new WaitForSeconds(newBlockTime);
        CreateNewCube();
    }
    IEnumerator closeDoor()
    {
        yield return new WaitForSeconds(closeDoorTime);
        CloseMachineDoor();
    }
    IEnumerator restart()
    {
        newBlockNeeded = false;
        yield return new WaitForSeconds(newBlockTime*2);
        newBlockNeeded=true;
    }
    private void Update()
    {
        
        if (isDoorOpen)
            machineDoor.position = Vector3.Lerp(machineDoor.position, DoorStartpos + doorHightDifference, Time.deltaTime * doorVecolicity);
        else
            machineDoor.position = Vector3.Lerp(machineDoor.position, DoorStartpos, Time.deltaTime * doorVecolicity);
    }
}
