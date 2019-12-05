using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene("Start");
        if (Input.GetKeyDown(KeyCode.F1)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
