using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Debuff 
{
    public string debuffName;
    public List<GameObject> causedBy;
    public GameObject gameObjectValue;
    public GameObject intValue;
    public bool NegativeDebuff;//?
    public string animationTag;
    //Atskira klase AllAvailableDebuffs debuffmanageryje ar kur ten, kur galetume pazymet dar ir bool MovementDebuff
    //public Sprite icon;

    public Debuff(string debuffName, List<GameObject> causedBy, string animationTag = "")
    {
        this.debuffName = debuffName;
        this.causedBy = causedBy;
        this.animationTag = animationTag;
    }
    public Debuff(string debuffName, GameObject causer, string animationTag = "")
    {
        this.debuffName = debuffName;
        this.causedBy = new List<GameObject>();
        this.causedBy.Add(causer);
        this.animationTag = animationTag;
    }
}
