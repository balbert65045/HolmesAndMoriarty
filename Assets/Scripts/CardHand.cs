using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHand : MonoBehaviour {

    // Use this for initialization
    CardDeck cardDeck;
    public Transform[] CardPositions;
    public List<Card> CardsHolding;

	void Start () {
        cardDeck = FindObjectOfType<CardDeck>();

    }
	
    public void DrawNewCards(int numberOfCards)
    {
        for (int i = 0; i< numberOfCards; i++)
        {
           Card CardDrawn = cardDeck.DrawCard();
           CardsHolding.Add(CardDrawn);
           GameObject Card = Instantiate(CardDrawn.gameObject, CardPositions[i]);
        }
    }

    public void RemoveCard(Card card)
    {
        foreach (Card HCard in CardsHolding)
        {
            if (HCard.Number == card.Number && HCard.ThisCardType == card.ThisCardType)
            {
                CardsHolding.Remove(HCard);
                return;
            }
        }
    }

    public void ResetCards()
    {
        RemoveAllCards();
    }

    public void RemoveAllCards()
    {

        foreach (Transform position in CardPositions)
        {
            if (position.GetComponentInChildren<Card>())
            {
                Destroy((position.GetComponentInChildren<Card>().gameObject));
            }
        }
        CardsHolding.Clear();
    }

    public void AddNewCards(List<Card> cards)
    {
        int i = 0;
        if (cards.Count == 5)
        {
            i = 1;
        }
        else if (cards.Count == 3)
        {
            i = 2;
        }

        foreach (Card card in cards)
        {
            GameObject Card = Instantiate(card.gameObject, CardPositions[i]);
            CardsHolding.Add(card);
            i++;
        }
    }

}
