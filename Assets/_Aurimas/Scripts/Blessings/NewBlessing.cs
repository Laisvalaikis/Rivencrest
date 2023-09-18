using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBlessing : ScriptableObject
{
    public string name;
    public int rarity;
    [TextArea]
    public string description;

    public virtual void UseBlessing()
    {
        
    }

}
