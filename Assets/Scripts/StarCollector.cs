using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCollector : MonoBehaviour
{
    #region Attributes
    private int starCount = 0;
    private GameObject[] clockStars;
    GameObject allStarsEffect;
    #endregion

    private void Awake()
    {
        GameObject.Find("ClockHand").SetActive(false);
        allStarsEffect = GameObject.Find("AllStarsEffect");
        allStarsEffect.SetActive(false);
        clockStars = new GameObject[3];
        for (int i = 0; i < clockStars.Length; i++)
        {
            clockStars[i] = GameObject.Find("ClockStar" + (i + 1));
            clockStars[i].SetActive(false);
        }
        allStarsEffect.transform.localScale = Vector3.one * 1200f;

    }


    public void AddStar()
    {
        clockStars[starCount].SetActive(true);
        clockStars[starCount].transform.Find("StarPickupEffect").localScale = Vector3.one * 1000f;
        starCount++;
        if (starCount == 3)
            allStarsEffect.SetActive(true);
    }
}
