using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Ability
{

    public AbilityText abilityText;
    public bool enabled = true;
    public Sprite AbilityImage;
    public BaseAction Action;
}