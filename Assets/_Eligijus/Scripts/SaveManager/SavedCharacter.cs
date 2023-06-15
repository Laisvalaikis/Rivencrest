using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


    [Serializable]
    public class SavedCharacter : SavableCharacter
    {
        public GameObject prefab;
        //[HideInInspector] public int level;
        //[HideInInspector] public int XP;
        //[HideInInspector] public int XPToGain;
        //[HideInInspector] public string characterName;
        //[HideInInspector] public bool Dead;
        //[HideInInspector] public int abilityPointCount;
        //[HideInInspector] public string unlockedAbilities;
        //public List<string> blessings;
        //public int cost;

        //public string blessingString()
        //{
        //    string blessingsInOneString = "";
        //    foreach(string x in blessings)
        //    {
        //        blessingsInOneString += x;
        //        //nededa paskutinio ;
        //        if (x!=blessings[blessings.Count-1])
        //        blessingsInOneString += ";";
        //    }

        //    return blessingsInOneString;
        //}
        //private List<string> ConvertBlessingStringIntoList(string blessingString)
        //{
        //    List<string> convertedList = new List<string>();
        //    string[] parts = blessingString.Split(';');
        //    for(int i = 0; i < parts.Length; i++)
        //    {
        //        if (parts[i] != "")
        //        {
        //            convertedList.Add(parts[i]);
        //        }
        //    }
        //    return convertedList;
        //}
        public SavedCharacter() { }

        public SavedCharacter(GameObject prefab, int level, int xP, int xPToGain, bool dead, string characterName,
            int abilityPointCount, string unlockedAbilities, List<Blessing> blessings, int prefabIndex)
            : base(level, xP, xPToGain, dead, characterName, abilityPointCount, unlockedAbilities, blessings, prefabIndex)
        {
            this.prefab = prefab;
            //this.level = level;
            //this.XP = xP;
            //XPToGain = xPToGain;
            //this.Dead = Dead;
            //this.characterName = characterName;
            //this.abilityPointCount = abilityPointCount;
            //this.unlockedAbilities = unlockedAbilities;
            //this.blessings = ConvertBlessingStringIntoList(blessingString);
            //this.cost = 1000;
        }
        public SavedCharacter(SavedCharacter x) : base(x)
        {
            this.prefab = x.prefab;
        }

        public SavedCharacter(SavableCharacter x, GameObject prefab) : base(x)
        {
            this.prefab = prefab;
        }

        public string CharacterTableBlessingString()
        {
            string blessingsInOneString = "";
            for(int i = 0; i < blessings.Count; i++)
            {
                blessingsInOneString += blessings[i].blessingName;
                if (i != blessings.Count - 1) blessingsInOneString += "\n";
            }
            return blessingsInOneString;
        }
    }
