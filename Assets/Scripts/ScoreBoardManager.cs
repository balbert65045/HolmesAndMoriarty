using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardManager : MonoBehaviour {

    // Use this for initialization
    LevelPropertyManager _levelPropertyManager;
    public TileArea tileArea;
    public Text WinPersonText;

    public Text TotalRoundsText;
    public Text HolmesCasesWonText;
    public Text MoriartyCasesWonText;


    void Start () {
        _levelPropertyManager = FindObjectOfType<LevelPropertyManager>();
        if (_levelPropertyManager == null) { Debug.LogWarning("No level property manager in scene"); }
        tileArea.CopyTileArea(_levelPropertyManager.Tile2DSaved);
        
        if (_levelPropertyManager.PlayerWon == PlayerType.Holmes)
        {
            WinPersonText.text = "Holmes Wins";
        }
        else
        {
            WinPersonText.text = "Moriarty Wins";
        }

        TotalRoundsText.text = _levelPropertyManager.TotalNumberofRounds.ToString() ;
        HolmesCasesWonText.text = _levelPropertyManager.NumberofHolmesCaseWon.ToString();
        MoriartyCasesWonText.text = _levelPropertyManager.NumberofMoriartyCaseWon.ToString();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
