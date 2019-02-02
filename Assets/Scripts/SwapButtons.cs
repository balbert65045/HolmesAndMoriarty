using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapButtons : MonoBehaviour {

    public int Button;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Swap()
    {
        if (FindObjectOfType<PlayerController>() != null) { FindObjectOfType<PlayerController>().SwapClueCards(Button); }
        else { FindObjectOfType<myPlayer>().GetComponentInChildren<M_PlayerController>().SwapClueCardsButton(Button); }
    }
}
