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

    public int numberOfEnemies = 3;
    public string mapCategory;
    public bool allowDuplicates;
    public Sprite mapImage;
    public string informationText;



}
