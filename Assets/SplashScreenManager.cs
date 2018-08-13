using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenManager : MonoBehaviour {

    // Use this for initialization
    public float delayTime = 2f;
    public string LevelName = "VerticleLayout";


    void Start () {
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if (levelManager != null) { levelManager.LoadLevelWithDelay(LevelName, delayTime); }
        else { Debug.Log("Could not find levelManager"); }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
