using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIndicator : MonoBehaviour {

    public Sprite HolmesSprtie;
    public Sprite MoriartySprite;

    private PlayerType MyPlayerType = PlayerType.Holmes;

	// Use this for initialization
	void Start () {
        if (FindObjectOfType<LevelPropertyManager>() != null)
        {
            MyPlayerType = FindObjectOfType<LevelPropertyManager>().GetPlayerType();
        }

        switch (MyPlayerType)
        {
            case PlayerType.Holmes:
                GetComponentInChildren<Image>().sprite = HolmesSprtie;
                break;
            case PlayerType.Moriarty:
                GetComponentInChildren<Image>().sprite = MoriartySprite;
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
