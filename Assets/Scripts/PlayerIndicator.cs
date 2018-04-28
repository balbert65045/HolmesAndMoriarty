using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIndicator : MonoBehaviour {

    public enum Player { player,oponnent}
    public Player myPlayer = Player.player;
    public Sprite HolmesSprtie;
    public Sprite MoriartySprite;

    public PlayerType MyPlayerType = PlayerType.Holmes;
    public PlayerType ThisIndicator;

	// Use this for initialization
	void Awake () {
        if (FindObjectOfType<LevelPropertyManager>() != null)
        {
            MyPlayerType = FindObjectOfType<LevelPropertyManager>().GetPlayerType();
        }

        switch (myPlayer)
        {
            case Player.player:
                switch (MyPlayerType)
                {
                    case PlayerType.Holmes:
                        GetComponentInChildren<SpriteRenderer>().sprite = HolmesSprtie;
                        ThisIndicator = PlayerType.Holmes;
                        break;
                    case PlayerType.Moriarty:
                        GetComponentInChildren<SpriteRenderer>().sprite = MoriartySprite;
                        ThisIndicator = PlayerType.Moriarty;
                        break;
                }
                break;

            case Player.oponnent:
                switch (MyPlayerType)
                {
                    case PlayerType.Holmes:
                        GetComponentInChildren<SpriteRenderer>().sprite = MoriartySprite;
                        ThisIndicator = PlayerType.Moriarty;
                        break;
                    case PlayerType.Moriarty:
                        GetComponentInChildren<SpriteRenderer>().sprite = HolmesSprtie;
                        ThisIndicator = PlayerType.Holmes;
                        break;
                }
                break;
        }
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
