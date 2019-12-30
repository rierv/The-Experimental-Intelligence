using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyCore : MonoBehaviour {
	public static float cohesion;
	public float _cohesion = 10;

	public static float drag;
	public float _drag = 1;

	/*public static float gaseousAntiGravity;
	public float _gaseousAntiGravity = 0.4f;*/

	public static float gaseousDrag;
	public float _gaseousDrag = 0.8f;

	public static float minDistance;
	[Space]
	public float _minDistance = -0.5f;
	public static float maxDistance;
	public float _maxDistance = 0.5f;

	public static float minShift;
	public float _minShift = -0.5f;
	public static float maxShift;
	public float _maxShift = 0.5f;

	void Awake() {
		OnValidate();
	}

	void OnValidate() {
		cohesion = _cohesion;
		drag = _drag;
		//gaseousAntiGravity = _gaseousAntiGravity;
		gaseousDrag = _gaseousDrag;

		minDistance = _minDistance;
		maxDistance = _maxDistance;
		minShift = _minShift;
		maxShift = _maxShift;
	}
}
