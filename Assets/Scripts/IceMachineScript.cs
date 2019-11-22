using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMachineScript : MonoBehaviour {
	#region Attributes
	public Transform cubeQueue;
	public Transform machineDoor;
	public GameObject block;
	private GameObject instance;
	public float throwBlockForce = 1000f;
    public float block_melting_speed = 0.1f;
    public float block_melting_duration = 15f;
    public Vector3 doorHightDifference = new Vector3(0f, 0.5f, 0f);
	[Range(1, 5)]
	public float doorVecolicity = 2;
	private bool isMachineActive = false;
	[Range(2, 20)]
	public float newBlockTime = 2f;
	private float closeDoorTime;
	private float openDoorTime;
	private float currentTimeLeft;
	private bool isDoorOpen;
	Vector3 throwForce;
	Vector3 DoorStartpos;
	bool newBlockNeeded = true;
    public bool firstBlock = true;
	#endregion
	// Start is called before the first frame update
	void Start() {
		closeDoorTime = newBlockTime + 1 / doorVecolicity;
		openDoorTime = newBlockTime - 1 / doorVecolicity;
		DoorStartpos = machineDoor.position;
		throwForce = -transform.forward * throwBlockForce;
		ActivateMachine();
	}

	void ActivateMachine() {
		isMachineActive = true;
        if(firstBlock) StartCoroutine(firstBlockCoroutine());
	}

	void OpenMachineDoor() {
		isDoorOpen = true;
	}

	void CloseMachineDoor() {
		isDoorOpen = false;
	}

	void CreateNewCube() {
		if (isDoorOpen) {

			instance = Instantiate(block, cubeQueue.position, cubeQueue.rotation, cubeQueue);
            instance.GetComponent<TemperatureBlock>().melting_speed = block_melting_speed;
            instance.GetComponent<TemperatureBlock>().melting_duration = block_melting_duration;
            instance.GetComponent<Rigidbody>().AddForce(throwForce);
			instance.GetComponent<BoxCollider>().isTrigger = true;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.GetComponent<TemperatureBlock>()) {
			other.GetComponent<BoxCollider>().isTrigger = false;
		}
	}


	private void FixedUpdate() {
		if (isMachineActive && newBlockNeeded) {
			StartCoroutine(newBlockCoroutine());
		}

	}

    

    private void Update() {

		if (isDoorOpen)
			machineDoor.position = Vector3.Lerp(machineDoor.position, DoorStartpos + doorHightDifference, Time.deltaTime * doorVecolicity);
		else
			machineDoor.position = Vector3.Lerp(machineDoor.position, DoorStartpos, Time.deltaTime * doorVecolicity);
	}

    IEnumerator newBlockCoroutine()
    {
        newBlockNeeded = false;

        yield return new WaitForSeconds(openDoorTime);
        OpenMachineDoor();
        yield return new WaitForSeconds(newBlockTime - openDoorTime);
        CreateNewCube();
        yield return new WaitForSeconds(closeDoorTime - newBlockTime);
        CloseMachineDoor();
        newBlockNeeded = true;
    }
    IEnumerator firstBlockCoroutine()
    {
        newBlockNeeded = false;
        OpenMachineDoor();
        yield return new WaitForSeconds(1f);
        CreateNewCube();
        yield return new WaitForSeconds(1f);
        CloseMachineDoor();
        newBlockNeeded = true;
    }
}
