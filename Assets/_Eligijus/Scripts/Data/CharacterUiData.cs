using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterUiData", menuName = "ScriptableObjects/CharacterUIData", order = 1)]
public class CharacterUiData : ScriptableObject
{
    [Header("Color")] 
    public Color classColor;
    public Color secondClassColor;
    public Color textColor;
    public Color backgroundColor;
    [Header("Images")] 
    public Sprite characterSprite;
    public List<Sprite> abilitySprites;
    public List<AbilityData> abilities;

}

[System.Serializable]
public class AbilityData
{
    public Sprite sprite;
    public AbilityAction abilityAction;
}
