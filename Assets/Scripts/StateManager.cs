using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlapperState {
	jelly,
	solid,
	gaseous
}

public class StateManager : MonoBehaviour {
	public FlapperState state;
	JellyBone[] bones;

	void Start() {
		bones = FindObjectsOfType<JellyBone>();
		OnValidate();
	}

	void OnValidate() {
		try {
			SetState(state);
		} catch (System.Exception e) {

		}
	}

	public void SetState(FlapperState newState) {
		state = newState;
		foreach (JellyBone b in bones) {
			b.SetState(state);
		}
	}
}
