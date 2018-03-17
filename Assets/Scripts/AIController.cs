using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

    public List<ClueCard> CardsHolding;
    gameManager gameManager;
    public ClueDeck cardDeck;
    AICardArea CrimeArea;
    AICardArea ClueArea;

    public GameObject MoriartyTile;
    TileArea tileArea;

    public PlayerType MyPlayerType;

    public void DrawCards(int Number)
    {
        for (int i = 0; i < Number; i++)
        {
            ClueCard CardDrawn = (cardDeck.DrawCard()) as ClueCard;
            CardsHolding.Add(CardDrawn);
        }
        PlayCards();
    }

    //Basic Random guess. Needs modify to play smart
    void PlayCards()
    {
        //Clue Card Place Down
        int RandomIndex = Random.Range(0, CardsHolding.Count);
        GameObject ClueCard = Instantiate(CardsHolding[RandomIndex].gameObject);
        CardsHolding.Remove(CardsHolding[RandomIndex]);
        ClueArea.PlaceCard(ClueCard.GetComponent<ClueCard>(), gameManager.CurrentCaseOn);

        // Crime Card Place Down
        int RandomIndex2 = Random.Range(0, CardsHolding.Count);
        GameObject CrimeCard = Instantiate(CardsHolding[RandomIndex2].gameObject);
        CardsHolding.Remove(CardsHolding[RandomIndex2]);
        CrimeArea.PlaceCard(CrimeCard.GetComponent<ClueCard>(), gameManager.CurrentCaseOn);


    }

    public void ResetCards()
    {
        RemoveAllCards();
    }

    public void RemoveAllCards()
    {
        CardsHolding.Clear();
    }

    public void AddNewCards(List<ClueCard> Cards)
    {
        foreach (ClueCard card in Cards)
        {
            CardsHolding.Add(card);
        }
        PlayCards();
    }


    public void PickTile()
    {
        TileSpot[] TilesSpots = FindObjectsOfType<TileSpot>();
        List<TileSpot> OpenTileSpots = new List<TileSpot>();
        foreach (TileSpot TS in TilesSpots)
        {
            if (!TS.Used) { OpenTileSpots.Add(TS); }
        }
        int RandomOpenTileIndex = Random.Range(0, OpenTileSpots.Count);
        tileArea.PlaceTile(MoriartyTile, OpenTileSpots[RandomOpenTileIndex].Number, PlayerType.Moriarty);
    }

    // Use this for initialization
    void Start () {
        // cardDeck = FindObjectOfType<CardDeck>();
        tileArea = FindObjectOfType<TileArea>();
         AICardArea[] CardAreas = FindObjectsOfType<AICardArea>();
        foreach (AICardArea carda in CardAreas)
        {
            if (carda.ThisRow == CardArea.Row.Clue)
            {
                ClueArea = carda;
            }
            else if (carda.ThisRow == CardArea.Row.Crime)
            {
                CrimeArea = carda;
            }
        }

        gameManager = FindObjectOfType<gameManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
