using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour, I_Activable {
	public NextLevel teleport;
	public List<Object> levels;
	bool active = false;
	float counter = 0;
	int currentLevel = 0;

	TMPro.TextMeshProUGUI levelName;
	public void Activate(bool type = true) {
		active = true;
	}

	public void canActivate(bool enabled) {
		throw new System.NotImplementedException();
	}

	public void Deactivate() {
		active = false;
	}
	private void Awake() {
		levelName = GetComponent<TMPro.TextMeshProUGUI>();
		setLevel();
	}

	void Update() {
		if (active) {
			counter += Time.deltaTime * Input.GetAxis("Horizontal");
			if (counter > 1 && currentLevel == levels.Count - 1) {
				currentLevel = 0;
				setLevel();
			} else if (counter < -1 && currentLevel == 0) {
				currentLevel = levels.Count - 1;
				setLevel();
			}

			if (counter > 1) {
				currentLevel++;
				setLevel();

			} else if (counter < -1) {
				currentLevel--;
				setLevel();
			}
		}
	}

	void setLevel() {
		teleport.nextLevel = levels[currentLevel];
		levelName.text = "Play level " + (currentLevel + 1) + ":\n" + teleport.nextLevel.name + "?";
		counter = 0;
	}
}
