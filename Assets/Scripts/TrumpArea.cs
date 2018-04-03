using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpArea : MonoBehaviour {

    public Sprite[] TrumpSprites;

    Sprite _sprite;
	// Use this for initialization
	void Start () {
        _sprite = GetComponentInChildren<SpriteRenderer>().sprite;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
