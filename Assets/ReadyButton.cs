using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyButton : MonoBehaviour {

    public bool isOn = false;
    public GameObject CheckMark;
    public LobbyPlayerUI LobbyPlayerUI;

	public void Toggle()
    {
        isOn = !isOn;
        if (isOn) { CheckMark.SetActive(true); }
        else { CheckMark.SetActive(false); }
        LobbyPlayerUI.ToggleChange(isOn);
    }

    public void SetValue(bool value)
    {
        isOn = value;
        if (isOn) { CheckMark.SetActive(true); }
        else { CheckMark.SetActive(false); }
    }
}
