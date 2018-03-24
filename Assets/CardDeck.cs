using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour {

    public List<Card> CardsInDeck;
    public List<Card> InitialCards;
    // Use this for initialization
    void Start()
    {
        foreach (Card card in CardsInDeck)
        {
            InitialCards.Add(card);
        }

    }

    public void ResetCards()
    {
        CardsInDeck.Clear();
        foreach (Card card in InitialCards)
        {
            CardsInDeck.Add(card);
        }
    }

    public Card DrawCard()
    {
        int RandomCardIndex = Random.Range(0, CardsInDeck.Count);
        Card CardPicked = CardsInDeck[RandomCardIndex];
        CardsInDeck.Remove(CardPicked);
        return (CardPicked);
    }

    public ClueCard FindCardInDeck(ClueCard card)
    {
        foreach (Card cardInDeck in InitialCards)
        {
            ClueCard clueCard = (ClueCard)cardInDeck;
            if (clueCard.Number == card.Number)
            {
                return clueCard;
            }

        }
        Debug.LogWarning("Cant find card in deck");
        return null;
    }
}
