using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour {

    // Use this for initialization
    public Button[] TurnButtons;
    public int CurrentTurn = 1;  

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void NextTurn()
    {
        TurnButtons[CurrentTurn - 1].interactable = false;
        CurrentTurn++;
        TurnButtons[CurrentTurn - 1].interactable = true;
    }

}
