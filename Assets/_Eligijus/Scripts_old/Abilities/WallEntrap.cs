using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WallEntrap : BaseAction
{
    public GameObject WallPrefab;

    private List<GameObject> SpawnedWalls = new List<GameObject>();

    private List<GameObject> AdditionalDamageTiles = new List<GameObject>();
    private GameObject protectedAlly;

    void Start()
    {
        actionStateName = "WallEntrap";
        isAbilitySlow = false;
    }

    public override void OnTurnStart()
    {
        if (SpawnedWalls.Count > 0)
        {
            foreach (GameObject x in SpawnedWalls)
            {
                x.GetComponent<PlayerInformation>().DealDamage(1, false, gameObject);
            }
        }
    }
    public override void ResolveAbility(GameObject clickedTile)
    {
        if (canTileBeClicked(clickedTile))
        {
            base.ResolveAbility(clickedTile);
            SpawnedWalls.Clear();
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
            GameObject target = GetSpecificGroundTile(clickedTile, 0, 0, blockingLayer);
            SpawnAdjacentWalls(target);

            FinishAbility();
        }
    }
    private void SpawnAdjacentWalls(GameObject center)
    {
        var DirectionVectors = new List<(int, int)>
                {
                    (1, 0),
                    (0, 1),
                    (-1, 0),
                    (0, -1)
                };
        foreach (var x in DirectionVectors)
        {
            if (CheckIfSpecificLayer(center, x.Item1, x.Item2, groundLayer)) //animation on ground
            {
                //GetSpecificGroundTile(center, x.Item1, x.Item2, groundLayer).transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("yellow1");
            }
            if (CheckIfSpecificLayer(center, x.Item1, x.Item2, groundLayer) && !CheckIfSpecificLayer(center, x.Item1, x.Item2, blockingLayer))
            {
                GameObject tileToSpawnTo = GetSpecificGroundTile(center, x.Item1, x.Item2, groundLayer);
                GameObject spawnedWall = Instantiate(WallPrefab, tileToSpawnTo.transform.position + new Vector3(0f, 0f, 1f), Quaternion.identity) as GameObject;
                GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().ChangeVisionTiles();
                spawnedWall.transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell1");
                SpawnedWalls.Add(spawnedWall);
            }
        }
    }
    public override void HighlightAll()
    {
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                tile.GetComponent<HighlightTile>().SetHighlightBool(true);
                tile.GetComponent<HighlightTile>().canAbilityTargetAllies = true;
                tile.GetComponent<HighlightTile>().canAbilityTargetYourself = true;
                tile.GetComponent<HighlightTile>().activeState = actionStateName;
                tile.GetComponent<HighlightTile>().ChangeBaseColor();
            }
        }
        GetSpecificGroundTile(transform.gameObject, 0, 0, groundLayer).GetComponent<HighlightTile>().SetHighlightBool(true);
    }
    public override bool canTileBeClicked(GameObject tile)
    {
        return CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player");
    }
    public override void OnTileHover(GameObject tile)
    {

      EnableTextPreview(tile, "ENTRAP");
  
    }
    public override GameObject PossibleAIActionTile()
    {
        List<GameObject> EnemyCharacterList = new List<GameObject>();
        if (canGridBeEnabled())
        {
            CreateGrid();
            foreach (GameObject tile in MergedTileList)
            {
                if (canTileBeClicked(tile) && !isAllegianceSame(tile))
                {
                    GameObject character = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                    EnemyCharacterList.Add(character);
                }
            }
        }
        int actionChanceNumber = Random.Range(0, 100); //ar paleist spella ar ne
        if (EnemyCharacterList.Count > 0 && actionChanceNumber <= 100)
        {
            return GetSpecificGroundTile(EnemyCharacterList[Random.Range(0, EnemyCharacterList.Count - 1)], 0, 0, groundLayer);
        }
        return null;
    }
}
