using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirFan : MonoBehaviour, I_Activable {
	public Transform fan;
	public float fanSpeed;
	public float horizontalForce = 8;
	public float verticalForce = 6;
	public float splashForce = 0.85f;
	public Vector3 direction = Vector3.up;
	//public float hight = 1;
	bool activable = true;
	float surface;
	public bool active = true;
	AudioSource audioSource;
	List<Rigidbody> objectsinAir;
	ParticleSystem particleSystem;
	StateManager flapperState;
	public Rigidbody specialSpinObject;
	public float specialSpinForce;
	public float specialSpinOffset;
	//bool flapperSolid = false;

	void Awake() {
		audioSource = GetComponent<AudioSource>();
		particleSystem = GetComponentInChildren<ParticleSystem>();
		if (!active) {
			audioSource.Stop();
			particleSystem.Stop();
		}
		if (active) Activate();
	}

	private void Start() {
		objectsinAir = new List<Rigidbody>();
		//PS = GetComponentInChildren<ParticleSystem>();
		//PS.startLifetime *= hight * .33f;
		BoxCollider airCollider = GetComponent<BoxCollider>();

		//airCollider.size = new Vector3(airCollider.size.x, hight, airCollider.size.z);
		//airCollider.center = new Vector3(0, hight / 2, 0);
		//transform.position = new Vector3 (transform.position.x, transform.position.y + (hight/2), transform.position.z);
		surface = transform.localScale.y * airCollider.size.y;
		flapperState = GameObject.Find("CORE").GetComponent<StateManager>();

	}

	void Update() {
		if (active) {
			if (flapperState.state == FlapperState.solid && objectsinAir.Contains(flapperState.GetComponent<Rigidbody>())) {
				objectsinAir.Remove(flapperState.GetComponent<Rigidbody>());
				//flapperSolid = true;
				flapperState.GetComponent<Rigidbody>().useGravity = true;
			}

			fan.Rotate(Vector3.up, fanSpeed * Time.deltaTime);
			foreach (Rigidbody r in objectsinAir) {
				if (r) {
					if (direction == Vector3.up) {
						r.useGravity = false;
					}
					if (specialSpinObject && specialSpinObject == r) {
						// non ho modificato questo pezzo, ma solo quello sotto (Davide)
						r.transform.position = Vector3.Lerp(r.transform.position, new Vector3(r.transform.position.x + (surface + specialSpinOffset) * direction.x, transform.position.y + (surface + specialSpinOffset) * direction.y, r.transform.position.z + (surface + specialSpinOffset) * direction.z), Time.deltaTime * specialSpinForce);
					} else {
						r.transform.position = Vector3.Lerp(r.transform.position, new Vector3(r.transform.position.x + horizontalForce * direction.x, r.transform.position.y + verticalForce * direction.y, r.transform.position.z + horizontalForce * direction.z), Time.deltaTime);
					}
				}
			}
		} else {
			foreach (Rigidbody o in objectsinAir) {
				if (o) {
					o.useGravity = true;
					//objectsinAir.Remove(o);
				}
			}
		}
		//fan.localPosition = Vector3.zero;
	}

	private void OnTriggerEnter(Collider other) {
		/*
         * per distinguere tra core e ossa ci vorrebbero 4 parentesi in più, però forse è più carino l'effetto se mandiamo su solo il core e lasciamo le ossa a inseguirlo
         * era così
         * (other.GetComponent<StateManager>() && other.GetComponent<StateManager>().state == FlapperState.jelly || other.GetComponent<JellyBone>() && other.GetComponent<JellyBone>().state == FlapperState.jelly)
         * per distinguere tra core e ossa a livello booleano lo farei così
         * ((other.GetComponent<StateManager>() && other.GetComponent<StateManager>().state == FlapperState.jelly) || (other.GetComponent<JellyBone>() && other.GetComponent<JellyBone>().state == FlapperState.jelly))
         ma alla fine proverei così per usare anche i pushable e far levitare solo il core: */

		if (CheckOther(other)) {
			Debug.Log("enter " + other.gameObject.name);

			Rigidbody r = other.GetComponent<Rigidbody>();
			if (!objectsinAir.Contains(r)) objectsinAir.Add(r);
			//da non pochi problemi:
			/*if (direction == Vector3.up) {
				r.AddForce(transform.up * -r.velocity.y * splashForce, ForceMode.VelocityChange);
			}*/
		}
	}
	private void OnTriggerStay(Collider other) {
		if (other.GetComponent<StateManager>() && other.GetComponent<StateManager>().state == FlapperState.jelly /*&& flapperSolid == true*/) {
			Debug.Log("inStay");
			if (!objectsinAir.Contains(other.GetComponent<Rigidbody>()))
				objectsinAir.Add(other.GetComponent<Rigidbody>());
			//flapperSolid = false;
		}
	}
	private void OnTriggerExit(Collider other) {
		if (CheckOther(other)) {
			Debug.Log("exit " + other.gameObject.name);
			other.GetComponent<Rigidbody>().useGravity = true;
			objectsinAir.Remove(other.GetComponent<Rigidbody>());
			//if (other.GetComponent<StateManager>()) flapperSolid = false;
		}
	}

	bool CheckOther(Collider other) {
		return other.isTrigger != true &&
				(other.gameObject.layer == 13 || (other.gameObject.layer == 12 && !other.GetComponent<ThrowableObject>().enabled) ||
				(other.GetComponentInChildren<Pushable>() && !other.GetComponentInChildren<Pushable>().solidOnly) ||
			other.GetComponent<StateManager>());
	}

	public void Activate(bool type = true) {
		if (activable) {
			active = true;
			audioSource.Play();
			particleSystem.Play();
		}
	}

	public void Deactivate() {
		active = false;
		audioSource.Stop();
		particleSystem.Stop();
	}

	public void canActivate(bool enabled) {
		activable = enabled;
	}
}
