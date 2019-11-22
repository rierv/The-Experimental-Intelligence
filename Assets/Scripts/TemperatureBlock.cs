using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureBlock : MonoBehaviour {
    public float duration;
    public BoxCollider collider;
    private ParticleSystem dissolveParticle;

    // Start is called before the first frame update

    private void Start()
    {
        dissolveParticle = GetComponent<ParticleSystem>();
    }
    // Update is called once per frame
    void Update() {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime / duration);
        if (transform.localScale.magnitude < 0.8f) {
                dissolveParticle.Play();
                Destroy(this.gameObject);
        }
	}
}
