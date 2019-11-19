using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureBlock : MonoBehaviour
{
    public float duration;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime / duration);
        if (transform.localScale.magnitude < 0.1f) Destroy(this.gameObject);
    }
}
