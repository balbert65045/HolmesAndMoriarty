using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpToggle : MonoBehaviour {

    public MeshRenderer Clue1;
    public MeshRenderer Clue2;
    public MeshRenderer Crime1;
    public MeshRenderer Crime2;

    bool HelpOn = true;

    public void ToggleHelp()
    {
        HelpOn = !HelpOn;
        if (HelpOn)
        {
            Clue1.enabled = true;
            Clue2.enabled = true;
            Crime1.enabled = true;
            Crime2.enabled = true;
        }
        else
        {
            Clue1.enabled = false;
            Clue2.enabled = false;
            Crime1.enabled = false;
            Crime2.enabled = false;
        }
    }

}
