using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingAR : MonoBehaviour
{
    float timer = 0;
    public Image progressBar;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadAsyncLevel());
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }
    IEnumerator LoadAsyncLevel()
    {
        AsyncOperation loadLevel = SceneManager.LoadSceneAsync(2);
        loadLevel.allowSceneActivation = false;
        while (loadLevel.progress < .9f || timer < 2.5f)
        {
            progressBar.fillAmount = Mathf.Lerp(0, 1, timer/2.5f);
            yield return new WaitForEndOfFrame();
        }
        loadLevel.allowSceneActivation = true;
    }
}
