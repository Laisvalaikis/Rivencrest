using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AbilityText", order = 1)]
public class AbilityText : ScriptableObject
{
    [TextArea]
    public string abilityTitle;
    [TextArea]
    public string abilityDescription;
}
