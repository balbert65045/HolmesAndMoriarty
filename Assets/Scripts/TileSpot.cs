using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpot : MonoBehaviour {

    public CardType ThisCardType;
    public int Number;
    public bool Used = false;
    public int ThreatLevel = 0;

    public bool Highlighted = false;
    public bool GetHighlighted { get { return Highlighted; } }
    public void SetHighlighted(bool value) {Highlighted = value;}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
