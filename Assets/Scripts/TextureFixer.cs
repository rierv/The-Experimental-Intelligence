using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureFixer : MonoBehaviour {
	public float baseScale = 10;
	public Mesh oldCube;
	public Mesh newCube;
	public Material[] materialsToFix = new Material[0];

	void Start() {
		List<Material> list = new List<Material>();
		list.AddRange(materialsToFix);
		foreach (MeshRenderer meshRenderer in FindObjectsOfType<MeshRenderer>()) {
			if (list.Contains(meshRenderer.sharedMaterial)) {
				if (meshRenderer.GetComponent<MeshFilter>().sharedMesh == oldCube) {
					meshRenderer.GetComponent<MeshFilter>().mesh = newCube;
					Material m = meshRenderer.sharedMaterial;
					meshRenderer.materials = new Material[] { m, m, m, m, m, m };
				}
				for (int i = 0; i < meshRenderer.materials.Length; i++) {
					float x = 1, y = 1;
					switch (i) {
						case 0: // front
							x = meshRenderer.gameObject.transform.localScale.y;
							y = meshRenderer.gameObject.transform.localScale.x;
							break;
						case 1: // back
							x = meshRenderer.gameObject.transform.localScale.x;
							y = meshRenderer.gameObject.transform.localScale.y;
							break;
						case 2: // left
							x = meshRenderer.gameObject.transform.localScale.z;
							y = meshRenderer.gameObject.transform.localScale.y;
							break;
						case 3: // down
							x = meshRenderer.gameObject.transform.localScale.z;
							y = meshRenderer.gameObject.transform.localScale.x;
							break;
						case 4: // right
							x = meshRenderer.gameObject.transform.localScale.z;
							y = meshRenderer.gameObject.transform.localScale.y;
							break;
						case 5: // up
							x = meshRenderer.gameObject.transform.localScale.z;
							y = meshRenderer.gameObject.transform.localScale.x;
							break;
					}
					//float scale = Mathf.Min(x, y);
					meshRenderer.materials[i].SetTextureScale("_BaseMap", new Vector2(Mathf.Clamp((int)(baseScale * x), 1, 100), Mathf.Clamp((int)(baseScale * y), 1, 100)));
				}
			}
		}
	}
}
