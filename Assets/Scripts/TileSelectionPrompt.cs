using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileSelectionPrompt : MonoBehaviour {

    // Use this for initialization
    public GameObject TextPanel;
    public GameObject TextString;

	void Start () {
        TextPanel.gameObject.SetActive(false);
    }

    public void SetText(string textString)
    {
        TextPanel.SetActive(true);
        TextString.GetComponent<Text>().text = textString;
    }

    public void HideText()
    {
        TextPanel.SetActive(false);
    }
}
