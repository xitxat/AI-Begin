using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Sceene display, Action + user #

// PLACEMENT: Canvas > World States
// Drag WS TEXT Component into the STATES slot


public class UpdateWorld : MonoBehaviour
{
    public Text states;

    void LateUpdate()
    {
        Dictionary<string,int> worldstates = GWorld.Instance.GetWorld().GetStates();
        states.text  = "";
        foreach (KeyValuePair<string,int> s in worldstates)
        {
            // + update the int val.
            states.text += s.Key + ", " + s.Value + "\n";

        }
    }
}
