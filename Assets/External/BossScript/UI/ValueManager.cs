using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ValueManager : MonoBehaviour
{
    public int min = 0, max=10;

    Text txt;
    void Start()
    {
        txt = GetComponent<Text>();

    }

    // Start is called before the first frame update
    public void increase()
    {
        if(int.Parse(txt.text)<max)
            txt.text = ""+(int.Parse(txt.text) + 1);
    }
    public void decrease()
    {
        if (int.Parse(txt.text) > min)
            txt.text = ""+(int.Parse(txt.text) - 1);
    }

}
