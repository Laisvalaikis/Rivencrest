using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSetup : MonoBehaviour
{
    private string MapName;
    [SerializeField] private List<MapData> _mapDatas;
    public Dictionary<string, MapData> mapDatas;
    [SerializeField] private GameObject mapHolder;
    [SerializeField] private GameObject toFollow;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private PlayerTeams playerTeams;
    [SerializeField] private AIManager aiManager;
    private MapData currentMapData;
    private Data _data;

    private void OnEnable()
    {
        mapDatas = new Dictionary<string, MapData>();
        for (int i = 0; i < _mapDatas.Count; i++)
        {
            mapDatas.Add(_mapDatas[i].name, _mapDatas[i]);
        }

        if (_data == null)
        {
            _data = Data.Instance;
            MapName = _data.townData.selectedMission;
            Debug.Log(MapName);
            SetupAMap();
        }

    }

    public void SetupAMap() 
    {

        if (mapDatas.ContainsKey(MapName))
        {
            MapData mapInfo = new MapData();
            mapInfo.CopyData(mapDatas[MapName]);
            currentMapData = mapInfo;
            //coordinates
            for (int i = 0; i < playerTeams.allCharacterList.Teams.Count && !playerTeams.allCharacterList.Teams[i].isTeamAI; i++)
            {
                playerTeams.allCharacterList.Teams[i].coordinates.Clear();
                for (int j = 0; j < mapInfo.mapCoordinates[i].coordinates.Count; j++)
                {
                    playerTeams.allCharacterList.Teams[i].coordinates.Add(mapInfo.mapCoordinates[i].coordinates[j]);
                }
            }
            //NPC team spawning
            if (mapInfo.npcTeam.Count == 0)
            {
                playerTeams.allCharacterList.Teams.RemoveAt(2);
            }

            //AI destinations
            aiManager.AIDestinations = mapInfo.aiMapCoordinates.coordinates;
            CreateMap();
        }
        else
        {
            Debug.LogError("Map can not be found");
        }
    }

    private void CreateMap()
    {
        Instantiate(GetSelectedMap(), mapHolder.transform);
        toFollow.transform.position = currentMapData.toFollowStartPosition;
        cameraController.panLimitX = currentMapData.panLimitX;
        cameraController.panLimitY = currentMapData.panLimitY;
        cameraController.cinemachineVirtualCamera.Follow = toFollow.transform;
    }


    public GameObject GetSelectedMap()
    {
        GameObject selectedMap = null;
        if (MapName != null)
        {
            selectedMap = mapDatas[MapName].mapPrefab;
        }
        return selectedMap;
    }
}
