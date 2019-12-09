using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject pauseMenuUI;
    public Button resumeButton;
    public Button restartButton;
    public Button exitButton;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)|| Input.GetKeyDown("joystick button 7"))
        {
            if (isGamePaused)
                Resume();
            else
                Pause();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isGamePaused)
            Restart();
        if (Input.GetKeyDown(KeyCode.F1) && isGamePaused)
            StageSelect();

    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
        EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
        resumeButton.OnSelect(null);
        resumeButton.onClick.AddListener(Resume);
        restartButton.onClick.AddListener(Restart);
        exitButton.onClick.AddListener(StageSelect);
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void StageSelect()
    {
        SceneManager.LoadScene("Start");
        Time.timeScale = 1f;
    }

}
