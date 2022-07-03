using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public static bool isGamePaused = false;
	public GameObject pauseMenuUI;
    public GameObject UI;
	public Button resumeButton;
	public Button restartButton;
	public Button exitButton;
	public SpriteRenderer profSprite;
	Sprite profSpriteDefault;
    PauseTouchScript Pause_Trigger;

	public ImageTracking IT;
	public MarkerlessLevelPositioning MLP;
    void Start() {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		profSpriteDefault = profSprite.sprite;
        Pause_Trigger = GameObject.Find("Pause_Button").GetComponent<PauseTouchScript>();
    }

	void Update() {
		if ((Pause_Trigger.pauseButtonHold && !isGamePaused)||Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7")) {
			if (isGamePaused)
				Resume();
			else
				Pause();
		}

		/*if (Input.GetKeyDown(KeyCode.Escape) && isGamePaused)
            Restart();
        if (Input.GetKeyDown(KeyCode.F1) && isGamePaused)
            StageSelect();*/

	}

	public void Pause() {
        UI.SetActive(false);
		pauseMenuUI.SetActive(true);
        Pause_Trigger.pauseButtonHold = false;

        Time.timeScale = 0f;
		isGamePaused = true;
		EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
		resumeButton.OnSelect(null);
		resumeButton.onClick.AddListener(Resume);
		restartButton.onClick.AddListener(Restart);
		exitButton.onClick.AddListener(StageSelect);
	}

	public void Resume() {
        UI.SetActive(true);
        pauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
		isGamePaused = false;
	}

	public void Restart() {
		
		pauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
		isGamePaused = false;
		if (IT) IT.Reset();
		else if (MLP) MLP.Reset();
		UI.SetActive(true);
		//SceneManager.LoadScene(SceneManager.GetActiveScene().name);

	}

    public void StageSelect() {
		SceneManager.LoadScene(0); // Start
		Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void ResetProfSprite() {
		profSprite.sprite = profSpriteDefault;
	}
}
