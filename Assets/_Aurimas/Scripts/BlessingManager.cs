using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlessingManager : MonoBehaviour
{
    private PlayerInformationData _playerInformationData;
    private int counter = 0;
    public List<Blessing> BlessingList = new List<Blessing>();
    public List<GeneratedBlessing> GeneratedBlessingList = new List<GeneratedBlessing>();
    [SerializeField] private List<BlessingInformation> blessingInformations;
    [SerializeField] private List<GameObject> blessingTable;
    public Blessing blessing;
    public SavedCharacter character;
    //[SerializeField] private List<GameObject> characterXp;
    [SerializeField] private List<XPCard> _xpCard;
    //public TextAsset blessingsFile;
    public SaveData _saveData;
    private Data _data;
    public UnlockedAbilities unlockedAbilities;
    // Start is called before the first frame update
    void Start()
    {
        _data = Data.Instance;
        blessingInformations = new List<BlessingInformation>();
        //  MakeBlessingList(BlessingList);
        //transform.Find("ContinueButton").transform.Find("Text").GetComponent<Text>().text = "SKIP";
        //transform.Find("ContinueButton").GetComponent<Button>().interactable = false;
        GenerateBlessings(0);
        SetupBlessingCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [System.Serializable]
    public class GeneratedBlessing
    {
        public Blessing blessing;
        public SavedCharacter character;

        public GeneratedBlessing(Blessing blessing, SavedCharacter character)
        {
            this.blessing = blessing;
            this.character = character;
        }
    }
    public void RerollBlessings()
    {
        // MakeBlessingList(BlessingList);
        GenerateBlessings(0);
        SetupBlessingCards();
    }
    private void GenerateBlessings(int rarityLevel)
    {
        GeneratedBlessingList.Clear();
        //su while;
        for (int i = 0; i < 3; i++)
        {
            bool wasTargetFound = false;
            while (!wasTargetFound)
            {
                if (blessingInformations.Count < 1)
                {
                    wasTargetFound = true;
                    break;
                }
                //check rarity
                int randomBlessingIndex = Random.Range(0, blessingInformations.Count);
                blessingInformations[i] = blessingInformations[randomBlessingIndex];
                //List<SavedCharacter> BlessingTargets = PossibleBlessingTargets(randomBlessing);
                //jeigu ne tuscias
                // if (BlessingTargets.Count > 0)
                {
                    wasTargetFound = true;
                    // int randomTargetIndex = Random.Range(0, BlessingTargets.Count);
                    // SavedCharacter randomBlessingTarget = BlessingTargets[randomTargetIndex];
                    // GeneratedBlessing blessingToAdd = new GeneratedBlessing(blessingInformations, randomBlessingTarget);
                    // GeneratedBlessingList.Add(blessingToAdd);
                }
                // else
                {
                    //Debug.Log(randomBlessing.blessingName);
                    blessingInformations.Remove(blessingInformations[i]);
                }
                
            }
        }

    }
    private void SetupBlessingCards()
    {
        //var blessingCards = GameObject.Find("CanvasCamera").transform.Find("Blessings");
        for(int i = 0; i< blessingTable.Count; i++)
        {
            if(i < GeneratedBlessingList.Count)
            {
                blessingTable[i].SetActive(true);
                //var card = blessingCards.GetChild(i);
                blessingTable[i].GetComponent<Image>().color = GeneratedBlessingList[i].character.prefab.GetComponent<PlayerInformationData>().secondClassColor;
                blessingTable[i].GetComponent<Image>().sprite = GeneratedBlessingList[i].character.prefab.GetComponent<PlayerInformationData>().CharacterPortraitSprite;
                //icon
                blessingTable[i].GetComponent<Image>().color = GeneratedBlessingList[i].character.prefab.GetComponent<PlayerInformationData>().classColor;
                blessingTable[i].GetComponent<TextMeshProUGUI>().text = GeneratedBlessingList[i].character.characterName;
                blessingTable[i].GetComponent<TextMeshProUGUI>().text = GeneratedBlessingList[i].blessing.blessingName;
                blessingTable[i].GetComponent<TextMeshProUGUI>().text = GeneratedBlessingList[i].blessing.description;
            }
            //Out of bound of generated blessings
            else
            {
                blessingTable[i].SetActive(false);
            }
        }
    }
    private List<SavedCharacter> PossibleBlessingTargets(Blessing blessing)
    {
        List<SavedCharacter> targets = new List<SavedCharacter>();
        //assign characters who were on mission to list  
        var gameProgressCharacters = _data.Characters;
        var onLastMissionCharacterIndexes = _data.CharactersOnLastMission;
        for(int i = 0; i < onLastMissionCharacterIndexes.Count; i++)
        {
            targets.Add(gameProgressCharacters[onLastMissionCharacterIndexes[i]]);
        }
        //ar nera tarp jau sugeneruotu, kad nerodytu dvieju tu paciu blessing+veikejas
        foreach(GeneratedBlessing x in GeneratedBlessingList)
        {
            //jeigu blessing yra to pacio tipo kaip ir jau sugeneruota, tai negeneruos tu paciu kaip ir ta sugeneruota
            if (x.blessing == blessing && targets.Contains(x.character))
            {
                targets.Remove(x.character);
            }
        }
        //ar neturi tie like veikejai sito blessingo
        List<SavedCharacter> targetsCopy = new List<SavedCharacter>(targets);
        foreach (SavedCharacter x in targetsCopy)
        {
            if (x.blessings.Find(x => x.blessingName == blessing.blessingName) != null || (x.dead && blessing.condition!="Dead"))
            {
                targets.Remove(x);
            }         
        }
        //ar atitinka conditionus
        targetsCopy = new List<SavedCharacter>(targets);
        if (blessing.className != "-")
        {
            foreach(SavedCharacter x in targetsCopy)
            {
                //if meets class requirement
                if ((x.prefab.GetComponent<PlayerInformationData>().ClassName).ToUpper() != (blessing.className).ToUpper())
                {
                    targets.Remove(x);
                }
                //else check if has that spell
                // else if (x.prefab.GetComponent<ActionManager>().ActionScripts.Find(y => y.actionName == blessing.spellName) != null)
                // {
                //     int spellIndex = x.prefab.GetComponent<ActionManager>().ActionScripts.Find(y => y.actionName == blessing.spellName).AbilityIndex;
                //     //if(unlockedAbilities[spellIndex] == '0') //aurio komentaras reiks fixint cia
                //    // {
                //        // targets.Remove(x);
                //    // }
                // }
                
            }
        }
        if (blessing.condition == "Dead")
        {
            foreach (SavedCharacter x in targetsCopy)
            {
                if (!x.dead)
                {
                    targets.Remove(x);
                }
            }
        }
        return targets;
    }
    public void ClaimBlessing(int blessingIndex)
    {
        if(GeneratedBlessingList[blessingIndex].blessing.blessingName == "Revive")
        {
            GeneratedBlessingList[blessingIndex].character.dead = false;
        }
        else
        {
            GeneratedBlessingList[blessingIndex].character.blessings.Add(GeneratedBlessingList[blessingIndex].blessing);
        }
        
    }
    public void ContinueButton()
    {
        int randomBlessingNumber = Random.Range(0, 100);
        if (randomBlessingNumber >= 0 && GeneratedBlessingList.Count > 0)//!!!!
        { 
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            GameObject.Find("CanvasCamera").transform.Find("Blessings").gameObject.SetActive(true);
            GameObject.Find("CanvasCamera").transform.Find("OtherBlessingObjects").gameObject.SetActive(true);
        }
    }
    
}
