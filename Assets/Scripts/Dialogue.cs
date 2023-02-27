using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;
    public int fontS;
    public Sprite imagem;
    [TextArea(5,10)] public string[] sentences;

}
