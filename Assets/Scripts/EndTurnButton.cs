using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{

    // Use this for initialization
    Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.interactable = false;
    }

    public void EnableEndTurn()
    {
        button.interactable = true;
    }

    public void DisableEndTurn()
    {
        button.interactable = false;
    }
}
