using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPropertyManager : MonoBehaviour {

	// Use this for initialization

    public enum Difficulty { Easy, Medium, Hard}
    public Difficulty DifficultyPicked;

    public PlayerType PlayerTypePicked;

    public TileType[,] Tile2DSaved;
       
	void Awake () {
        DontDestroyOnLoad(this.gameObject);
        LevelPropertyManager[] LevelManagers = FindObjectsOfType<LevelPropertyManager>();
        if (LevelManagers.Length > 1) { Destroy(LevelManagers[1].gameObject); }

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

    public void SaveTileArea(TileType[,] Tile2D)
    {
        Tile2DSaved = Tile2D;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
