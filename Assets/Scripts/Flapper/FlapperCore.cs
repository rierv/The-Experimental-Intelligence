using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlapperCore : MonoBehaviour {

	public Transform ui;
    /*public static string logFile = "levels log.csv";
	private void Start() {
		if (!File.Exists(logFile)) {
			File.WriteAllText(logFile, "Level;Completion time");
		}
		File.AppendAllText(logFile, "\n" + SceneManager.GetActiveScene().name + ";");
	}*/
    float timer = 0;
    public bool deactivating = false;
    private void OnEnable()
    {
        StartCoroutine(DeactivateAfterSeconds(3));
    }
   
    private void Update()
    {
        if (deactivating && Camera.main)
        {
            Quaternion finalRot = Quaternion.LookRotation(Camera.main.transform.up, -Camera.main.transform.forward);
            Quaternion startingRot  = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up); 
            ui.rotation = Quaternion.Lerp(startingRot, finalRot, timer);
            if (timer < 1)
            {
                timer += Time.deltaTime;
            }
            else
            {
                ui.gameObject.SetActive(false);
                this.enabled = false;
            }
        }
        else
        {
            ui.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
        }
    }
    IEnumerator DeactivateAfterSeconds(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        deactivating = true;
    }
}
