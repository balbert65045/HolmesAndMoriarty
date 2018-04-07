using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPropertyManager : MonoBehaviour {

	// Use this for initialization

    public enum Difficulty { Easy, Medium, Hard}
    public Difficulty DifficultyPicked;

    public PlayerType PlayerTypePicked;
       
	void Start () {
        DontDestroyOnLoad(this.gameObject);
        LevelPropertyManager[] LevelManagers = FindObjectsOfType<LevelPropertyManager>();
        if (LevelManagers.Length > 1) { Destroy(LevelManagers[0].gameObject); }

	}
	

    public void SetDifficulty(Difficulty D)
    {
        DifficultyPicked = D;
    }


    public void SetPlayerType(PlayerType PT)
    {
        PlayerTypePicked = PT;
    }

    public PlayerType GetPlayerType()
    {
        return PlayerTypePicked;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
