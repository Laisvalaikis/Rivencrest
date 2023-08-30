using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison
{
    public GameObject Poisoner;
    public ChunkData chunk;
    public int turnsLeft;
    public int poisonValue;
    
    public Poison(ChunkData chunk, int turnsleft, int poisonvalue)
    {
        this.chunk = chunk;
        turnsLeft = turnsleft;
        poisonValue = poisonvalue;
    }
}