using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyCore : MonoBehaviour {
	public static float cohesion = 10;
	public float _cohesion = 10;

	public static float drag = 1;
	public float _drag = 1;

	public static float maxFallingForce = 10;
	public float _maxFallingForce = 10;

	void Start() {
		OnValidate();
	}

	void OnValidate() {
		cohesion = _cohesion;
		drag = _drag;
		maxFallingForce = _maxFallingForce;
	}
}
