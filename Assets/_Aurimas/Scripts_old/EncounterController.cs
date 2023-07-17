using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class EncounterController : MonoBehaviour
{
    public List<string> encounterCategories;
    public TextMeshProUGUI missionName;
    public TextMeshProUGUI level;
    public TextMeshProUGUI category;
    public TextMeshProUGUI numOfEnemies;
    public TextMeshProUGUI missionInfo;
    public GameObject missionInformationGo;
    public Button embark;
    public Data _data;
    public GameProgress gameProgress;
    [SerializeField] public Map Map;

    private void Start()
    {
        ChangeSelectedEncounter(null);
    }

    public List<Encounter> Setup(List<Encounter> pastEncounters, ref bool generateNewEncounters, List<Encounter> alreadyGeneratedEncounters)
    {
        List<Encounter> generatedEncounters;
        if (generateNewEncounters)
        {
            GenerateEncounters(out generatedEncounters, pastEncounters);
            generateNewEncounters = false;
        }
        else generatedEncounters = alreadyGeneratedEncounters;
        ToggleEncounterButtons(generatedEncounters);
        return generatedEncounters;
    }

    private void AddAttributesToEncounter(Encounter encounter, GameObject map, string category, int level)
    {
        encounter.missionCategory = category;
        encounter.encounterLevel = level;
        encounter.mapName = map.name;
        encounter.enemyPool = map.GetComponent<Map>().suitableEnemies;
        encounter.allowDuplicates = map.GetComponent<Map>().allowDuplicates;
        encounter.numOfEnemies = map.GetComponent<Map>().numberOfEnemies;
        
        
    }
    
    private void GenerateEncounters(out List<Encounter> encounterListToPopulate, List<Encounter> pastEncounters)
    {
        encounterListToPopulate = new List<Encounter>();
       // var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();

        GameObject tutorialMap = gameProgress.GetComponent<MapSetup>().MapPrefabs.Find(x => x.GetComponent<Map>().name == "Tutorial");
        Encounter tutorialEncounter = new Encounter();
        AddAttributesToEncounter(tutorialEncounter, tutorialMap, "Forest", 1);
        encounterListToPopulate.Add(tutorialEncounter);
        if (pastEncounters.Find(x => x.missionCategory == "Forest" && x.encounterLevel == 1) != null)
        {
            GameObject merchantMap = gameProgress.GetComponent<MapSetup>().MapPrefabs.Find(x => x.GetComponent<Map>().name == "Merchant Ambush");
            Encounter merchantEncounter = new Encounter();
            AddAttributesToEncounter(merchantEncounter, merchantMap, "Forest", 2);
            encounterListToPopulate.Add(merchantEncounter);
        }


        //This part generates random levels and adds them to encounter list
        //It is commented out for now because we only want 2 levels for the demo
        //They are added manually in the code above (this will probably change for non-demo release)
/*        for (int i = 2; i <= 5; i++)
        {
            foreach (string category in encounterCategories)
            {
                if(i == 1 || pastEncounters.Find(x => x.missionCategory == category && x.encounterLevel == i - 1) != null)
                {
                    Encounter newEncounter = new Encounter();
                    newEncounter.missionCategory = category;
                    newEncounter.encounterLevel = i;
                    List<GameObject> suitableMaps = gameProgress.GetComponent<MapSetup>().MapPrefabs.FindAll(x => x.GetComponent<Map>().mapCategory == category && x.GetComponent<Map>().suitableLevels.Contains(i));
                    GameObject suitableMap = suitableMaps[Random.Range(0, suitableMaps.Count)];
                    newEncounter.mapName = suitableMap.name;
                    newEncounter.enemyPool = suitableMap.GetComponent<Map>().suitableEnemies;
                    newEncounter.allowDuplicates = suitableMap.GetComponent<Map>().allowDuplicates;
                    newEncounter.numOfEnemies = suitableMap.GetComponent<Map>().numberOfEnemies;
                    encounterListToPopulate.Add(newEncounter);
                }
            }
        }*/

    }

    private void ToggleEncounterButtons(List<Encounter> generatedEncounters)
    {
        foreach(Encounter encounter in generatedEncounters)
        {
            var button = transform.Find(encounter.missionCategory + "EncounterButtons").transform.Find("Level" + encounter.encounterLevel + "Encounter").gameObject;
            button.SetActive(true);
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                ChangeSelectedEncounter(encounter);
            });
            
        }
    }
    
    public void ChangeSelectedEncounter(Encounter encounter)
    {
        bool activate = encounter != _data.townData.selectedEncounter & encounter != null;
        // Debug.Log(townData.selectedEncounter.encounterLevel);
        _data.townData.selectedEncounter = activate ? encounter : null;
        _data.townData.selectedMission = activate ? encounter.mapName : "";
        if (activate)
        {

            missionName.text = _data.townData.selectedEncounter.mapName;
            level.text = _data.townData.selectedEncounter.encounterLevel.ToString();
            category.text = _data.townData.selectedEncounter.missionCategory;
            numOfEnemies.text = _data.townData.selectedEncounter.numOfEnemies.ToString();
            string missionText = "";
            if(_data.townData.selectedEncounter.mapName == "Merchant Ambush") 
            {
                missionText = "Merchant was ambushed by enemies. Defeat the enemies while keeping the merchant alive. The merchant will help you fight.";
            }
            else 
            {
                missionText = "Defeat the enemies to complete the mission.";
            }
            missionInfo.text = missionText;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        missionInformationGo.SetActive(activate);
        embark.interactable = (activate);
    }
}
