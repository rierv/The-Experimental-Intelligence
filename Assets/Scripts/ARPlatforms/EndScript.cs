using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScript : MonoBehaviour
{
    public GameObject ps;
    AudioSource winAudio;
    bool endStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        winAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "CORE" && !endStarted)
        {
            endStarted = true;
            winAudio.Play();
            ps.SetActive(true);
            StartCoroutine(LoadStartScene());
        }
    }
    /*private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "CORE")
        {
            ps.SetActive(false);
            //StartCoroutine(LoadStartScene());
        }
    }*/
    IEnumerator LoadStartScene()
    {
        yield return new WaitForSeconds(5);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
