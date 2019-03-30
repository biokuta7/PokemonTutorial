using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    
    [TextArea(3, 5)]
    public string[] sentences;

    public Dialogue(string[] _sentences)
    {
        sentences = _sentences;
    }

    public Dialogue(string _sentence)
    {
        sentences = new string[1] { _sentence };
    }


}
