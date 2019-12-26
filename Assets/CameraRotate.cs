using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
	public Vector3 rotation;
	public bool resetRotation;
	
    public void OnTriggerEnter(Collider other) {
		if(other.GetComponent<JellyCore>()) {
			if(resetRotation) {
				FindObjectOfType<FollowPlayer>().ResetRotation();
			} else {
				FindObjectOfType<FollowPlayer>().targetRotation = Quaternion.Euler(rotation);
			}
		}
	}
}
