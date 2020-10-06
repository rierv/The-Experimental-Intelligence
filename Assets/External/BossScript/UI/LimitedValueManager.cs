using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LimitedValueManager : MonoBehaviour
{
    public int min = 0, max = 100;
    private int trueMax = 0;
    Text txt;
    public Text compareTxtMax;
    void Start()
    {
        txt = GetComponent<Text>();

    }

    // Start is called before the first frame update
    public void increase()
    {
        if (int.Parse(txt.text) < trueMax)
            txt.text = "" + (int.Parse(txt.text) + 1);
    }
    public void decrease()
    {
        if (int.Parse(txt.text) > min)
            txt.text = "" + (int.Parse(txt.text) - 1);
    }

    // Update is called once per frame
    void Update()
    {
        trueMax = Mathf.Min(max, (int.Parse(compareTxtMax.text)-1));
        if (int.Parse(txt.text) > trueMax) txt.text = "" + trueMax;
    }
}
