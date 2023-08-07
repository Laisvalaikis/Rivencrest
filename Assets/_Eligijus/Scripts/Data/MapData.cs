using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MapData", order = 1)]
public class MapData : ScriptableObject
{
    [Header("Suitable Enemies")] 
    public List<string> suitableEnemies;
    [Header("Npc Team")]
    public List<GameObject> npcTeam;
    [Header("Suitable Levels")] 
    public List<int> suitableLevels;
    public List<MapCoordinates> mapCoordinates;
    public MapCoordinates aiMapCoordinates;

    public int numberOfEnemies = 3;
    public string mapCategory;
    public bool allowDuplicates;
    public Sprite mapImage;
    public string informationText;

    public void CopyData(MapData mapData)
    {
        
        suitableEnemies = mapData.suitableEnemies;
        npcTeam = mapData.npcTeam;
        suitableLevels = mapData.suitableLevels;
        mapCoordinates = mapData.mapCoordinates;
        aiMapCoordinates = mapData.aiMapCoordinates;
        
        numberOfEnemies = mapData.numberOfEnemies;
        mapCategory = mapData.mapCategory;
        allowDuplicates = mapData.allowDuplicates;
        mapImage = mapData.mapImage;
        informationText = mapData.informationText;
    }

}
