using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirFan : MonoBehaviour, I_Activable {
	public Transform fan;
	public float fanSpeed;
	public float force = 4;
	public float splashForce = 0.85f;
	public float hight = 1;
    bool activable = true;

    float surface;
	public bool active = true;
	ParticleSystem PS;
	void Awake() {
		if (!active) {
			GetComponentInChildren<ParticleSystem>().Stop();
		}
	}
	private void Start() {
		PS = GetComponentInChildren<ParticleSystem>();
		PS.startLifetime *= hight * .9f;
		BoxCollider collider = GetComponent<BoxCollider>();
		collider.size = new Vector3(collider.size.x, hight, collider.size.z);
		surface = transform.localScale.y * collider.size.y;

	}
	void Update() {
		if (active) fan.Rotate(fan.transform.up, fanSpeed * Time.deltaTime);
	}

	private void OnTriggerEnter(Collider other) {
		/*
         * per distinguere tra core e ossa ci vorrebbero 4 parentesi in più, però forse è più carino l'effetto se mandiamo su solo il core e lasciamo le ossa a inseguirlo
         * era così
         * (other.GetComponent<StateManager>() && other.GetComponent<StateManager>().state == FlapperState.jelly || other.GetComponent<JellyBone>() && other.GetComponent<JellyBone>().state == FlapperState.jelly)
         * per distinguere tra core e ossa a livello booleano lo farei così
         * ((other.GetComponent<StateManager>() && other.GetComponent<StateManager>().state == FlapperState.jelly) || (other.GetComponent<JellyBone>() && other.GetComponent<JellyBone>().state == FlapperState.jelly))
         ma alla fine proverei così per usare anche i pushable e far levitare solo il core: */

		if (active && CheckOther(other)) {
			Rigidbody r = other.GetComponent<Rigidbody>();
			r.useGravity = false;
			r.AddForce(transform.up * -r.velocity.y * splashForce, ForceMode.VelocityChange);
		}
	}

	private void OnTriggerStay(Collider other) {
		if (active && CheckOther(other)) {
			other.GetComponent<Rigidbody>().AddForce(transform.up * (transform.position.y + surface - other.transform.position.y) * force, ForceMode.Acceleration);
		}
	}

	private void OnTriggerExit(Collider other) {
		if (CheckOther(other)) {
			other.GetComponent<Rigidbody>().useGravity = true;
		}
	}

	bool CheckOther(Collider other) {
		return (other.isTrigger != true &&
				(other.gameObject.layer == 13 || (other.gameObject.layer == 12 && !other.GetComponent<ThrowableObject>().enabled)) &&
				(!other.GetComponentInChildren<Pushable>() || !other.GetComponentInChildren<Pushable>().heavy)) ||
			(other.GetComponent<StateManager>() && other.GetComponent<StateManager>().state != FlapperState.solid);
	}

	public void Activate(bool type = true) {
        if (activable)
        {
            active = true;
            GetComponentInChildren<ParticleSystem>().Play();
        }
	}

	public void Deactivate() {
		active = false;
		GetComponentInChildren<ParticleSystem>().Stop();
	}

    public void canActivate(bool enabled)
    {
        activable = enabled;
    }
}
