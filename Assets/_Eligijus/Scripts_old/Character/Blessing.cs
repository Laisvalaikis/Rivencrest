﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Blessing
{
    public string blessingName;
    public int rarity;
    public string className;
    public string spellName;
    public string condition;
    public string description;
    //BlessingIcon???

    public Blessing(string blessingName, int rarity, string className, string spellName, string condition, string description)
    {
        this.blessingName = blessingName;
        this.rarity = rarity;
        this.className = className;
        this.spellName = spellName;
        this.condition = condition;
        this.description = description;
    }
}
