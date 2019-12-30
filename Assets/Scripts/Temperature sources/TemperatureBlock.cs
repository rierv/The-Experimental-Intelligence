using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureBlock : MonoBehaviour {
    public float melting_speed=0.1f;
    public float melting_duration=20f;
    private ParticleSystem dissolveParticle;
    bool startEnd = false;
    // Start is called before the first frame update

    private void Awake()
    {
        dissolveParticle = GetComponent<ParticleSystem>();
    }
    private void Start()
    {
        StartCoroutine(BlockLife());
    }
    // Update is called once per frame
    void Update() {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * melting_speed);
        if(startEnd) transform.parent.localScale = Vector3.Lerp(transform.parent.localScale, Vector3.zero, Time.deltaTime * melting_speed);
    }
    IEnumerator BlockLife()
    {
        
        yield return new WaitForSeconds(melting_duration*1.5f);
        if (enabled) dissolveParticle.Play();
        yield return new WaitForSeconds(melting_duration*.5f);
        if (enabled) melting_speed *= 5;
        StartCoroutine(Destroyblock());
        yield return new WaitForSeconds(1);
        if (!enabled) dissolveParticle.Stop();
    }
    public void fadeOut()
    {
        StartCoroutine(Destroyblock());
    }
    IEnumerator Destroyblock()
    {
        startEnd = true;
        melting_speed *= 30;
        yield return new WaitForSeconds(1);
        if (enabled) Destroy(this.transform.parent.parent.gameObject);
    }
}
