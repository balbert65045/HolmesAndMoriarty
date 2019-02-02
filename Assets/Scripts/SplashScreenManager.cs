using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenManager : MonoBehaviour {

    // Use this for initialization
    public float delayTime = 2f;
    public string LevelName = "VerticleLayout";
    LevelManager levelManager;



    void Start () {
        Time.timeScale = 1;

         levelManager = FindObjectOfType<LevelManager>();
        if (levelManager != null) {
            //Debug.Log("Loading Next Level");
            levelManager.LoadLevelWithDelay(LevelName, delayTime);
        }
        else { Debug.Log("Could not find levelManager"); }
	}
	
	// Update is called once per frame
	void Update () {
        //Counter += Time.deltaTime;
    }
}
