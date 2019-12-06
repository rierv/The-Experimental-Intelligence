using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateComic : MonoBehaviour, I_Activable
{
    #region Attributes
    GameObject comicCloud;
    private Color color;
    private bool isActive = false;
    private bool activateAnimation = false;
    private Vector3 standardScale;
    private Vector3 littleScale;
    private Vector3 bigScale;
    private int animationPhase = 0;
    GameObject textCloud;
    public string[] completeTextPieces;
    private string completeText = "";
    string text;
    #endregion

    private void Awake()
    {
        comicCloud = gameObject.transform.Find("ComicCloud").gameObject;
        color = comicCloud.GetComponent<Image>().color;
        color.a = 0f;
        comicCloud.SetActive(false);
        textCloud = comicCloud.transform.GetChild(0).gameObject;
        standardScale = comicCloud.transform.localScale;
        bigScale = 1.1f * standardScale;
        littleScale = 0.9f * standardScale;
        for (int k = 0; k < completeTextPieces.Length - 1; k++)
        {
            completeTextPieces[k] = completeTextPieces[k].ToUpper();
            completeText += completeTextPieces[k] + "\n";
        }
        completeTextPieces[completeTextPieces.Length - 1] = completeTextPieces[completeTextPieces.Length - 1].ToUpper();
        completeText += completeTextPieces[completeTextPieces.Length - 1];
    }

    private void Update()
    {
        if(activateAnimation)
        {
            switch (animationPhase)
            {
                case 0:
                default:
                    StartCoroutine(Appear());
                    break;
                case 1:
                    StartCoroutine(ChangeScale(standardScale, bigScale));
                    break;
                case 2:
                    StartCoroutine(ChangeScale(bigScale, littleScale));
                    break;
                case 3:
                    StartCoroutine(ChangeScale(littleScale, standardScale));
                    break;
                case 4:
                    StartCoroutine(AnimateText(completeText));
                    activateAnimation = false;
                    break;
            }
        }
    }

    IEnumerator Appear()
    {
        for (float a = 0; color.a < 1; a += Time.deltaTime)
        {
            color.a = Mathf.Clamp(a, 0, 1);
            comicCloud.GetComponent<Image>().color = color;
            yield return null;
        }
        animationPhase++;
        StopAllCoroutines();
    }

    IEnumerator ChangeScale(Vector3 start, Vector3 finish)
    {
        for (float j = 0; j < 1; j += Time.deltaTime)
        {
            comicCloud.transform.localScale = Vector3.Lerp(start, finish, j);
            yield return null;
        }
        animationPhase++;
        StopAllCoroutines();
    }

    IEnumerator AnimateText(string completeText)
    {
        int i = 0;
        text = "";
        while (i < completeText.Length)
        {
            text += completeText[i++];
            textCloud.GetComponent<Text>().text = text;
            yield return new WaitForSeconds(0.2F);
        }
        animationPhase = 0;
        StopAllCoroutines();
    }



    public void Activate(bool type = true)
    {
        if (!isActive)
        {
            comicCloud.SetActive(true);
            comicCloud.GetComponent<Image>().color = color;
            isActive = true;
            activateAnimation = true;
        }
    }

    public void canActivate(bool enabled)
    {
        throw new System.NotImplementedException();
    }


    public void Deactivate()
    {

    }
}
