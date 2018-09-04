using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myPlayer : MonoBehaviour {

    M_PlayerController Controller;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetIndicator()
    {
        Debug.Log("Setting indicator");
        M_PlayerIndicator[] Indicators = FindObjectsOfType<M_PlayerIndicator>();
        foreach (M_PlayerIndicator PI in Indicators)
        {
            if (PI.myPlayer == M_PlayerIndicator.Player.player)
            {
                Debug.Log("Setting indicator for player");
                PI.SetPlayerIndicator(GetComponentInChildren<M_PlayerController>().MyPlayerType);
            }
        }
    }

    public void EndTurn()
    {
        Controller = GetComponentInChildren<M_PlayerController>();
        Controller.PlayerEndTurn();
    }

}
