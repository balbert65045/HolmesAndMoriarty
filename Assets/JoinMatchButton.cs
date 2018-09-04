using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinMatchButton : MonoBehaviour {

    public int MatchID = 0;

    public void Join()
    {
        FindObjectOfType<matchScreen>().JoinMatch(MatchID);
    }
}
