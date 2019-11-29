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
    private StateManager flapperStateManager;
    private FlapperState currentState;
    private float minTemperature = -1.0f;
    private float maxTemperature = 1.0f;
    private float minFillAmount = 0.4f;
    private float maxFillAmount = 1.0f;
    //private float changeColorOffset;
    //private Color[] barColors;
    private Color flapperColor = new Color(0.6470588f, 0.9529412f, 0.9568628f, 1);
    private Color iceColor = new Color(0f, 0.5f, 1f, 1f);
    private Color fireColor = new Color(1f, 0.2f, 0f, 1f);
    #endregion

    private void Awake()
    {
        //changeColorOffset = (maxFillAmount - minFillAmount) / 5;
        thermometers = new GameObject[3];
        thermometers[0] = GameObject.Find("JellyThermometer");
        thermometers[1] = GameObject.Find("SolidThermometer");
        thermometers[2] = GameObject.Find("GaseousThermometer");
        foreach (var v in thermometers)
            v.SetActive(false);
        temperatureBar = gameObject.GetComponent<Image>();
        flapperStateManager = GameObject.Find("CORE").GetComponent<StateManager>();
        currentState = flapperStateManager.state;
        /*barColors = new Color[7];
        barColors[0] = new Color(0.2f, 0.6f, 1f, 1f);
        barColors[1] = new Color(0.2f, 0.8f, 1f, 1f);
        barColors[5] = new Color(0.2f, 1f, 1f, 1f);
        barColors[3] = new Color(1f, 0.4f, 0f, 1f);
        barColors[4] = new Color(1f, 0.2f, 0f, 1f);
        barColors[6] = new Color(1f, 0.6f, 0f, 1f);
        barColors[2] = new Color(0.65f, 0.95f, 0.96f, 1f);
        */
        HandleModels();
        HandleThermometer();

    }

    private void Update()
    {
        HandleThermometer();
    }

    private void HandleThermometer()
    {
        if (currentState != flapperStateManager.state)
        {
            currentState = flapperStateManager.state;
            HandleModels();
        }

        fillAmount = flapperStateManager.temperature;
        fillAmount = Map(fillAmount, minTemperature, maxTemperature, minFillAmount, maxFillAmount);
        if (fillAmount != temperatureBar.fillAmount)
        {
            temperatureBar.fillAmount = Mathf.Lerp(temperatureBar.fillAmount, fillAmount, 0.03f);
            HandleBarColor();
        }

    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    private void HandleModels()
    {
        switch (currentState)
        {
            case FlapperState.solid:
                thermometers[1].SetActive(true);
                thermometers[0].SetActive(false);
                thermometers[2].SetActive(false);
                break;
            case FlapperState.gaseous:
                thermometers[2].SetActive(true);
                thermometers[0].SetActive(false);
                thermometers[1].SetActive(false);
                break;
            case FlapperState.jelly:
            default:
                thermometers[0].SetActive(true);
                thermometers[1].SetActive(false);
                thermometers[2].SetActive(false);
                break;
        }
    }

    private void HandleBarColor()
    {
        if(flapperStateManager.temperature < 0)
            temperatureBar.color = Color.Lerp(flapperColor, iceColor, Mathf.Abs(flapperStateManager.temperature));
        else if(flapperStateManager.temperature > 0)
            temperatureBar.color = Color.Lerp(flapperColor, fireColor, Mathf.Abs(flapperStateManager.temperature));
        /*if (fillAmount < (minFillAmount + changeColorOffset))
            temperatureBar.color = barColors[0];
        else if (fillAmount < (minFillAmount + 2 * changeColorOffset))
            temperatureBar.color = barColors[1];
        else if (fillAmount < (minFillAmount + 3 * changeColorOffset))
        {
            switch (currentState)
            {
                case FlapperState.solid:
                    temperatureBar.color = barColors[5];
                    break;
                case FlapperState.gaseous:
                    temperatureBar.color = barColors[6];
                    break;
                case FlapperState.jelly:
                default:
                    temperatureBar.color = barColors[2];
                    break;
            }

        }
        else if (fillAmount < (minFillAmount + 4 * changeColorOffset))
            temperatureBar.color = barColors[3];
        else
            temperatureBar.color = barColors[4];*/
    }
}
