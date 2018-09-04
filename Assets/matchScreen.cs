using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class matchScreen : MonoBehaviour {

    MyNetworkHud Hud;

    public GameObject MatchPrefab;
    public GameObject ContentArea;
    public float GapBetweenMatches = 10f;


    private List<MatchInfoSnapshot> Matches;

    public void SetMatches(List<MatchInfoSnapshot> MatchesAvailable)
    {
        Matches = MatchesAvailable;
        Debug.Log(Matches.Count);
        for (int i = 0; i < Matches.Count; i++)
        {
            GameObject Match = Instantiate(MatchPrefab, ContentArea.transform);

            Match.GetComponent<RectTransform>().Translate(new Vector3(0, -GapBetweenMatches * i, 0));
            Match.GetComponentInChildren<Text>().text = Matches[i].name;
            Match.GetComponentInChildren<JoinMatchButton>().MatchID = i;
        }
    }

    public void JoinMatch(int matchID)
    {
        MatchInfoSnapshot match = Matches[matchID];
        Hud.JoinMatch(match);
    }

    // Use this for initialization
    void Start () {
        Hud = FindObjectOfType<MyNetworkHud>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
