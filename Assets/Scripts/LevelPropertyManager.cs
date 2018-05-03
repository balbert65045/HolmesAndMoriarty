using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPropertyManager : MonoBehaviour {

	// Use this for initialization

    public enum Difficulty { Easy, Medium, Hard}
    public Difficulty DifficultyPicked;
    public PlayerType PlayerWon;
    public PlayerType PlayerTypePicked;

    public int TotalNumberofRounds;
    public int NumberofHolmesCaseWon;
    public int NumberofMoriartyCaseWon;

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

    public void SetPlayerWon(PlayerType PT)
    {
        PlayerWon = PT;
    }

    public void SetDetails(int TotRounds, int HolmsWins, int MoriartyWins)
    {
        TotalNumberofRounds = TotRounds;
        NumberofHolmesCaseWon = HolmsWins;
        NumberofMoriartyCaseWon = MoriartyWins;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
