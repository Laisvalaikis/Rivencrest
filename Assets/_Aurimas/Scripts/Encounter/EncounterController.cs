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
    public MapSetup mapSetup;
    public List<Button> encounterButton;
    public List<EncounterButton> encounterSelections;
    public Encounter selectedEncounter { get; private set; }
    private Data _data;

    private void Start()
    {
        _data = Data.Instance;
        _data.townData.generatedEncounters = Setup();
        ChangeSelectedEncounter(null);
    }

    public List<Encounter> Setup()
    {
        List<Encounter> generatedEncounters;
        if (_data.townData.generateNewEncounters)
        {
            GenerateEncounters(out generatedEncounters);
            _data.townData.generateNewEncounters = false;
        }
        else generatedEncounters = _data.townData.generatedEncounters;
        ToggleEncounterButtons(generatedEncounters);
        return generatedEncounters;
    }

    public void DisableAllButtons()
    {
        for (int i = 0; i < encounterButton.Count; i++)
        {
            encounterButton[i].interactable = false;
        }
    }

    public void EnableAllButtons()
    {
        for (int i = 0; i < encounterButton.Count; i++)
        {
            encounterButton[i].interactable = true;
        }
    }

    private void AddAttributesToEncounter(Encounter encounter, MapData map, string category, int level)
    {
        encounter.missionCategory = category;
        encounter.encounterLevel = level;
        encounter.mapName = map.name;
        encounter.enemyPool = map.suitableEnemies;
        encounter.allowDuplicates = map.allowDuplicates;
        encounter.numOfEnemies = map.numberOfEnemies;
        
        
    }
    
    private void GenerateEncounters(out List<Encounter> encounterListToPopulate)
    {
       encounterListToPopulate = new List<Encounter>();
       // // var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
       //  MapData tutorialMap = mapSetup.mapDatas["Tutorial"];
       //  Encounter tutorialEncounter = new Encounter();
       //  AddAttributesToEncounter(tutorialEncounter, tutorialMap, "Forest", 1);
       //  encounterListToPopulate.Add(tutorialEncounter);
       //  if (pastEncounters.Find(x => x.missionCategory == "Forest" && x.encounterLevel == 1) != null)
       //  {
       //      MapData merchantMap = mapSetup.mapDatas["MerchantAmbush"];
       //      Encounter merchantEncounter = new Encounter();
       //      AddAttributesToEncounter(merchantEncounter, merchantMap, "Forest", 2);
       //      encounterListToPopulate.Add(merchantEncounter);
       //  }


        //This part generates random levels and adds them to encounter list
        //It is commented out for now because we only want 2 levels for the demo
        //They are added manually in the code above (this will probably change for non-demo release)
        for (int i = 3; i <= 5; i++)
        {
            foreach (string category in encounterCategories)
            {
                // if(i == 1 || _data.townData.pastEncounters.Find(x => x.missionCategory == category && x.encounterLevel == i - 1) != null)
                // {
                    Encounter newEncounter = new Encounter();
                    newEncounter.missionCategory = category;
                    newEncounter.encounterLevel = i;
                    List<MapData> suitableMaps = new List<MapData>();
                    foreach (string key in mapSetup.mapDatas.Keys)
                    {
                        if (mapSetup.mapDatas[key].mapCategory == category && mapSetup.mapDatas[key].suitableLevels.Contains(1))
                        {
                            suitableMaps.Add(mapSetup.mapDatas[key]);
                        }
                    }
                    MapData suitableMap = suitableMaps[Random.Range(0, suitableMaps.Count)];
                    newEncounter.mapName = suitableMap.name;
                    newEncounter.enemyPool = suitableMap.suitableEnemies;
                    newEncounter.allowDuplicates = suitableMap.allowDuplicates;
                    newEncounter.numOfEnemies = suitableMap.numberOfEnemies;
                    newEncounter.missionText = suitableMap.informationText;
                    encounterListToPopulate.Add(newEncounter);
                // }
            }
        }

    }
    
    private void ToggleEncounterButtons(List<Encounter> generatedEncounters)
    {
        for (int i = 0; i < encounterSelections.Count; i++)
        {
            encounterSelections[i].AddEncounter(generatedEncounters[i]); 
        }
    }

    public void ChangeSelectedEncounter(Encounter encounter)
    {
        bool activate = encounter != _data.townData.selectedEncounter && encounter != null;
        
        _data.townData.selectedEncounter = activate ? encounter : null;
        _data.townData.selectedMission = activate ? encounter.mapName : "";
        if (activate)
        {
            missionName.text = _data.townData.selectedEncounter.mapName;
            level.text = _data.townData.selectedEncounter.encounterLevel.ToString();
            category.text = _data.townData.selectedEncounter.missionCategory;
            numOfEnemies.text = _data.townData.selectedEncounter.numOfEnemies.ToString();
            missionInfo.text = _data.townData.selectedEncounter.missionText;
        }
        selectedEncounter = encounter;
    }
    
}
