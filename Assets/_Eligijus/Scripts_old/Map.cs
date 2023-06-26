using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public List<string> suitableEnemies;
    public List<GameObject> NpcTeam;
    public List<int> suitableLevels;
    public int numberOfEnemies;
    public string mapCategory;
    public bool allowDuplicates;
    public Sprite mapImage;
}
