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


    // Bad time syncing up 
    // Problems with giving the same card across the server
    public int SetCard()
    {
        Debug.Log("Setting Card Index card");
        int CardIndex = Random.Range(0, NumerOfCardsLeft);
        NumerOfCardsLeft--;
        Debug.Log(CardIndex);
        return (CardIndex);
    }

    //OLD Method maybe still used for Case cards
    //public Card DrawCard()
    //{
    //    Debug.Log("Grabbing card");
    //    int CardIndex = Random.Range(0, CardsInDeck.Count);
    //    Card card = CardsInDeck[CardIndex];
    //    return (card);
    //}

    public Card GetandRemoveCard(int index)
    {
        Debug.Log("Grabbing by index card");
        Debug.Log(index);
        Card card = CardsInDeck[index];
        CardsInDeck.Remove(CardsInDeck[index]); 
        return (card);
    }



    //[Command]
    //public void CmdDrawCard()
    //{
    //    Debug.Log("CMD drawiing card");
    //    int RandomCardIndex = Random.Range(0, CardsInDeck.Count);
    //    RpcDrawCard(RandomCardIndex);
    //}

    //[ClientRpc]
    //public void RpcDrawCard(int CardIndex)
    //{
    //    Debug.Log("RPC Setting card");
    //    CardDrawn = CardsInDeck[CardIndex];
    //    Draw = true;
    //}

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
