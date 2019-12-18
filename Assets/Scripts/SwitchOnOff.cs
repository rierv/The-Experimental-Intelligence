using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOnOff : MonoBehaviour {
	public Transform pushableBlock;
	public Collider solidOnlyCollider;
	public float speed;
	public bool solidOnly;
	public GameObject[] triggeredObjects;

	List<I_Activable> activables = new List<I_Activable>();
	float pushableBlockY;
	float targetX;

	void Awake() {
		pushableBlockY = pushableBlock.localPosition.y;
		targetX = pushableBlock.localPosition.x;
		solidOnlyCollider.enabled = solidOnly;
	}

	void Start() {
		foreach (GameObject go in triggeredObjects) {
			activables.Add(go.GetComponent<I_Activable>());
		}
		if (pushableBlock.localPosition.x > 0) {
			foreach (I_Activable ac in activables) {
				ac.Activate();
			}
		} else {
			foreach (I_Activable ac in activables) {
				ac.Deactivate();
			}
		}
	}

	void Update() {
		if (pushableBlock.localPosition.x != targetX) {
			pushableBlock.localPosition = Vector3.Lerp(pushableBlock.localPosition, new Vector3(targetX, pushableBlockY, 0), speed * Time.deltaTime);
		}
		pushableBlock.localPosition = new Vector3(Mathf.Clamp(pushableBlock.localPosition.x, -0.5f, 0.5f), pushableBlockY, 0);
	}

	void OnTriggerEnter(Collider other) {
		if (!other.isTrigger && other.transform == pushableBlock) {
			if (pushableBlock.localPosition.x < 0) {
				targetX = 0.5f;
			} else {
				targetX = -0.5f;
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (!other.isTrigger && other.transform == pushableBlock) {
			if (pushableBlock.localPosition.x > 0) {
				foreach (I_Activable ac in activables) {
					ac.Activate();
				}
			} else {
				foreach (I_Activable ac in activables) {
					ac.Deactivate();
				}
			}
		}
	}
}
