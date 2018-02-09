using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardArea : MonoBehaviour {

    // Use this for initialization
    public enum Row { Clue, Crime }
    public Row ThisRow;

    public RowAreaPosition[] Positions;

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

    public bool CheckForAvailableSpace(int CaseN)
    {
        if (!Positions[CaseN - 1].InUse) { return true; }
        return false;
    }

    public void PlaceCard(Card card, int CaseN)
    {
        // simply placing it in position one for now 
        card.transform.SetParent(Positions[CaseN - 1].transform);
        card.transform.position = Positions[CaseN - 1].transform.position;
        card.transform.rotation = Positions[CaseN - 1].transform.rotation;
        Positions[CaseN - 1].InUse = true;

    }

    public Card FlipCard(int Case)
    {
        Card card = Positions[Case - 1].GetComponentInChildren<Card>();
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
            Destroy(RP.GetComponentInChildren<Card>().gameObject);
        }
    }


}
