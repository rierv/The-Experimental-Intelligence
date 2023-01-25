using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineBlockSpawner : MonoBehaviour, I_Activable {
	public IceMachineScript machineScript;

	public void Activate(bool type = true) {
		if (type && machineScript.active) {
			machineScript.StartCoroutine(machineScript.newBlockCoroutine());
		}
	}

	public void canActivate(bool enabled) {
	}

	public void Deactivate() {
	}
}
