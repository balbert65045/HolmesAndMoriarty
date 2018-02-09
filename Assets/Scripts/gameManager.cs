using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour {


    public GameObject HolmesTile;
    public GameObject MoriartTile;

    TileArea tileArea;

    public int CurrentCaseOn = 1;

    CardArea CrimeArea;
    CardArea ClueArea;
    AICardArea ACrimeArea;
    AICardArea AClueArea;

    EndTurnButton endTurnButton;
    PlayerController playerController;
    AIController aiController;
    CardHand cardHand;

    public List<Card> PlayersCards = new List<Card>();
    public List<Card> AICards = new List<Card>();
    // Use this for initialization
    void Start () {
       
        endTurnButton = FindObjectOfType<EndTurnButton>();
        tileArea = FindObjectOfType<TileArea>();
        CardArea[] CardAreas = FindObjectsOfType<CardArea>();
        foreach (CardArea carda in CardAreas)
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
        AICardArea[] ACardAreas = FindObjectsOfType<AICardArea>();
        foreach (AICardArea carda in ACardAreas)
        {
            if (carda.ThisRow == CardArea.Row.Clue)
            {
                AClueArea = carda;
            }
            else if (carda.ThisRow == CardArea.Row.Crime)
            {
                ACrimeArea = carda;
            }
        }

        playerController = FindObjectOfType<PlayerController>();
        aiController = FindObjectOfType<AIController>();
        cardHand = FindObjectOfType<CardHand>();

        StartCoroutine("PlayersDrawCards");
    }
	
    IEnumerator PlayersDrawCards()
    {
        yield return new WaitForSeconds(.5f);
        playerController.DrawCards(7);
        aiController.DrawCards(7);
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(1f);
        playerController.ResetCards();
        aiController.ResetCards();
        //ClueArea
        //CrimeArea
        //AClueArea
        //ACrimeArea

    }

    public void CheckEndTurn()
    {
        if (!ClueArea.CheckForAvailableSpace(CurrentCaseOn) && !CrimeArea.CheckForAvailableSpace(CurrentCaseOn))
        {
            endTurnButton.EnableEndTurn();
        }
    }

    public void EndTurn()
    {
        endTurnButton.DisableEndTurn();
        if (CurrentCaseOn == 3)
        {
            for (int i = 1; i <= 3; i++)
            {
                CheckForScore(i);
            }
            CurrentCaseOn = 1;
        }
        else
        {
            CurrentCaseOn++;
            SwapCards();
        }
    }


    void SwapCards()
    {
        PlayersCards.Clear();
        AICards.Clear();
        foreach (Card card in cardHand.CardsHolding){
            Debug.Log(card);
            PlayersCards.Add(card); }
        foreach (Card card in aiController.CardsHolding) { AICards.Add(card); }
        aiController.RemoveAllCards();
        cardHand.RemoveAllCards();

        aiController.AddNewCards(PlayersCards);
        cardHand.AddNewCards(AICards);
    }

    void CheckForScore(int Case)
    {

        Card PlayerCrimeCard;
        Card PlayerClueCard;
        Card AICrimeCard;
        Card AIClueCard;
        FlipCards(Case, out PlayerCrimeCard, out PlayerClueCard, out AICrimeCard, out AIClueCard);
        Card.CardType Trump = CheckForTrump(PlayerCrimeCard, AICrimeCard);
        if (CheckForPlayerWin(Trump, PlayerClueCard, AIClueCard))
        {
           switch (playerController.MyPlayerType)
            {
                case PlayerType.Holmes:
                    tileArea.PlaceTile(HolmesTile, PlayerClueCard.Number);
                    break;
                case PlayerType.Moriarty:
                    tileArea.PlaceTile(MoriartTile, PlayerCrimeCard.Number);
                    break;
            }
        }
        else
        {
            switch (aiController.MyPlayerType)
            {
                case PlayerType.Holmes:
                    tileArea.PlaceTile(HolmesTile, AIClueCard.Number);
                    break;
                case PlayerType.Moriarty:
                    tileArea.PlaceTile(MoriartTile, AICrimeCard.Number);
                    break;
            }
        }
    }

    void FlipCards(int Case, out Card PlayerCrimeCard, out Card PlayerClueCard, out Card AICrimeCard, out Card AIClueCard)
    {
        PlayerCrimeCard = CrimeArea.FlipCard(Case);
        PlayerClueCard = ClueArea.FlipCard(Case);
        AICrimeCard = ACrimeArea.FlipCard(Case);
        AIClueCard = AClueArea.FlipCard(Case);
    }

    Card.CardType CheckForTrump(Card PlayerCrimeCard, Card AICrimeCard)
    {
        if (PlayerCrimeCard.Number > AICrimeCard.Number)
        {
            return PlayerCrimeCard.ThisCardType;
        }
        else if (AICrimeCard.Number > PlayerCrimeCard.Number)
        {
            return AICrimeCard.ThisCardType;
        }
        else
        {
            Debug.LogError("Player Card and AI card should never be the same");
            return Card.CardType.Red;
        }
    }

    // Check to see who has the highest card with trump in play 
    // first check who has trump and then if either both do or do not check for highest card
    bool CheckForPlayerWin(Card.CardType Trump, Card PlayerClueCard, Card AIClueCard)
    {
        if (PlayerClueCard.ThisCardType == Trump && AIClueCard.ThisCardType == Trump)
        {
            if (PlayerClueCard.Number > AIClueCard.Number)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (PlayerClueCard.ThisCardType == Trump)
        {
            return true;
        }
        else if (AIClueCard.ThisCardType == Trump)
        {
            return false;
        }
        else
        {
            if (PlayerClueCard.Number > AIClueCard.Number)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
