using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyCore : MonoBehaviour {
	public static float cohesion;
	public float _cohesion = 10;

	public static float drag;
	public float _drag = 1;

	/*public static float maxFallingForce;
	public float _maxFallingForce = 10;*/

	public static float gaseousAntiGravity;
	public float _gaseousAntiGravity = 0.4f;

	public static float gaseousDrag;
	public float _gaseousDrag = 0.8f;

	void Awake() {
		OnValidate();
	}

	void OnValidate() {
		cohesion = _cohesion;
		drag = _drag;
		//maxFallingForce = _maxFallingForce;
		gaseousAntiGravity = _gaseousAntiGravity;
		gaseousDrag = _gaseousDrag;
	}
}
