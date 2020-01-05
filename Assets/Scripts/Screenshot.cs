using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Screenshot : MonoBehaviour {
#if UNITY_EDITOR
	void Update() {
		if (Input.GetKeyDown(KeyCode.Keypad0)) {
			Time.timeScale = 0;
			transform.GetChild(0).gameObject.SetActive(false);
			ScreenCapture.CaptureScreenshot("Assets\\Resources\\Levels\\" + SceneManager.GetActiveScene().name + ".png");
			Debug.Log("Screenshot Assets\\Resources\\Levels\\" + SceneManager.GetActiveScene().name + ".png");
		} else if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		} else if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
		}
	}
#endif
}
