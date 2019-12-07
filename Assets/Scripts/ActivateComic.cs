using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateComic : MonoBehaviour, I_Activable
{
    #region Attributes
    public bool comicOrientation;
    public float duration;
    public float fadeTime;
    public float charWaitTime;
    GameObject comicCloud;
    private Color color;
    private bool isActive = false;
    private bool activateAnimation = false;
    private bool disactivateAnimation = false;
    public Vector3 standardScale;
    private Vector3 littleScale;
    private Vector3 bigScale;
    private int animationPhase = 0;
    GameObject textCloud;
    public string[] completeTextPieces;
    private string completeText = "";
    private string text;
    private Color textColor;
    public bool deactivateByTrigger = false;
    public bool activateTriggerAnyTime = false;
    private float stretchLerp = 0f;
    #endregion

    private void Start()
    {
        comicCloud = gameObject.transform.Find("ComicCloud").gameObject;
        comicCloud.transform.localScale = standardScale;
        color = comicCloud.GetComponent<SpriteRenderer>().color;
        color.a = 0f;
        comicCloud.SetActive(false);
        textCloud = comicCloud.transform.GetChild(0).gameObject;
        textColor = textCloud.GetComponent<Text>().color;
        bigScale = 1.1f * standardScale;
        littleScale = 0.9f * standardScale;
        text = "";
        for (int k = 0; k < completeTextPieces.Length - 1; k++)
        {
            completeTextPieces[k] = completeTextPieces[k].ToUpper();
            completeText += completeTextPieces[k] + "\n";
        }
        completeTextPieces[completeTextPieces.Length - 1] = completeTextPieces[completeTextPieces.Length - 1].ToUpper();
        completeText += completeTextPieces[completeTextPieces.Length - 1];
        if (comicOrientation)
            comicCloud.GetComponent<SpriteRenderer>().flipX = true;
    }

    private void Update()
    {
        if (activateAnimation)
        {
            StartCoroutine(StartAnimation());
            StartCoroutine(AnimateText());
            activateAnimation = false;
        }
        else if (disactivateAnimation)
        {
            StopAllCoroutines();
            StartCoroutine(EndAnimation());
            disactivateAnimation = false;
        }
    }

    IEnumerator StartAnimation()
    {
        Appear();
        Stretch();
        yield return new WaitForSeconds(Time.deltaTime / fadeTime);
        yield return StartAnimation();
    }

    private void Appear()
    {
        color.a += Time.deltaTime / fadeTime;
        color.a = Mathf.Clamp(color.a, 0, 1);
        textColor.a = color.a;
        comicCloud.GetComponent<SpriteRenderer>().color = color;
        textCloud.GetComponent<Text>().color = textColor;
    }

    private void Stretch()
    {
        switch (animationPhase)
        {
            case 0:
                comicCloud.transform.localScale = Vector3.Lerp(standardScale, bigScale, stretchLerp);
                break;
            case 1:
                comicCloud.transform.localScale = Vector3.Lerp(bigScale, littleScale, stretchLerp);
                break;
            case 2:
                comicCloud.transform.localScale = Vector3.Lerp(littleScale, standardScale, stretchLerp);
                break;
            default:
                break;
        }
        stretchLerp += (Time.deltaTime / fadeTime) * 3;
        if (stretchLerp > 1)
        {
            animationPhase++;
            stretchLerp = 0f;
        }
    }


    IEnumerator AnimateText()
    {
        int index;
        int newLineCount;
        for (index = 0, newLineCount = 1; index < completeText.Length; index++)
        {
            text += completeText[index];

            if (completeText[index] == '\n')
            {
                newLineCount++;
            }
            if (newLineCount > 3)
            {
                text = text.Remove(0, text.IndexOf('\n'));
                newLineCount--;
            }
            textCloud.GetComponent<Text>().text = text;
            yield return new WaitForSeconds(charWaitTime);

        }
        if (index == completeText.Length)
        {
            yield return new WaitForSeconds(duration);
            disactivateAnimation = true;
        }
    }

    IEnumerator EndAnimation()
    {
        Disappear();
        yield return new WaitForSeconds(Time.deltaTime / fadeTime);
        yield return EndAnimation();
    }

    private void Disappear()
    {
        color.a -= Time.deltaTime / fadeTime;
        color.a = Mathf.Clamp(color.a, 0, 1);
        textColor.a = color.a;
        comicCloud.GetComponent<SpriteRenderer>().color = color;
        textCloud.GetComponent<Text>().color = textColor;
        if (color.a <= 0)
        {
            activateAnimation = false;
            disactivateAnimation = false;
            StopAllCoroutines();
        }
    }

    public void Activate(bool type = true)
    {
        if (!isActive)
        {
            comicCloud.SetActive(true);
            comicCloud.GetComponent<SpriteRenderer>().color = color;
            isActive = true;
            disactivateAnimation = false;
            activateAnimation = true;
            animationPhase = 0;
            text = "";
            bigScale = 1.1f * standardScale;
            littleScale = 0.9f * standardScale;
            color.a = 0f;
        }
    }

    public void canActivate(bool enabled)
    {
        throw new System.NotImplementedException();
    }


    public void Deactivate()
    {
        if (activateTriggerAnyTime) isActive = false;

        if (deactivateByTrigger)
        {
            bigScale = 1.1f * standardScale;
            littleScale = 0.9f * standardScale;
            color.a = 0f;
            activateAnimation = false;
            animationPhase = 0;
            text = "";
            disactivateAnimation = true;
            StartCoroutine(EndAnimation());
            comicCloud.SetActive(false);


        }
    }
}
