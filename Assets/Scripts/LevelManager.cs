using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<GameObject> Levels = new List<GameObject>();
    public GameObject startLevel;
    GameObject myLevel;
    Pose myPlacementPose;
    int currentIndex = 0;
    // Start is called before the first frame update
    public void LoadNextLevel(int nextLevel)
    {
        myLevel.SetActive(false);
        Destroy(myLevel);
        
        if (nextLevel != 3) myLevel=Instantiate(Levels[nextLevel], myPlacementPose.position, myPlacementPose.rotation);
        else myLevel = Instantiate(Levels[nextLevel], myPlacementPose.position - Vector3.right / 3f - Vector3.up / 3f - Vector3.forward / 3f, Quaternion.identity);
        currentIndex = nextLevel;
    }

    internal void StartGame(Pose placementPose)
    {
        myPlacementPose = placementPose;
        myLevel=Instantiate(startLevel, placementPose.position, myPlacementPose.rotation);
    }
}
