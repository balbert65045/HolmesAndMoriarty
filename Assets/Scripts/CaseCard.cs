using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseCard : Card {


    public int CardEffect = 0; 
    public PlayerType PlayerType;
    public List<CardType> CardTypes = new List<CardType>();

    GameObject OldParent;

    bool MovedUp = false;
    public bool GetMovedUp { get { return MovedUp; } }

    public override void MoveUp(int pos)
    {
        MovedUp = true;
        OldParent = transform.parent.gameObject;
        Transform MovePos;
        if (GetComponentInParent<CaseArea>() != null)
        {
             MovePos = GetComponentInParent<CaseArea>().HighPositions[pos - 1];
        }
        else
        {
            Debug.Log("Finding new Case area");
             MovePos = GetComponentInParent<M_CaseArea>().HighPositions[pos - 1];
        }
        transform.SetParent(MovePos);
        transform.localPosition = Vector3.zero;
        transform.localScale = new Vector3(5, 7, .05f);
       // float newY = transform.position

    }

    public override void MoveBackDown()
    {
        MovedUp = false;
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
