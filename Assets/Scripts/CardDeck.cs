using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour {

    public List<Card> CardsInDeck;

	// Use this for initialization
	void Start () {
		
	}
	
    public Card DrawCard()
    {
        int RandomCardIndex = Random.Range(0, CardsInDeck.Count);
        Card CardPicked = CardsInDeck[RandomCardIndex];
        CardsInDeck.Remove(CardPicked);
        return (CardPicked);
    }

}
