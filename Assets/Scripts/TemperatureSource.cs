using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureSource : MonoBehaviour, I_Activable {
	[Range(-1, 1)]
	public int variation;
    public bool active=false;
    MeshRenderer myColor;
    public Material activeMaterial;
    public Material nonActiveMaterial;

    private void Start()
    {
        myColor = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        if (!active) Deactivate();
    }
    public void Activate()
    {
        active=true;
        myColor.material = activeMaterial;
    }
    public void Deactivate()
    {
        active=false;
        myColor.material = nonActiveMaterial;

    }

    private void OnTriggerEnter(Collider other) {
		JellyBone jellyBone = other.GetComponent<JellyBone>();
		if (jellyBone&&active) {
			jellyBone.GetComponentInParent<FlapperCore>().GetComponentInChildren<StateManager>().temperature = variation;
		}
	}
}
