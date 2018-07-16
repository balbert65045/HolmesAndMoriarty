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


    public void EndTurn()
    {
        Controller = GetComponentInChildren<M_PlayerController>();
        Controller.PlayerEndTurn();
    }

}
