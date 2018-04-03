using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseCard : Card {

    public PlayerType PlayerType;
    public List<CardType> CardTypes = new List<CardType>();

    GameObject OldParent;

    public void MoveUp(int pos)
    {
        OldParent = transform.parent.gameObject;
        Transform MovePos = GetComponentInParent<CaseArea>().HighPositions[pos - 1];
        transform.SetParent(MovePos);
        transform.localPosition = Vector3.zero;
        transform.localScale = new Vector3(5, 7, .05f);
       // float newY = transform.position

    }

    public void MoveBackDown()
    {
        Transform MovePos = OldParent.transform;
        transform.SetParent(MovePos);
        transform.localPosition = Vector3.zero;
        transform.localScale = new Vector3(5, 7, .05f);
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
