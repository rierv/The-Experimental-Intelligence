using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivateComic : MonoBehaviour, I_Activable {
	#region Attributes
	public TextMeshPro textMesh;
	public float fadeInTime;
	public float fadeOutTime;
	public float charWaitTime;
	//public float duration;

	GameObject comicCloud;
	private Color color;
	private bool isActive = false;
	private bool activateAnimation = false;
	private bool deactivateAnimation = false;

	public Vector3 standardScale;
	private Vector3 littleScale;
	private Vector3 bigScale;
	private int animationPhase = 0;
	private bool isAnimationRunning = false;

	//GameObject textCloud;
	//public string[] completeTextPieces;
	[Multiline]
	public string completeText = "";
	private string text;
	private Color textColor;

	[Header("Negative values means wait external trigger")]
	public float activateAfter = 0;
	public float deactivateAfter = 0;
	public GameObject[] objectsToActivateOnFinish = new GameObject[0];

	/*public bool deactivateByTrigger = false;
	public bool activateTriggerAnyTime = false;*/
	private float stretchLerp = 0f;
	#endregion

	private void Start() {
		comicCloud = gameObject.transform.Find("ComicCloud").gameObject;
		comicCloud.transform.localScale = standardScale;
		color = comicCloud.GetComponent<SpriteRenderer>().color;
		color.a = 0f;
		comicCloud.SetActive(false);
		//textCloud = comicCloud.transform.GetChild(0).gameObject;
		textColor = textMesh.color;
		bigScale = 1.1f * standardScale;
		littleScale = 0.9f * standardScale;
		/*text = "";
		for (int k = 0; k < completeTextPieces.Length - 1; k++) {
			completeTextPieces[k] = completeTextPieces[k].ToUpper();
			completeText += completeTextPieces[k] + '\n';
		}
		completeTextPieces[completeTextPieces.Length - 1] = completeTextPieces[completeTextPieces.Length - 1].ToUpper();
		completeText += completeTextPieces[completeTextPieces.Length - 1];*/

		if (activateAfter >= 0) {
			StartCoroutine(ActivateCoroutine());
		}
	}
	IEnumerator ActivateCoroutine() {
		yield return new WaitForSeconds(activateAfter);
		Activate();
	}

	private void Update() {
		if (activateAnimation) {
			GetComponent<AudioSource>().Play();
			StartCoroutine(StartAnimation());
			StartCoroutine(AnimateText());
			activateAnimation = false;
		} else if (deactivateAnimation) {
			StopAllCoroutines();
			StartCoroutine(EndAnimation());
			deactivateAnimation = false;
		}
	}

	IEnumerator StartAnimation() {
		Appear();
		Stretch();
		yield return new WaitForSeconds(Time.deltaTime / fadeInTime);
		yield return StartAnimation();
	}

	private void Appear() {
		color.a += Time.deltaTime / fadeInTime;
		color.a = Mathf.Clamp(color.a, 0, 1);
		textColor.a = color.a;
		comicCloud.GetComponent<SpriteRenderer>().color = color;
		textMesh.color = textColor;
	}

	private void Stretch() {
		switch (animationPhase) {
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
		stretchLerp += (Time.deltaTime / fadeInTime) * 3;
		if (stretchLerp > 1) {
			animationPhase++;
			stretchLerp = 0f;
		}
	}


	IEnumerator AnimateText() {
		int index;
		int newLineCount;
		for (index = 0, newLineCount = 1; index < completeText.Length; index++) {
			text += completeText[index];

			if (completeText[index] == '\n') {
				newLineCount++;
			}
			if (newLineCount > 3) {
				text = text.Remove(0, text.IndexOf('\n') + 1);
				newLineCount--;
			}

			textMesh.text = text;
			yield return new WaitForSeconds(charWaitTime);

		}

		/*if (index == completeText.Length && !deactivateByTrigger) {
			yield return new WaitForSeconds(duration);
			deactivateAnimation = true;
		}*/
		if (deactivateAfter >= 0) {
			yield return new WaitForSeconds(deactivateAfter);
			deactivateAnimation = true;
		}
	}

	IEnumerator EndAnimation() {
		while (textColor.a > 0) {
			color.a -= Time.deltaTime / fadeOutTime;
			color.a = Mathf.Clamp(color.a, 0, 1);
			textColor.a = color.a;
			comicCloud.GetComponent<SpriteRenderer>().color = color;
			textMesh.color = textColor;
			yield return new WaitForSeconds(Time.deltaTime / fadeOutTime);
		}
		activateAnimation = false;
		deactivateAnimation = false;
		isAnimationRunning = false;
		/*Disappear();
		//yield return EndAnimation();*/
		foreach (GameObject go in objectsToActivateOnFinish) {
			if (go.GetComponent<I_Activable>() != null) {
				go.GetComponent<I_Activable>().Activate();
			}
		}
		Deactivate();
	}

	/*private void Disappear() {
		color.a -= Time.deltaTime / fadeOutTime;
		color.a = Mathf.Clamp(color.a, 0, 1);
		textColor.a = color.a;
		comicCloud.GetComponent<SpriteRenderer>().color = color;
		textMesh.color = textColor;
		if (color.a <= 0) {
			activateAnimation = false;
			deactivateAnimation = false;
			isAnimationRunning = false;
			StopAllCoroutines();
		}
	}*/

	public void Activate(bool type = true) {
		if (!isActive && !isAnimationRunning) {
			StopAllCoroutines();
			comicCloud.SetActive(true);
			comicCloud.GetComponent<SpriteRenderer>().color = color;
			isActive = true;
			deactivateAnimation = false;
			activateAnimation = true;
			animationPhase = 0;
			text = "";
			color.a = 0f;
			stretchLerp = 0;
			comicCloud.transform.localScale = standardScale;
			isAnimationRunning = true;
		}
	}

	public void canActivate(bool enabled) {
		throw new System.NotImplementedException();
	}


	public void Deactivate() {
		//if (activateTriggerAnyTime) {
		isActive = false;
		//}

		//if (deactivateByTrigger) {
		StopAllCoroutines();
		activateAnimation = false;
		deactivateAnimation = true;
		//}
	}
}