using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerInformation", order = 1)]
public class PlayerInformationData : ScriptableObject
{
    [Header("Character")]
    public string ClassName;
    public int MaxHealth = 100;
    public int critChance = 5;
    public int accuracy = 100;
    public int dodgeChance = 20;
    public string role;

    [Header("Color")] 
    public Color classColor;
    public Color secondClassColor;
    public Color textColor;
    public Color backgroundColor;
    [Header("Images")] 
    public Sprite CharacterPortraitSprite;
    public Sprite CharacterSplashArt;//For character table
    public Sprite characterSprite;
    public Sprite CroppedSplashArt;
    public List<Sprite> abilitySprites;
    public List<AbilityData> abilities;
  
    public List<Blessing> BlessingsAndCurses = new List<Blessing>();
}



[System.Serializable]
public class AbilityData
{
    public Sprite sprite;
    public AbilityAction abilityAction;
}

