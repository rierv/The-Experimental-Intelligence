using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComplete : MonoBehaviour
{

    public GameObject [] Stars;
    RectTransform [] StarsRT = new RectTransform [3];
    int starCount = 0;
    float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        starCount = PlayerPrefs.GetInt("CurrentStars");
        for (int i = 0; i< starCount; i++)
        {
            Stars[i].SetActive(true);
            StarsRT[i] = Stars[i].GetComponent<RectTransform>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 2)
        {
            timer += Time.deltaTime;
            for (int i = 0; i < starCount; i++)
            {
                StarsRT[i].localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timer/2);
            }
        }
    }
    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(4);

    }

    public void NextLevel()
    {
        LevelSelectionManager.currentLevel += 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(4);
    }

    public void StageSelect()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);

    }
}
