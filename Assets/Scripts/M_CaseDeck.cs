using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_CaseDeck : M_CardDeck {
    public GameObject OriginalDeck;
    public GameObject ExpansionDeck;

    public void Start()
    {
        Debug.Log("Setting up deck");
        PhotonLauncher PhotonL = FindObjectOfType<PhotonLauncher>();
        if (PhotonL != null) { SetDeck(PhotonL.LostInThoughtEnabled); }
        else { SetDeck(false); }

    }

    public void SetDeck(bool expansionOn)
    {
        if (expansionOn)
        {
            InitialCards = new List<Card>(ExpansionDeck.GetComponentsInChildren<Card>());
            CardsInDeck = new List<Card>(ExpansionDeck.GetComponentsInChildren<Card>());
        }
        else
        {
            InitialCards = new List<Card>(OriginalDeck.GetComponentsInChildren<Card>());
            CardsInDeck = new List<Card>(OriginalDeck.GetComponentsInChildren<Card>());
        }

        foreach (Card card in CardsInDeck)
        {
            NumerOfCardsLeft++;
        }
    }

}
