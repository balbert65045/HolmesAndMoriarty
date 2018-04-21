using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueCard : Card {

    // Use this for initialization
    public int Number;
    public CardType ThisCardType;

    public GameObject SelectionOutline;
    public bool Used = false;

    public SpriteRenderer FrontSprite;
    public SpriteRenderer BackSprite;

	void Start () {
        SelectionOutline.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FadeCard()
    {
        BackSprite.enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        FrontSprite.color = new Color(FrontSprite.color.r, FrontSprite.color.g, FrontSprite.color.b, .5f);
    }

    public void SelectCard()
    {
       // Debug.Log("Card " + gameObject.name + " Selected");
        SelectionOutline.SetActive(true);
    }

    public void DeSelectCard()
    {
        SelectionOutline.SetActive(false);
    }

    public void PlacedDown()
    {
        Used = true;
        //GetComponent<BoxCollider>().enabled = false;
    }

}
