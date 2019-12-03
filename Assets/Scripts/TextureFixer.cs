using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureFixer : MonoBehaviour {
	public float baseScale = 10;
	public Material[] materialsToFix = new Material[0];

	void Start() {
		List<Material> list = new List<Material>();
		list.AddRange(materialsToFix);
		foreach (MeshRenderer meshRenderer in FindObjectsOfType<MeshRenderer>()) {
			if (list.Contains(meshRenderer.sharedMaterial)) {
				meshRenderer.material.SetTextureScale("_BaseMap", new Vector2(baseScale * meshRenderer.gameObject.transform.localScale.x, baseScale * meshRenderer.gameObject.transform.localScale.z));
			}
		}
	}
}
