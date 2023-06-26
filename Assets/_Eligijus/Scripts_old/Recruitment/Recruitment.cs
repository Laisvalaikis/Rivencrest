using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Recruitment : MonoBehaviour
{
    public List<SavedCharacter> CharactersInShop = null;
    public int AttractedCharactersCount;
    private int CharacterLevelChar = '0';
    private List<string> NamesM = new List<string>();
    private List<string> NamesW = new List<string>();
    public TextAsset NamesMFile;
    public TextAsset NamesWFile;
    public Data _data;
    public void RecruitmentStart()
    {
        //Debug.Log("Recruitment start");
        //System.IO.File.WriteAllText(Path.Combine(Application.dataPath, "Assets", "Text") + "/NM.log", NamesMFile.text);
        //System.IO.File.WriteAllText(Path.Combine(Application.dataPath, "Assets", "Text") + "/NW.log", NamesWFile.text);
        ReadString(NamesM, NamesMFile);
        ReadString(NamesW, NamesWFile);
        if (_data.townData.day > 1)
        {
            //Debug.Log("Day is bigger than 1");
            AttractedCharactersCount = 2;
            char townHallChar = _data.townData.townHall[1];
            CharacterLevelChar = _data.townData.townHall[4];
            if (townHallChar == '1')
            {
                AttractedCharactersCount = 3;
            }
            else if(townHallChar == '2')
            {
                AttractedCharactersCount = 4;
            }
            else if (townHallChar == '3')
            {
                AttractedCharactersCount = 5;
            }
        }
        if(CharactersInShop == null)
        {
            //Debug.Log("Characters in shop is null, creating characters in shop");
            CharactersInShop = new List<SavedCharacter>();
            CreateCharactersInShop();
        }
        UpdateButtons();
    }

    public void ReadString(List<string> stringList, TextAsset namesFile)
    {
        stringList.Clear();
        using (var file = new StringReader(namesFile.text))
        {
            string line;
            while ((line = file.ReadLine()) != null)
            {
                stringList.Add(line);
            }
        }
    }
    private void CreateCharactersInShop()
    {
        List<SavedCharacter> AllCharactersCopy = new List<SavedCharacter>(_data.AllAvailableCharacters);
        CharactersInShop.Clear();
        if(NamesW.Count <= 8)
        {
            ReadString(NamesW, NamesWFile);
        }
        if (NamesM.Count <= 8)
        {
            ReadString(NamesM, NamesMFile);
        }
        for (int i = 0; i < AttractedCharactersCount; i++)
        {
            int randomIndex = Random.Range(0, AllCharactersCopy.Count);
            SavedCharacter characterToAdd = new SavedCharacter(AllCharactersCopy[randomIndex]);
            characterToAdd.level = 1;//
            characterToAdd.xP = 0;//
            AllCharactersCopy.RemoveAll(x => x.prefab == characterToAdd.prefab);
            //
            List<string> NameList;
            if (characterToAdd.prefab.GetComponent<PlayerInformation>().ClassName == "ASSASSIN" ||
                characterToAdd.prefab.GetComponent<PlayerInformation>().ClassName == "ENCHANTRESS" ||
                characterToAdd.prefab.GetComponent<PlayerInformation>().ClassName == "SORCERESS" ||
                characterToAdd.prefab.GetComponent<PlayerInformation>().ClassName == "HUNTRESS")
            {
                NameList = NamesW;
            }
            else {
                NameList = NamesM;
            }
            int randomIndex2 = Random.Range(0, NameList.Count);
            characterToAdd.characterName = NameList[randomIndex2].ToUpper();
            NameList.RemoveAt(randomIndex2);
            //
            characterToAdd.abilityPointCount = 1;
            characterToAdd.unlockedAbilities = "0000";
            //
            if (CharacterLevelChar == '1')
            {
                characterToAdd.level = 2;
                characterToAdd.abilityPointCount = 2;
            }
            //
            CharactersInShop.Add(characterToAdd);
        }
    }
    public void UpdateButtons()
    {
        UpdateRerollButton();
        Transform characters = transform.Find("Characters").transform;
        for (int i = 0; i < characters.childCount; i++)
        {
            if (i < CharactersInShop.Count)
            {
                characters.GetChild(i).gameObject.GetComponent<RecruitButton>().character = CharactersInShop[i];
            }
            else
            {
                characters.GetChild(i).gameObject.GetComponent<RecruitButton>().character = null;
            }
            characters.GetChild(i).gameObject.GetComponent<RecruitButton>().UpdateRecruitButton();
        }
    }
    private void UpdateRerollButton()
    {
        if (_data.townData.townHall != null)
        {
            char townHallChar = _data.townData.townHall[3];
            transform.Find("Reroll").GetComponent<Button>().interactable = (townHallChar == '1');
        }
    }
    public void Reroll()
    {
        AttractedCharactersCount = CharactersInShop.Count;
        CreateCharactersInShop();
        UpdateButtons();
    }
    public void InspectCharacterInShop(int index)
    {
        var table = transform.Find("CharacterInShopTable");
        var character = CharactersInShop[index].prefab.GetComponent<PlayerInformation>();

        table.gameObject.SetActive(true);

        table.Find("CharacterArt").GetComponent<Image>().sprite = character.CroppedSplashArt;
        table.Find("TableBorder").GetComponent<Image>().color = character.ClassColor2;
        table.Find("Info").Find("ClassName").GetComponent<Text>().text = character.ClassName;
        table.Find("Info").Find("ClassName").GetComponent<Text>().color = character.ClassColor;
        table.Find("Info").Find("Role").GetComponent<Text>().text = character.role;
        table.Find("Info").Find("Role").GetComponent<Text>().color = character.ClassColor;
        table.Find("Info").Find("Role").Find("DAMAGE").gameObject.SetActive(character.role == "DAMAGE");
        table.Find("Info").Find("Role").Find("TANK").gameObject.SetActive(character.role == "TANK");
        table.Find("Info").Find("Role").Find("SUPPORT").gameObject.SetActive(character.role == "SUPPORT");
        table.Find("Info").Find("MaxHP").GetComponent<Text>().text = character.MaxHealth + 4 + " HP";
        table.Find("Info").Find("MaxHP").GetComponent<Text>().color = character.ClassColor;
        bool melee = character.GetComponent<PlayerAttack>().AttackRange == 1;
        table.Find("Info").Find("Range").GetComponent<Text>().text = melee ? "MELEE" : "RANGED";
        table.Find("Info").Find("Range").GetComponent<Text>().color = character.ClassColor;
        table.Find("Info").Find("Range").Find("1").gameObject.SetActive(melee);
        table.Find("Info").Find("Range").Find("2").gameObject.SetActive(!melee);
        table.Find("Info").Find("Level").GetComponent<Text>().text = "LEVEL " + CharactersInShop[index].level;
        table.Find("Info").Find("Level").GetComponent<Text>().color = character.ClassColor;
        for (int j = 0; j < table.Find("Abilities").transform.childCount; j++)
        {
            table.Find("Abilities").GetChild(j).gameObject.SetActive(true);
            table.Find("Abilities").GetChild(j).Find("Color").GetComponent<Image>().sprite = character.GetComponent<ActionManager>().AbilityBackground;
            table.Find("Abilities").GetChild(j).Find("AbilityIcon").GetComponent<Image>().color = character.ClassColor;
            if (character.GetComponent<ActionManager>().FindActionByIndex(j) != null)
            {
                table.Find("Abilities").GetChild(j).Find("AbilityIcon").GetComponent<Image>().sprite = character.GetComponent<ActionManager>().FindActionByIndex(j).AbilityIcon;
            }
            else
            {
                table.Find("Abilities").GetChild(j).gameObject.SetActive(false);
            }
            var abilityName = character.GetComponent<ActionManager>().FindActionByIndex(j).actionName;
            table.Find("Abilities").GetChild(j).GetComponent<Button>().onClick.RemoveAllListeners();
            table.Find("Abilities").GetChild(j).GetComponent<Button>().onClick.AddListener(() =>
            {
                GameObject.Find("HelpTableController").GetComponent<HelpTableController>().EnableTableByName(abilityName, CharactersInShop[index]);
            });
        }
    }
}
