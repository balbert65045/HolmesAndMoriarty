using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    // Use this for initialization
    public int Number;
    public enum CardType { Blue, Yellow, Green, Red}
    public CardType ThisCardType;

    public GameObject SelectionOutline;
    public bool Used = false;

	void Start () {
        SelectionOutline.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
		
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
        GetComponent<BoxCollider>().enabled = false;
    }

}
