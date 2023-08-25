using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BlessingInformation", order = 1)]
public class BlessingInformation : ScriptableObject
{
   public string blessingName;
   public int rarity;
   public string className;
   public string spellName;
   public string condition;
   [TextArea]
   public string description;



}
