using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperGravity : MonoBehaviour {
	public float fallingBoost = 60;
	Rigidbody rigidbody;
    PlayerMove player;
    bool force = false;
    public bool forceUp = true;
	void Awake() {
		rigidbody = GetComponent<Rigidbody>();
        player = GetComponent<PlayerMove>();

    }

    void Update() {
		if (forceUp&& player.moveType==MoveType.Rigidbody && rigidbody.useGravity && rigidbody.velocity.y > -.1f && force) {
			rigidbody.AddForce(Vector3.up*1.5f * fallingBoost, ForceMode.Acceleration);
		}
	}
    private void OnCollisionEnter(Collision collision)
    {
        force = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        force = false;
    }
}
