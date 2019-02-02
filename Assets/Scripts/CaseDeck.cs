using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseDeck : CardDeck {

    public GameObject OriginalDeck;
    public GameObject ExpansionDeck;

    public void Start()
    {
        LevelPropertyManager levelPropertyManager = FindObjectOfType<LevelPropertyManager>();
        if (levelPropertyManager != null) { SetDeck(levelPropertyManager.GetPlotTwist()); }
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
    }

}
