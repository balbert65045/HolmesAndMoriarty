using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myOponnent : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
    public void SetIndicator()
    {
        M_PlayerIndicator[] Indicators = FindObjectsOfType<M_PlayerIndicator>();
        foreach (M_PlayerIndicator PI in Indicators)
        {
            if (PI.myPlayer == M_PlayerIndicator.Player.oponnent)
            {
                PI.SetPlayerIndicator(GetComponentInChildren<M_PlayerController>().MyPlayerType);
            }
        }
    }


}
