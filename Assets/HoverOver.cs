using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoverOver : MonoBehaviour
{
    //GameObject Obj;
    TextMeshProUGUI Text;
    string OriginalText;

    private void Start() {
        //Obj = GameObject.Find("Start");
        Text = gameObject.GetComponent<TextMeshProUGUI>();
        OriginalText = Text.text;
    }
    public void Enter()
     {
        Text.SetText("> "+OriginalText);
     }

    public void Exit()
    {
        Text.SetText(OriginalText);
    }
}
