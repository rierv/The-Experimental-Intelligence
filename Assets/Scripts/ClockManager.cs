using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockManager : MonoBehaviour
{
    #region Attributes
    GameObject clockCenter;
    RectTransform clockCenterTransform;
    private int starCount = 0;
    private bool rotateTowardsStar = false;
    private Vector3[] starPositions;
    private float clockSpeed = 45f;
    private GameObject[] clockStars;
    private bool[] isStarActive;
    GameObject allStarsEffect;
    #endregion

    private void Awake()
    {
        clockCenter = GameObject.Find("ClockCenter");
        clockCenterTransform = clockCenter.GetComponent<RectTransform>();
        allStarsEffect = GameObject.Find("AllStarsEffect");
        allStarsEffect.SetActive(false);
        starPositions = new Vector3[3];
        for (int i = 0; i < starPositions.Length; i++)
            starPositions[i] = clockCenterTransform.eulerAngles;
        starPositions[0].z = -120f;
        starPositions[1].z = -240f;
        starPositions[2].z = 0f;
        clockStars = new GameObject[3];
        for(int i = 0; i < clockStars.Length; i++)
        {
            clockStars[i] = GameObject.Find("ClockStar" + (i + 1));
            clockStars[i].SetActive(false);
        }
        isStarActive = new bool[3];

    }
    private void Update()
    {
        if (rotateTowardsStar)
        {
            RotateTowardsStar();
        }
        
    }

    public void AddStar()
    {
        starCount++;
        rotateTowardsStar = true;
    }

    private void RotateTowardsStar()
    {
        switch (starCount)
        {
            case 1:
                ActivateStar(0, true);
                break;
            case 2:
                if (!isStarActive[0])
                    ActivateStar(0, false);
                else
                    ActivateStar(1, true);
                break;
            case 3:
            default:
                if (!isStarActive[0])
                    ActivateStar(0, false);
                else if (!isStarActive[1])
                    ActivateStar(1, false);
                else
                    ActivateStar(2, true);
                break;
        }
    }

    private void ActivateStar(int starIndex, bool stopAfterCurrent)
    {
        clockCenterTransform.rotation = Quaternion.RotateTowards(clockCenterTransform.rotation, Quaternion.Euler(starPositions[starIndex]), Time.deltaTime * clockSpeed);
        if (clockCenterTransform.rotation == Quaternion.Euler(starPositions[starIndex]))
        {
            if(stopAfterCurrent)
                rotateTowardsStar = false;
            clockStars[starIndex].SetActive(true);
            isStarActive[starIndex] = true;
            if (starIndex == 2)
                allStarsEffect.SetActive(true);
        }
    }
}
