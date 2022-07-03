using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour
{

    // Start is called before the first frame update
    public GameObject content;

    public static int currentLevel;
    void Start()
    {
        int levels;
        levels = PlayerPrefs.GetInt("Unlocked Levels", 0);
        for(int i=0; i< levels+1; i++)
        {
            int type= PlayerPrefs.GetInt("Level" + i + "State", 2);
            content.transform.GetChild(i).GetChild(1).GetChild(type).gameObject.SetActive(true);
            if (type == 0)
            {
                for (int j = 0; j < PlayerPrefs.GetInt("Level" + i + "Stars"); j++)
                {
                    content.transform.GetChild(i).GetChild(1).GetChild(type).GetChild(1).GetChild(j).gameObject.SetActive(true);
                }
            }
        }
        for(int i = levels; i<content.transform.childCount; i++)
        {
            content.transform.GetChild(i).GetChild(1).GetChild(1).gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadLevel(int level)
    {
        currentLevel = level;
        SceneManager.LoadScene(4);
    }
}
