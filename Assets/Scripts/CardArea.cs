using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardArea : MonoBehaviour {

    // Use this for initialization
    public enum CardAreaType { Player, Opponent}
    public CardAreaType ThisCardAreaType;

    public enum Row { Clue, Crime }
    public Row ThisRow;
    public GamePlayer PlayerAttached;

    public RowAreaPosition[] Positions;

    public Card[] Cards = new Card[3];

    void Start () {
        Positions = GetComponentsInChildren<RowAreaPosition>();
        for (int i = 0; i < Positions.Length; i++)
        {
            if  (Positions[i].Case == 1)
            {
                RowAreaPosition TemPos = Positions[0];
                Positions[0] = Positions[i];
                Positions[i] = TemPos;
            }
            else if (Positions[i].Case == 2)
            {
                RowAreaPosition TemPos = Positions[1];
                Positions[1] = Positions[i];
                Positions[i] = TemPos;
            }
            else if (Positions[i].Case == 3)
            {
                RowAreaPosition TemPos = Positions[2];
                Positions[2] = Positions[i];
                Positions[i] = TemPos;
            }
            else
            {
                Debug.LogWarning("Case not found");
            }
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public ClueCard GetCard(int Pos)
    {
        return (ClueCard)Cards[Pos - 1];
    }

    public void ActiveSpot(int CaseN)
    {
        Positions[CaseN - 1].GetComponent<RowAreaPosition>().SetActiveImage(true);
    }

    public void DeActiveSpot(int CaseN)
    {
        Positions[CaseN - 1].GetComponent<RowAreaPosition>().SetActiveImage(false);
    }

    public bool CheckForAvailableSpace(int CaseN)
    {
        if (CaseN > 3) { return false; }
        else if (!Positions[CaseN - 1].InUse) { return true; }
        return false;
    }

    public int GetCardPosition(ClueCard card)
    {
        for (int i = 0; i < Positions.Length; i++)
        {
            if (GetCard(i+1) == card) { return i+1; }
        }
        return -1;
    }

    public void RemoveCard(ClueCard card)
    {
        foreach (RowAreaPosition RP in Positions)
        {
            if (RP.GetComponentInChildren<ClueCard>())
            {
                if (RP.GetComponentInChildren<ClueCard>().Number == card.Number)
                {
                    RP.InUse = false;
                    Cards[RP.Case - 1] = null;
                }
            }
        }
    }

    public void MoveCard(ClueCard card)
    {
        foreach (RowAreaPosition RP in Positions)
        {
            if (RP.GetComponentInChildren<ClueCard>())
            {
                if (RP.GetComponentInChildren<ClueCard>().Number == card.Number)
                {
                    RP.InUse = false;
                }
            }
        }
    }

    public void PlaceCardUp(ClueCard card, int CaseN)
    {
        // simply placing it in position one for now 
        card.transform.SetParent(Positions[CaseN - 1].transform);
        card.transform.position = Positions[CaseN - 1].transform.position;
        card.transform.rotation = Positions[CaseN - 1].transform.rotation;
        float y = card.transform.localRotation.eulerAngles.y + 180;
        float z = card.transform.localRotation.eulerAngles.z + 180;
        card.transform.localRotation = Quaternion.Euler(0, y, z);
        card.transform.localScale = new Vector3(.7f, .246f, .07f);
        Positions[CaseN - 1].InUse = true;
        Cards[CaseN - 1] = (card);

    }

    public void PlaceCardDown(ClueCard card, int CaseN)
    {
        // simply placing it in position one for now 
        card.transform.SetParent(Positions[CaseN - 1].transform);
        card.transform.position = Positions[CaseN - 1].transform.position;
        card.transform.rotation = Positions[CaseN - 1].transform.rotation;
        float y = card.transform.localRotation.eulerAngles.y;
        float z = card.transform.localRotation.eulerAngles.z + 180;
        card.transform.localRotation = Quaternion.Euler(0, y, z);
        card.transform.localScale = new Vector3(.7f, .246f, .07f);
        Positions[CaseN - 1].InUse = true;
        Cards[CaseN - 1] = (card);
    }

    public void SetCard(ClueCard card, int CaseN)
    {
        Cards[CaseN - 1] = card;
        card.transform.SetParent(Positions[CaseN - 1].transform);
        Positions[CaseN - 1].InUse = true;
       // Cards.Add(card);
    }


    public void PlaceCard(ClueCard card, int CaseN)
    {
        // simply placing it in position one for now 
        card.transform.SetParent(Positions[CaseN - 1].transform);
        card.transform.position = Positions[CaseN - 1].transform.position;
        card.transform.rotation = Positions[CaseN - 1].transform.rotation;
        float y = card.transform.localRotation.eulerAngles.y + 180;
        float z = card.transform.localRotation.eulerAngles.z + 180;
        card.transform.localRotation = Quaternion.Euler(0, y, z);
        card.transform.localScale = new Vector3(.7f, .246f, .07f);
        Positions[CaseN - 1].InUse = true;
        Cards[CaseN - 1] = (card);
    }

    public ClueCard FlipCard(int Case)
    {
        ClueCard card = (ClueCard)Cards[Case - 1];

        // currently flipping card when placing down 
        card.transform.SetParent(Positions[Case - 1].transform);
        card.transform.position = Positions[Case - 1].transform.position;
        card.transform.rotation = Positions[Case - 1].transform.rotation;
        float y = card.transform.localRotation.eulerAngles.y + 180;
        float z = card.transform.localRotation.eulerAngles.z + 180;
        card.transform.localRotation = Quaternion.Euler(0, y, z);
        card.transform.localScale = new Vector3(.7f, .246f, .07f);


        return card;
    }

    public void ClearCards()
    {
        foreach (ClueCard card in Cards)
        {
            RowAreaPosition RP = card.GetComponentInParent<RowAreaPosition>();
            if (RP == null) { Debug.Log("Card got into the wrong area"); }
            RP.InUse = false;
           // ClueCard card = RP.GetComponentInChildren<ClueCard>();

            if (FindObjectOfType<ClueDeck>() == null)
            {
                card.transform.SetParent(FindObjectOfType<M_ClueDeck>().transform);
            }
            else { card.transform.SetParent(FindObjectOfType<ClueDeck>().transform); }
           
            card.transform.position = FindObjectOfType<CardSpot>().transform.position;
            card.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
            card.UnfadeCard();

            Cards[RP.Case - 1] = null;
            //    Destroy(RP.GetComponentInChildren<ClueCard>().gameObject);
        }
       // Cards.Clear();
    }


}
