using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparency : MonoBehaviour {
	public float offset = 2;
	public float decrease = 1.5f;
	float baseOpacity;
	Material material;
	Transform flapper;

	void Awake() {
		material = GetComponent<MeshRenderer>().material;
		baseOpacity = material.GetColor("_BaseColor").a;
		flapper = FindObjectOfType<JellyCore>().transform;
	}

	void Update() {
		float opacity = Mathf.Clamp(-(flapper.position.z - transform.position.z) / decrease + offset, 0, 1) * baseOpacity;
		Debug.Log(opacity);
		material.SetColor("_BaseColor", new Color(1, 1, 1, opacity));
	}
}
