using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirFan : MonoBehaviour {
	public Transform fan;
	public float fanSpeed;
	public float force = 4;
	public float splashForce = 0.85f;
	float surface;

	void Awake() {
		surface = transform.localScale.y;
	}

	void Update() {
		fan.Rotate(transform.up, fanSpeed * Time.deltaTime);
	}

	private void OnTriggerEnter(Collider other) {
        /*
         * per distinguere tra core e ossa ci vorrebbero 4 parentesi in più, però forse è più carino l'effetto se mandiamo su solo il cose e lasciamo le ossa a inseguirlo
         * era così
         * (other.GetComponent<StateManager>() && other.GetComponent<StateManager>().state == FlapperState.jelly || other.GetComponent<JellyBone>() && other.GetComponent<JellyBone>().state == FlapperState.jelly)
         * per distinguere tra core e ossa a livello booleano lo farei così
         * ((other.GetComponent<StateManager>() && other.GetComponent<StateManager>().state == FlapperState.jelly) || (other.GetComponent<JellyBone>() && other.GetComponent<JellyBone>().state == FlapperState.jelly))
         ma alla fine proverei così per usare anche i pushable e far levitare solo il core*/

        if ((other.isTrigger != true && (other.gameObject.layer == 13 || other.gameObject.layer == 12) && other.GetComponentInChildren<PushableHeavy>() == null) || (other.GetComponent<StateManager>() && other.GetComponent<StateManager>().state == FlapperState.jelly))
        {
            Rigidbody r = other.GetComponent<Rigidbody>();
			r.useGravity = false;
			r.AddForce(transform.up * -r.velocity.y * splashForce, ForceMode.VelocityChange);
		}
        

    }

    private void OnTriggerStay(Collider other) {
        if ((other.isTrigger != true && (other.gameObject.layer == 13 || other.gameObject.layer == 12) && other.GetComponentInChildren<PushableHeavy>() == null) || (other.GetComponent<StateManager>() && other.GetComponent<StateManager>().state == FlapperState.jelly))
        {
            other.GetComponent<Rigidbody>().AddForce(transform.up * (transform.position.y + surface - other.transform.position.y) * force, ForceMode.Acceleration);
		}
	}

	private void OnTriggerExit(Collider other) {
        if ((other.isTrigger != true &&(other.gameObject.layer == 13|| other.gameObject.layer == 12) && other.GetComponentInChildren<PushableHeavy>() == null) || (other.GetComponent<StateManager>() && other.GetComponent<StateManager>().state == FlapperState.jelly))
        {
            other.GetComponent<Rigidbody>().useGravity = true;
		}
	}
}
