using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICardArea : MonoBehaviour {

    // Use this for initialization
    public CardArea.Row ThisRow;
    public GamePlayer PlayerAttached;
    public RowAreaPosition[] Positions;


    void Start () {
        Positions = GetComponentsInChildren<RowAreaPosition>();
        for (int i = 0; i < Positions.Length; i++)
        {
            if (Positions[i].Case == 1)
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

    public ClueCard GetCard(int Pos)
    {
        return Positions[Pos - 1].GetComponent<RowAreaPosition>().GetComponentInChildren<ClueCard>();
    }

    public void PlaceCard(ClueCard card, int CaseN)
    {
        // simply placing it in position one for now 
        card.transform.SetParent(Positions[CaseN - 1].transform);
        card.transform.position = Positions[CaseN - 1].transform.position;
        card.transform.rotation = Positions[CaseN - 1].transform.rotation;
        // use this for verticle setup
        card.transform.localScale = new Vector3(.7f, .246f, .07f);
        Positions[CaseN - 1].InUse = true;

    }

    public ClueCard FlipCard(int Case)
    {
        ClueCard card = Positions[Case - 1].GetComponentInChildren<ClueCard>();
        float y = card.transform.localRotation.eulerAngles.y + 180;
        float z = card.transform.localRotation.eulerAngles.z + 180;
        card.transform.localRotation = Quaternion.Euler(0, y, z);
        return card;
    }

    public void ClearCards()
    {
        foreach (RowAreaPosition RP in Positions)
        {
            RP.InUse = false;
            ClueCard card = RP.GetComponentInChildren<ClueCard>();
            card.transform.SetParent(FindObjectOfType<ClueDeck>().transform);
            card.transform.position = FindObjectOfType<CardSpot>().transform.position;
            card.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
            card.UnfadeCard();
            //    Destroy(RP.GetComponentInChildren<ClueCard>().gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
