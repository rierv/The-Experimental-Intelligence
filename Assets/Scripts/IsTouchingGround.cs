using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTouchingGround : MonoBehaviour {
	PlayerMove playerMove;
    GameObject Flapper;
	int colliderCount = 0;

	void Awake() {
        playerMove = GetComponentInParent<PlayerMove>();
        Flapper = GetComponentInParent<FlapperCore>().gameObject;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, Flapper.transform.localPosition + playerMove.transform.localPosition -Vector3.up, Time.deltaTime * 2);
    }
    private void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer != gameObject.layer) {
			colliderCount++;
			playerMove.canJump = colliderCount > 0;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.layer != gameObject.layer) {
			colliderCount--;
			playerMove.canJump = colliderCount > 0;
		}
	}
}
