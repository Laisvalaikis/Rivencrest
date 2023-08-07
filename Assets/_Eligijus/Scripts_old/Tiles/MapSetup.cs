using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSetup : MonoBehaviour
{
    [HideInInspector] public string MapName;
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
        }

    }

    public void SetupAMap() 
    {

        if (mapDatas[MapName] != null)
        {
            MapData mapInfo = new MapData();
            mapInfo.CopyData(mapDatas[MapName]);
            currentMapData = mapInfo;
            //coordinates
            playerTeams.allCharacterList.teams[1].coordinates.Clear();
            for (int i = 0; i < playerTeams.allCharacterList.teams.Count; i++)
            {
                for (int j = 0; j < mapInfo.mapCoordinates[i].coordinates.Count; j++)
                {
                    playerTeams.allCharacterList.teams[i].coordinates[j] = mapInfo.mapCoordinates[i].coordinates[j];
                }
            }
            //NPC team spawning
            if (mapInfo.npcTeam.Count == 0)
            {
                playerTeams.allCharacterList.teams.RemoveAt(2);
            }

            //AI destinations
            aiManager.AIDestinations = mapInfo.aiMapCoordinates.coordinates;
            CreateMap();
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
