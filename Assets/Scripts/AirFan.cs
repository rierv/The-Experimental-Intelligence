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
    List<Rigidbody> objectsinAir;
	ParticleSystem PS;
    StateManager flapperState;
	void Awake() {
		if (!active) {
			GetComponentInChildren<ParticleSystem>().Stop();
		}
	}
	private void Start() {
        objectsinAir = new List<Rigidbody>();
		PS = GetComponentInChildren<ParticleSystem>();
		PS.startLifetime *= hight * .33f;
        BoxCollider airCollider=GetComponent<BoxCollider>();

        airCollider.size = new Vector3(airCollider.size.x, hight, airCollider.size.z);
        airCollider.center = new Vector3(0, hight / 2, 0);
        //transform.position = new Vector3 (transform.position.x, transform.position.y + (hight/2), transform.position.z);
        surface = transform.localScale.y * airCollider.size.y;
        flapperState = GameObject.Find("CORE").GetComponent<StateManager>();
	}
	void Update() {
        if (active)
        {
            if (flapperState.state == FlapperState.solid) objectsinAir.Remove(flapperState.GetComponent<Rigidbody>());
            fan.Rotate(fan.transform.up, fanSpeed * Time.deltaTime);
            foreach (Rigidbody r in objectsinAir)
            {
                r.useGravity = false;
                r.transform.position = Vector3.Lerp(r.transform.position, new Vector3(r.transform.position.x, transform.position.y + surface, r.transform.position.z), Time.deltaTime * force);
            }
        }
        fan.localPosition = Vector3.zero;
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
            objectsinAir.Add(r);
			//da non pochi problemi:
            //r.AddForce(transform.up * -r.velocity.y * splashForce, ForceMode.VelocityChange);
		}
	}	

	private void OnTriggerExit(Collider other) {
		if ((other.GetComponent<StateManager>() && other.GetComponent<StateManager>().state == FlapperState.solid)||CheckOther(other)) {
            other.GetComponent<Rigidbody>().useGravity = true;
            objectsinAir.Remove(other.GetComponent<Rigidbody>());
		}
	}

	bool CheckOther(Collider other) {
		return other.isTrigger != true &&
				(other.gameObject.layer == 13 || (other.gameObject.layer == 12 && !other.GetComponent<ThrowableObject>().enabled) ||
				(other.GetComponentInChildren<Pushable>() && !other.GetComponentInChildren<Pushable>().heavy) ||
			(other.GetComponent<StateManager>() && other.GetComponent<StateManager>().state != FlapperState.solid));
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
