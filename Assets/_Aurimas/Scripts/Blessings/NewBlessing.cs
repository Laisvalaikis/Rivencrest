using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBlessing : ScriptableObject
{
    public string name;
    public int rarity;
    [TextArea]
    public string description;

    public virtual void UseBlessing(Vector3 position)
    {
        
    }
    public virtual void OnTurnStart()
    {
        
    }
    public virtual void OnTurnEnd()
    {

    }

}
