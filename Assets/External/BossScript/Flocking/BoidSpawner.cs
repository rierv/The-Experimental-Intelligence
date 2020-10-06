using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour {

	public float size = 10f;
    public float radius = 10;
    public int count = 100;
	public GameObject boid = null;
    BoidAlign[] boidList;
	void Start () {
        boidList = new BoidAlign[count];
		if (boid != null) {
			for (int i = 0; i < count; i += 1) {
				GameObject go = Instantiate (boid, transform.position + Random.insideUnitSphere * size, transform.rotation);
                boidList[i] = go.GetComponent<BoidAlign>();
                go.transform.parent = transform;
                go.GetComponent<BoidAlign>().radius = radius;
				go.transform.LookAt (transform.position + Random.insideUnitSphere * size);
				go.name = boid.name + " " + i;
			}
		}
	}

    public void UpdateRadius(float newRadius)
    {
        foreach(BoidAlign g in boidList)
        {
            g.radius = newRadius/2;
        }
    }
}
