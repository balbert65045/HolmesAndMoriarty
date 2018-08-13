﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    // Use this for initialization


    public float autoLoadNextLevelAfter;

    private void Awake()
    {
        LevelManager[] levelManagers = FindObjectsOfType<LevelManager>();
        if (levelManagers.Length > 1) { Destroy(levelManagers[1].gameObject); }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        if (autoLoadNextLevelAfter <= 0)
        {
            Debug.Log("Auto Load Disabled, use a positive number in seconds");
        } else
        {
            Invoke("LoadNextLevel", autoLoadNextLevelAfter);
        }
    }

    public void LoadLevelWithDelay(string levelName, float timeDelay)
    {
        IEnumerator LoadTheLevel = LoadLevel(levelName, timeDelay);
        StartCoroutine(LoadTheLevel);
    }

    IEnumerator LoadLevel(string levelname, float delaytime)
    {
        yield return new WaitForSeconds(delaytime);
        SceneManager.LoadScene(levelname);
    }

	public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
        Debug.Log("Level load requested for: " + name);
    }

    public void QuitRequest()
    {
       Application.Quit();
        Debug.Log("Quit Game");
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ReloadLevel()
    {
        Debug.Log("Reloading Level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
