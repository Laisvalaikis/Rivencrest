using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSetup : MonoBehaviour
{
    [HideInInspector] public string MapName;
    public List<GameObject> MapPrefabs;
    [SerializeField] private List<MapData> _mapDatas;
    public Dictionary<string, MapData> mapDatas;
    public GameObject gameInformation;
    public GameObject coordinatesGameObject;
    public Map mapInformation;
    public MapData map;

    private void Start()
    {
        mapDatas = new Dictionary<string, MapData>();
        for (int i = 0; i < _mapDatas.Count; i++)
        {
            mapDatas.Add(_mapDatas[i].name, _mapDatas[i]);
        }
        
    }
    //Aurio kolkas
    // public void SetupAMap() 
    // {
    //     //GameObject gameInformation = GameObject.Find("GameInformation");
    //     GameObject MapPrefab = GetSelectedMap();
    //     
    //     if (MapPrefab != null)
    //     {
    //         List<GameObject> NpcTeam = mapInformation.NpcTeam;
    //         //coordinates
    //         coordinatesGameObject = MapPrefab.transform.gameObject;
    //         var teamSettings = gameInformation.GetComponent<PlayerTeams>();
    //         teamSettings.allCharacterList.teams[1].coordinates.Clear();
    //         for (int i = 0; i < 3; i++) 
    //         {
    //             teamSettings.allCharacterList.teams[0].coordinates[i] = coordinatesGameObject.transform;
    //         }
    //         for(int i = 0; i < coordinatesGameObject.transform.childCount; i++)
    //         {
    //             teamSettings.allCharacterList.teams[1].coordinates.Add(coordinatesGameObject.GetChild(1).GetChild(i));
    //         }
    //         //NPC team spawning
    //         if (mapInformation.NpcTeam.Count == 0)
    //         {
    //             teamSettings.allCharacterList.teams.RemoveAt(2);
    //         }
    //         else
    //         {
    //             for (int i = 0; i < NpcTeam.Count; i++)
    //             {
    //                 teamSettings.allCharacterList.teams[2].coordinates.Add(coordinatesGameObject.GetChild(2).GetChild(i));
    //                 teamSettings.allCharacterList.teams[2].characters.Add (NpcTeam[i]);
    //             }
    //         }
    //         //AI destinations
    //         var destinationsGameObject = MapPrefab.transform.Find("AIDestinations");
    //         var AIsettings = gameInformation.GetComponent<AIManager>();
    //         List<GameObject> destinations = new List<GameObject>();
    //         for (int i = 0; i < destinationsGameObject.childCount; i++) 
    //         {
    //             destinations.Add(destinationsGameObject.GetChild(i).gameObject);
    //         }
    //         AIsettings.AIDestinations = destinations;
    //     }
    // }
    
    public void SetupAMap() 
    {
        //GameObject gameInformation = GameObject.Find("GameInformation");
        GameObject MapPrefab = GetSelectedMap();
        
        if (MapPrefab != null)
        {
            Map MapInformation = MapPrefab.GetComponent<Map>();
            List<GameObject> NpcTeam = MapInformation.NpcTeam;
            //coordinates
            var coordinatesGameObject = MapPrefab.transform.Find("Coordinates");
            var teamSettings = gameInformation.GetComponent<PlayerTeams>();
            teamSettings.allCharacterList.teams[1].coordinates.Clear();
            for (int i = 0; i < 3; i++) 
            {
                teamSettings.allCharacterList.teams[0].coordinates[i] = coordinatesGameObject.GetChild(0).GetChild(i);
            }
            for(int i = 0; i < coordinatesGameObject.GetChild(1).childCount; i++)
            {
                teamSettings.allCharacterList.teams[1].coordinates.Add(coordinatesGameObject.GetChild(1).GetChild(i));
            }
            //NPC team spawning
            if (MapInformation.NpcTeam.Count == 0)
            {
                teamSettings.allCharacterList.teams.RemoveAt(2);
            }
            else
            {
                for (int i = 0; i < NpcTeam.Count; i++)
                {
                    teamSettings.allCharacterList.teams[2].coordinates.Add(coordinatesGameObject.GetChild(2).GetChild(i));
                    teamSettings.allCharacterList.teams[2].characters.Add (NpcTeam[i]);
                }
            }
            //AI destinations
            var destinationsGameObject = MapPrefab.transform.Find("AIDestinations");
            var AIsettings = gameInformation.GetComponent<AIManager>();
            List<GameObject> destinations = new List<GameObject>();
            for (int i = 0; i < destinationsGameObject.childCount; i++) 
            {
                destinations.Add(destinationsGameObject.GetChild(i).gameObject);
            }
            AIsettings.AIDestinations = destinations;
        }
    }
    

    public GameObject GetSelectedMap()
    {
        GameObject selectedMap = null;
        if (MapName != null)
        {
            selectedMap = MapPrefabs.Find(x => x.name == MapName);
        }
        return selectedMap;
    }
}
