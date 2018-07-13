using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class M_CardDeck : NetworkBehaviour {

    public List<Card> CardsInDeck;
    public List<Card> InitialCards;

    public int NumerOfCardsLeft = 0;

    // Use this for initialization
    void Start()
    {
        foreach (Card card in CardsInDeck)
        {
            InitialCards.Add(card);
            NumerOfCardsLeft++;
        }

    }

    public void ResetCards()
    {
        CardsInDeck.Clear();
        NumerOfCardsLeft = 0;
        foreach (Card card in InitialCards)
        {
            CardsInDeck.Add(card);
            NumerOfCardsLeft++;
        }
    }


// Using a counter to keep track of the Number of cards removed 
    public int SetCard()
    {
        int CardIndex = Random.Range(0, NumerOfCardsLeft);
        NumerOfCardsLeft--;
        return (CardIndex);
    }


    public Card GetandRemoveCard(int index)
    {
        Card card = CardsInDeck[index];
        CardsInDeck.Remove(CardsInDeck[index]); 
        return (card);
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
