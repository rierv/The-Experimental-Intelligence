using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour, I_Activable {
	public NextLevel teleport;
	//public List<Object> levels;
	public float selectionSpeed = 2;
	public Image spriteRenderer;
    public VJHandler jsMovement;

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
		//WriteLevels( levels, "Assets/levelsNames.txt");

	}
    void Start()
    {
        jsMovement = GameObject.Find("Joycon_container").GetComponent<VJHandler>();

    }
    void Update() {
		if (active) {
			counter += Time.deltaTime * jsMovement.InputDirection.x * selectionSpeed;
			if (counter > 1 && currentLevel == Levels.names.Length - 1) {
				currentLevel = 0;
				setLevel();
			} else if (counter < -1 && currentLevel == 0) {
				currentLevel = Levels.names.Length - 1;
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
	/*static void WriteLevels(List<Object> levels, string path) {
		string text = "";
		int i = 0;
		foreach (Object l in levels) {
			i++;
			text = text + "level " + i + ": " + l.name + "\n";
		}
		//Write some text to the test.txt file
		StreamWriter writer = new StreamWriter(path, true);
		writer.WriteLine(text);
		writer.Close();

	}*/

	void setLevel() {
		teleport.nextLevel = currentLevel;
		levelName.text = "Play level " + (currentLevel + 1) + ":\n" + Levels.names[currentLevel];
		if (spriteRenderer) {
			spriteRenderer.sprite = Resources.Load<Sprite>("Levels/" + Levels.names[currentLevel].Replace("?", ""));
		}
		counter = 0;
	}
}
