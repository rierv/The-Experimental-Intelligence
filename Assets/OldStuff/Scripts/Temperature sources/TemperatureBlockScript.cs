using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureBlockScript : MonoBehaviour
{
    #region Attributes
    public GameObject iceBlock;
    private float temperature;
    private float increasingTemperatureFactor = 15f;
    private Vector3 localScale;
    private float tmp;
    private Vector3 scalingFactor;

    #endregion
    private void Start()
    {
        temperature = -100f;
        localScale = iceBlock.transform.localScale;
        tmp = Time.fixedDeltaTime / 80;
        scalingFactor = new Vector3(tmp, tmp, tmp);
    }

    private void FixedUpdate()
    {
        if(temperature >= 0f)
        {
            Destroy(iceBlock);
            Destroy(this);
        }
        else
        {
            temperature += (Time.fixedDeltaTime * increasingTemperatureFactor);
            localScale -= scalingFactor;
            iceBlock.transform.localScale = localScale;
        }
    }
}
