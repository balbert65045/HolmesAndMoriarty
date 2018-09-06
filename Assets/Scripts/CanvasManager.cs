using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour {


    public GameObject StartMenu;
    public GameObject DifficultyMenu;
    public GameObject PlayerChoiceMenu;
    public LevelManager levelManager;
    public LevelPropertyManager _LevelPropertyManager;

	// Use this for initialization
	void Start () {
        _LevelPropertyManager = FindObjectOfType<LevelPropertyManager>();
        MoveToStartMenu();
    }

    public void MoveToStartMenu()
    {
        StartMenu.SetActive(true);
        DifficultyMenu.SetActive(false);
        PlayerChoiceMenu.SetActive(false);
    }


    public void MoveToDifficultyMenu()
    {
        StartMenu.SetActive(false);
        DifficultyMenu.SetActive(true);
        PlayerChoiceMenu.SetActive(false);
    }

    public void MoveToPlayerChoiceMenu()
    {
        StartMenu.SetActive(false);
        DifficultyMenu.SetActive(false);
        PlayerChoiceMenu.SetActive(true);
    }

    public void ChoseEasy()
    {
        _LevelPropertyManager.SetDifficulty(LevelPropertyManager.Difficulty.Easy);
        MoveToPlayerChoiceMenu();
    }

    public void ChoseMedium()
    {
        _LevelPropertyManager.SetDifficulty(LevelPropertyManager.Difficulty.Medium);
        MoveToPlayerChoiceMenu();
    }

    public void ChoseHard()
    {
        _LevelPropertyManager.SetDifficulty(LevelPropertyManager.Difficulty.Hard);
        MoveToPlayerChoiceMenu();
    }


    public void ChoseHolmes()
    {
        _LevelPropertyManager.SetPlayerType(PlayerType.Holmes);
        levelManager.LoadLevel("SplashHolmesSceen");
    }

    public void ChoseMoriarty()
    {
        _LevelPropertyManager.SetPlayerType(PlayerType.Moriarty);
        levelManager.LoadLevel("SplashMoriartySceen");
    }

    public void ChoseRandom()
    {
        int RandomCoinFlip = Random.Range(0, 2);
        if (RandomCoinFlip == 0) { ChoseHolmes(); }
        else if (RandomCoinFlip == 1) {ChoseMoriarty();}
        else { Debug.LogError("Coin Flip did not work"); }
        
    }

    public void LoadMultiplayerLobby()
    {
        levelManager.LoadLevel("PhotonMultiplayerLobby");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
