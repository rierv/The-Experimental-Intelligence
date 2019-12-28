using UnityEngine;

public class FPSDisplay : MonoBehaviour {
	float deltaTime = 0.0f;
	bool showFramerate;

	void Update() {
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
		if (Input.GetKeyDown(KeyCode.F)) {
			showFramerate = !showFramerate;
		}
	}

	void OnGUI() {
		if (showFramerate) {
			int w = Screen.width, h = Screen.height;

			GUIStyle style = new GUIStyle();

			Rect rect = new Rect(5, 5, w, h * 2 / 100);
			style.alignment = TextAnchor.UpperLeft;
			style.fontSize = h * 2 / 100;
			style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
			float msec = deltaTime * 1000.0f;
			float fps = 1.0f / deltaTime;
			string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
			GUI.Label(rect, text, style);
		}
	}
}
