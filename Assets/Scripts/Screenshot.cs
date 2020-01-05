using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Screenshot : MonoBehaviour {
	void Update() {
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Keypad0)) {
			Time.timeScale = 0;
			transform.GetChild(0).gameObject.SetActive(false);
			ScreenCapture.CaptureScreenshot("Assets\\Resources\\Levels\\" + SceneManager.GetActiveScene().name + ".png");
			Debug.Log("Screenshot Assets\\Resources\\Levels\\" + SceneManager.GetActiveScene().name + ".png");
		}
#endif
		if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
			int i = SceneManager.GetActiveScene().buildIndex;
			if (i == SceneManager.sceneCountInBuildSettings - 1) {
				i = 0;
			} else {
				i++;
			}
			SceneManager.LoadScene(i);
		} else if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
			int i = SceneManager.GetActiveScene().buildIndex;
			if (i > 0) {
				i--;
				SceneManager.LoadScene(i);
			}
		}
	}
}
