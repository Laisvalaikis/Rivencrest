using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MapData", order = 1)]
public class MapData : ScriptableObject
{
    [Header("Suitable Enemies")] 
    public List<string> SuitableEnemies;
    [Header("Npc Team")]
    public List<GameObject> NpcTeam;
    [Header("Suitable Levels")] 
    public List<int> SuitableLevels;

    public int NumberOfEnemies = 3;
    public string MapCategory;
    public bool AllowDuplicates;
    public Sprite MapImage;



}
