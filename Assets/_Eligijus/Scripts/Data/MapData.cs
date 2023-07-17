using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : ScriptableObject
{
    [Header("Suitable Enemies")] 
    public List<MapSetup> SuitableEnemies;
    [Header("Npc Team")]
    public List<MapSetup> NpcTeam;
    [Header("Suitable Levels")] 
    public List<MapSetup> SuitableLevels;

    public int NumberOfEnemies = 3;
    public string MapCategory;
    public bool AllowDuplicates;
    public Sprite MapImage;



}
