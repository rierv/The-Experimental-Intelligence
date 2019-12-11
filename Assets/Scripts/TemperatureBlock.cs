using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureBlock : MonoBehaviour {
    public float melting_speed=0.1f;
    public float melting_duration=20f;
    private ParticleSystem dissolveParticle;
    // Start is called before the first frame update

    private void Awake()
    {
        dissolveParticle = GetComponent<ParticleSystem>();
    }
    private void Start()
    {
        StartCoroutine(destroyBlock());
    }
    // Update is called once per frame
    void Update() {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * melting_speed);
        
    }
    IEnumerator destroyBlock()
    {
        
        yield return new WaitForSeconds(melting_duration/2+melting_duration/4);
        if (enabled) dissolveParticle.Play();
        yield return new WaitForSeconds(melting_duration/2- melting_duration / 4);
        if (enabled) melting_speed *= 10;
        yield return new WaitForSeconds(3f);
        if(enabled) Destroy(this.transform.parent.parent.gameObject);
        else dissolveParticle.Stop();
    }
}
