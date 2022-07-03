using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour, I_Activable {
    public int nextLevel;
    [HideInInspector]
	public Sprite profSprite;
	public ActivateComic activateComic;
	public float delayToStopFlapper = 0.1f;
	public float delayToLoadLevel = 0.3f;
	public MeshRenderer mesh;
	public Material activeMaterial;
	public Material nonActiveMaterial;
	public bool isActive = true;
	bool activable = true;
	//Light light;
	ParticleSystem particleSystem;
	PlayerMove p;
	GameManager gameManager;
	public int Stars = 0;
	//float logTimer = 0;

	void Start() {
		gameManager = FindObjectOfType<GameManager>();
		//light = GetComponentInChildren<Light>();
		//light.enabled = isActive;
		particleSystem = GetComponentInChildren<ParticleSystem>();
		p = FindObjectOfType<PlayerMove>();
		if (!isActive) {
			mesh.material = nonActiveMaterial;
			particleSystem.Stop();
		}
	}

	void Update() {
		//logTimer += Time.deltaTime;
	}

	private void OnTriggerEnter(Collider other) {
		if (isActive && other.GetComponent<JellyCore>()) {
            StartCoroutine(LoadLevel());
		}
	}

	IEnumerator LoadLevel() {
		//System.IO.File.AppendAllText(FlapperCore.logFile, logTimer.ToString());
		if (LevelSelectionManager.currentLevel == PlayerPrefs.GetInt("Unlocked Levels", 0))
			PlayerPrefs.SetInt("Unlocked Levels", LevelSelectionManager.currentLevel+1);
		PlayerPrefs.SetInt("Level" + LevelSelectionManager.currentLevel + "State", 0);
		if(Stars> PlayerPrefs.GetInt("Level" + LevelSelectionManager.currentLevel + "Stars", 0)) PlayerPrefs.SetInt("Level" + LevelSelectionManager.currentLevel + "Stars", Stars);
		PlayerPrefs.SetInt("CurrentStars", Stars);
		Stars = 0;
		activable = false;
		yield return new WaitForSeconds(delayToStopFlapper);
		GetComponent<AudioSource>().Play();
		//foreach (PlayerMove p in FindObjectsOfType<PlayerMove>()) {
		p.canMove = false;
		//}

		if (activateComic) {
			activateComic.Activate();
			if (profSprite)
				gameManager.profSprite.sprite = profSprite;
			yield return new WaitForSeconds(delayToLoadLevel);
		} else {
			yield return new WaitForSeconds(0.6f);
		}
		FindObjectOfType<MarkerlessLevelPositioning>().NextLevel();
    }

	public void Activate(bool type = true) {
		if (activable) {
			isActive = true;
			//light.enabled = isActive;
			if (isActive) {
				mesh.material = activeMaterial;
				particleSystem.Play();
			} else {
				mesh.material = nonActiveMaterial;
				particleSystem.Stop();
			}
		}
	}

	public void Deactivate() {
		if (activable) {
			isActive = false;
			//light.enabled = isActive;
			mesh.material = nonActiveMaterial;
			particleSystem.Stop();
		}
	}

	public void canActivate(bool enabled) {
		activable = enabled;
	}
}
