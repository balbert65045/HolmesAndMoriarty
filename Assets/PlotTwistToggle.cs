using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlotTwistToggle : MonoBehaviour {

    private LevelPropertyManager levelPropertyManager;
	// Use this for initialization
	void Start () {
        levelPropertyManager = FindObjectOfType<LevelPropertyManager>();
	}
	
    public void SetPlotTwist()
    {
        Toggle toggle = GetComponent<Toggle>();
        levelPropertyManager.SetPlotTwist(toggle.isOn);
    }
}
