using System.Collections.Generic;
using UnityEngine;
using System;


    [Serializable]
    public class TownData
    {
        //Slot info
        public string teamColor;
        public string slotName;
        public bool singlePlayer;//cia sita gal kazkaip istrint reiks ¯\_(ツ)_/¯
        //Town info
        public int difficultyLevel;
        public int townGold;
        public int day;
        public bool newGame;
        public List<SavableCharacter> characters;
        public string townHall;
        public List<SavableCharacter> rcCharacters;
        public bool createNewRCcharacters;
        //Mission/Encounter info
        public List<int> charactersOnLastMission;
        public bool wasLastMissionSuccessful;
        public bool wereCharactersOnAMission;
        public string selectedMission;
        public List<int> enemies;
        public bool allowEnemySelection;
        public bool allowDuplicates;
        public Encounter selectedEncounter;
        public List<Encounter> pastEncounters;
        public bool generateNewEncounters;
        public List<Encounter> generatedEncounters;
        public GameSettings gameSettings;
        //Legacy
        //public int[] characters;
        //public int[] characterLevels;
        //public int[] characterXP;
        //public int[] characterXPToGain;
        //public bool[] isCharacterDead;
        //public string[] characterNames;
        //public int[] abilityPointCounts;
        //public string[] unlockedAbilities;
        //public string[] characterBlessingStrings;
        //public List<int> RCcharacters;
        //public List<int> RCcharacterLevels;
        //public string[] RCcharacterNames;

        public TownData() { }

        public TownData(int difficultyLevel, int townGold, int day, List<SavedCharacter> characters, List<int> charactersOnLastMission,
            bool wasLastMissionSuccessful, bool newGame, bool singlePlayer, string selectedMission, string townHall,
            List<SavedCharacter> rcCharacters, List<int> enemies, bool allowEnemySelection, bool allowDuplicates, string teamColor,
            string slotName, Encounter selectedEncounter, List<Encounter> pastEncounters, bool generateNewEncounters, List<Encounter> generatedEncounters,
            GameSettings gameSettings)
        {
            this.difficultyLevel = difficultyLevel;
            this.townGold = townGold;
            this.day = day;
            //int characterCount = characters.Count;
            this.characters = new List<SavableCharacter>();
            //this.charactersOnLastMission = new int[charactersOnLastMission.Count];
            this.wasLastMissionSuccessful = wasLastMissionSuccessful;
            //this.characterLevels = new int[characterCount];
            //this.characterXP = new int[characterCount];
            //this.characterXPToGain = new int[characterCount];
            //this.isCharacterDead = new bool[characterCount];
            //this.characterNames = new string[characterCount];
            //this.abilityPointCounts = new int[characterCount];
            //this.unlockedAbilities = new string[characterCount];
            //this.characterBlessingStrings = new string[characterCount];
            //for (int i = 0; i < characters.Count; i++)
            //{
            //    List<SavedCharacter> allCharacters = GameObject.Find("GameProgress").gameObject.GetComponent<GameProgress>().AllAvailableCharacters;
            //    for (int j = 0; j < allCharacters.Count; j++)
            //    {
            //        if (characters[i].prefab == allCharacters[j].prefab)
            //        {
            //            this.characters[i] = j; //nustato skaiciuka, kurioj listo vietoj yra prefabas
            //        }
            //    }
            //    characterLevels[i] = characters[i].level;
            //    characterXP[i] = characters[i].XP;
            //    characterXPToGain[i] = characters[i].XPToGain;
            //    isCharacterDead[i] = characters[i].Dead;
            //    characterNames[i] = characters[i].characterName;
            //    abilityPointCounts[i] = characters[i].abilityPointCount;
            //    unlockedAbilities[i] = characters[i].unlockedAbilities;
            //    characterBlessingStrings[i] = characters[i].blessingString();
            //}
            this.characters = new List<SavableCharacter>(characters);
            this.charactersOnLastMission = new List<int>(charactersOnLastMission);
            //for (int i = 0; i < this.charactersOnLastMission.Length; i++)
            //{
            //    this.charactersOnLastMission[i] = charactersOnLastMission[i];
            //}
            this.wereCharactersOnAMission = charactersOnLastMission.Count > 0;
            //if (charactersOnLastMission.Count > 0)
            //{
            //    this.wereCharactersOnAMission = true;
            //}
            //else
            //{
            //    wereCharactersOnAMission = false;
            //}
            this.newGame = newGame;
            this.singlePlayer = singlePlayer;
            this.selectedMission = selectedMission;
            this.townHall = townHall;
            //RC
            //if (RCcharacters != null)
            //{
            //    int RCcharacterCount = RCcharacters.Count;
            //    this.RCcharacters = new int[RCcharacterCount];
            //    this.RCcharacterNames = new string[RCcharacterCount];
            //    this.RCcharacterLevels = new int[RCcharacterCount];
            //    for (int i = 0; i < RCcharacters.Count; i++)
            //    {
            //        List<SavedCharacter> allCharacters = GameObject.Find("GameProgress").gameObject.GetComponent<GameProgress>().AllAvailableCharacters;
            //        for (int j = 0; j < allCharacters.Count; j++)
            //        {
            //            if (RCcharacters[i].prefab == allCharacters[j].prefab)
            //            {
            //                this.RCcharacters[i] = j; //nustato skaiciuka, kurioj listo vietoj yra prefabas
            //            }
            //        }
            //        RCcharacterLevels[i] = RCcharacters[i].level;
            //        RCcharacterNames[i] = RCcharacters[i].characterName;
            //    }
            //}
            if(rcCharacters != null)
                this.rcCharacters = new List<SavableCharacter>(rcCharacters);
            createNewRCcharacters = rcCharacters == null;
            this.enemies = new List<int>(enemies);
            this.allowEnemySelection = allowEnemySelection;
            this.allowDuplicates = allowDuplicates;
            this.teamColor = teamColor;
            this.slotName = slotName;
            this.selectedEncounter = selectedEncounter;
            this.pastEncounters = pastEncounters;
            this.generateNewEncounters = generateNewEncounters;
            this.generatedEncounters = generatedEncounters;
            this.gameSettings = gameSettings;
        }

        public static TownData NewGameData(string color, int difficulty, string slotName)
        {
            return new TownData
            {
                difficultyLevel = difficulty,
                townGold = 3500,
                day = 1,
                characters = { },
                charactersOnLastMission = { },
                wasLastMissionSuccessful = false,
                wereCharactersOnAMission = false,
                //characterLevels = { },
                //characterXP = { },
                //characterXPToGain = { },
                //isCharacterDead = { },
                //characterNames = { },
                //abilityPointCounts = { },
                //unlockedAbilities = { },
                //characterBlessingStrings = { },
                newGame = true,
                singlePlayer = false,
                selectedMission = "",
                townHall = "000000",
                rcCharacters = { },
                //RCcharacters = { },
                //RCcharacterLevels = { },
                //RCcharacterNames = { },
                createNewRCcharacters = false,
                enemies = { },
                allowEnemySelection = false,
                allowDuplicates = false,
                teamColor = color,
                slotName = slotName,
                selectedEncounter = new Encounter(),
                pastEncounters = new List<Encounter>(),
                generateNewEncounters = true,
                generatedEncounters = new List<Encounter>(),
                gameSettings = new GameSettings()
            };
        }
    }

    [Serializable]
    public class SavableCharacter
    {
        public int level;
        public int xP;
        public int xPToGain;
        public string characterName;
        public bool dead;
        public int abilityPointCount;
        public string unlockedAbilities;
        public List<Blessing> blessings;
        public int cost;
        public int prefabIndex;

        public SavableCharacter() { }

        public SavableCharacter(int level, int xP, int xPToGain, bool dead, string characterName,
            int abilityPointCount, string unlockedAbilities, List<Blessing> blessings, int prefabIndex)
        {
            this.level = level;
            this.xP = xP;
            this.xPToGain = xPToGain;
            this.dead = dead;
            this.characterName = characterName;
            this.abilityPointCount = abilityPointCount;
            this.unlockedAbilities = unlockedAbilities;
            this.blessings = new List<Blessing>(blessings);
            this.cost = 1000;
            this.prefabIndex = prefabIndex;
        }

        public SavableCharacter(SavableCharacter x)
        {
            this.level = x.level;
            this.xP = x.xP;
            this.xPToGain = x.xPToGain;
            this.dead = x.dead;
            this.characterName = x.characterName;
            this.abilityPointCount = x.abilityPointCount;
            this.unlockedAbilities = x.unlockedAbilities;
            this.blessings = this.blessings = new List<Blessing>(x.blessings);
            this.cost = x.cost;
            this.prefabIndex = x.prefabIndex;
        }
    }

    [Serializable]
    public class GameSettings
    {
        public bool attackHelper;
        public float masterVolume;
        public bool mute;

        public GameSettings()
        {
            attackHelper = false;
            masterVolume = 1f;
            mute = false;
        }
    }
