using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureSource : MonoBehaviour, I_Activable {
	[Range(-1, 1)]
	public int variation;
	MeshRenderer myColor;
	public Material activeMaterial;
	public Material nonActiveMaterial;
	[Header("Activable")]
	public bool active = true;
	public bool invertTrueFalse;

    public GameObject illuminationSource = null;
    private List<Light> illumination=null;
	private void Start() {
		myColor = transform.GetComponentInChildren<MeshRenderer>();
		if (!active) Deactivate();
        if (illuminationSource)
        {
            illumination = new List<Light>();
            foreach (Light l in illuminationSource.GetComponentsInChildren<Light>()) illumination.Add(l);
        }
	}
    private void Update()
    {
        if (illumination!=null)
        {
            if (active)
                foreach (Light l in illumination) l.intensity = Mathf.Lerp(l.intensity, 20, Time.deltaTime * 2);
            
            else
                foreach (Light l in illumination) l.intensity = Mathf.Lerp(l.intensity, 0, Time.deltaTime * 2);


        }
    
    }
    public void Activate(bool type) {
		if (type && !invertTrueFalse || !type && invertTrueFalse) {
			active = true;
			myColor.material = activeMaterial;
		} else {
			Deactivate();
		}
	}

	public void Deactivate() {
		active = false;
		myColor.material = nonActiveMaterial;

	}

	private void OnTriggerStay(Collider other) {
		if ((other.gameObject.tag == "Bone" || other.gameObject.tag == "Player") && active) {
			other.GetComponentInParent<FlapperCore>().GetComponentInChildren<StateManager>().temperature = variation;
		}
	}
}
