using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    #region Attributes
    private float fillAmount;
    private Image temperatureBar;
    private GameObject[] thermometers;
    private Image[] thermometersImages;
    private StateManager flapperStateManager;
    //private FlapperState currentState;
    private float minTemperature = -1.0f;
    private float maxTemperature = 1.0f;
    private float minFillAmount = 0.4f;
    private float maxFillAmount = 1.0f;
    private Color flapperColor = new Color(0.6470588f, 0.9529412f, 0.9568628f, 1);
    private Color iceColor = new Color(0f, 0.5f, 1f, 1f);
    private Color fireColor = new Color(1f, 0.2f, 0f, 1f);
    private Color tmpColor;
    private float lastTemperature;
    private float currTemperature;
    private bool routineRunning;
    #endregion

    private void Awake()
    {
        thermometers = new GameObject[3];
        thermometersImages = new Image[3];
        thermometers[0] = GameObject.Find("JellyThermometer");
        thermometers[1] = GameObject.Find("SolidThermometer");
        thermometers[2] = GameObject.Find("GaseousThermometer");
        for (int i = 0; i < thermometers.Length; i++)
        {
            thermometers[i].SetActive(true);
            thermometersImages[i] = thermometers[i].GetComponent<Image>();
        }
        tmpColor = thermometersImages[0].color;
        temperatureBar = gameObject.GetComponent<Image>();
        flapperStateManager = GameObject.Find("CORE").GetComponent<StateManager>();
        //currentState = flapperStateManager.state;
        //HandleModels();
        routineRunning = false;
        HandleThermometer();

    }

    private void Update()
    {
        lastTemperature = currTemperature;
        currTemperature = flapperStateManager.temperature;
        HandleThermometer();
    }

    private void HandleThermometer()
    {

        fillAmount = Map(currTemperature, minTemperature, maxTemperature, minFillAmount, maxFillAmount);

        if (fillAmount != temperatureBar.fillAmount)
        {
            temperatureBar.fillAmount = Mathf.Lerp(temperatureBar.fillAmount, fillAmount, 0.03f);

            if (!routineRunning)
            {
                if (Mathf.Abs(currTemperature - lastTemperature) > 0.2f)
                    LerpToNewState();
                else
                    ThermometerLerp();
            }
        }

    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }


    private void LerpToNewState()
    {
        routineRunning = true;
        int previous;
        int current;

        if (lastTemperature == 0)
            previous = 0;
        else if (lastTemperature < 0)
            previous = 1;
        else
            previous = 2;

        if (currTemperature == 0)
            current = 0;
        else if (currTemperature < 0)
            current = 1;
        else
            current = 2;

        StartCoroutine(StateTransition(previous, current));
    }

    IEnumerator StateTransition(int prevState, int currState)
    {
        for (float k = thermometersImages[prevState].color.a; k > 0; k -= Time.deltaTime)
        {
            tmpColor.a = k;
            thermometersImages[prevState].color = tmpColor;
            tmpColor.a = 1 - k;
            thermometersImages[currState].color = tmpColor;
            if (currState == 1)
                temperatureBar.color = Color.Lerp(flapperColor, iceColor, 1 - k);
            else if (currState == 2)
                temperatureBar.color = Color.Lerp(flapperColor, fireColor, 1 - k);
            yield return null;
        }
        routineRunning = false;
        StopAllCoroutines();
    }

    private void ThermometerLerp()
    {
        float absTemp = Mathf.Abs(flapperStateManager.temperature);
        if (flapperStateManager.temperature < 0)
        {
            temperatureBar.color = Color.Lerp(flapperColor, iceColor, absTemp);
            tmpColor.a = 1 - absTemp;
            thermometersImages[0].color = tmpColor;
            tmpColor.a = absTemp;
            thermometersImages[1].color = tmpColor;
            tmpColor.a = 0;
            thermometersImages[2].color = tmpColor;

        }
        else if (flapperStateManager.temperature > 0)
        {
            temperatureBar.color = Color.Lerp(flapperColor, fireColor, absTemp);
            tmpColor.a = 1 - absTemp;
            thermometersImages[0].color = tmpColor;
            tmpColor.a = absTemp;
            thermometersImages[2].color = tmpColor;
            tmpColor.a = 0;
            thermometersImages[1].color = tmpColor;
        }
        else
        {
            temperatureBar.color = flapperColor;
            tmpColor.a = 0;
            thermometersImages[1].color = tmpColor;
            thermometersImages[2].color = tmpColor;
            tmpColor.a = 1;
            thermometersImages[0].color = tmpColor;
        }
    }
}
