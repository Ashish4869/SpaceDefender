using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A class that holds all the diaouge in the game
/// </summary>
[System.Serializable]
public class Dialouge
{ 
    public string NPCName;

    [TextArea(3,10)]
    public string[] sentences;
}
