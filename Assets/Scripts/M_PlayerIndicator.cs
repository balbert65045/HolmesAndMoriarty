using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_PlayerIndicator : MonoBehaviour {

    public enum Player { player,oponnent}
    public Player myPlayer = Player.player;
    public Sprite HolmesSprtie;
    public Sprite MoriartySprite;

    public PlayerType ThisIndicator;

	
    public void SetPlayerIndicator(PlayerType MyPlayerType)
    {
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
    }
}
