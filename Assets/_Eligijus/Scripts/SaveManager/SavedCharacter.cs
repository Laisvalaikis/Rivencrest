using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


    [Serializable]
    public class SavedCharacter : SavableCharacter
    {
        public GameObject prefab;
        public int characterIndex;
        public SavedCharacter() { }

        public SavedCharacter(GameObject prefab, int level, int xP, int xPToGain, bool dead, string characterName,
            int abilityPointCount, string unlockedAbilities, List<Blessing> blessings, int prefabIndex, PlayerInformationData playerInformation)
            : base(level, xP, xPToGain, dead, characterName, abilityPointCount, unlockedAbilities, blessings, prefabIndex, playerInformation)
        {
            this.prefab = prefab;
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
