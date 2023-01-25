using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    // Start is called before the first frame update
    public static int playType = 0;
    
    public void MarkerGame()
    {
        playType = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void Multiplayer()
    {
        playType = 5;
        //UnityEngine.SceneManagement.SceneManager.LoadScene(5);
    }
    public void LinkToMarkers()
    {
        Application.OpenURL("https://drive.google.com/drive/folders/13E7dn9GF34bh3ToHcU5ggFyoFibOf9nO?usp=sharing");
    }
}
