using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scream : BaseAction
{
    //private string actionStateName = "StunAttack";
    //public int spellDamage = 60;
    //public int minAttackDamage = 3;
    //public int maxAttackDamage = 4;

    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();

    void Start()
    {
        actionStateName = "Scream";
        isAbilitySlow = false;
    }
    public override void ResolveAbility(Vector3 position)
    {
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            GameObject target = GetSpecificGroundTile(position);
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
            if (isTargetIsolated(target))
            {
                target.GetComponent<PlayerInformation>().Silenced = true;
            }
            DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
            //clickedTile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("red1");
            //
            //GameObject tileToDashBackwards = TileToDashBackwards(clickedTile);
            //if (tileToDashBackwards != null)
            //{
            //    GameObject.Find("GameInformation").GetComponent<GameInformation>().MoveSelectedCharacter(tileToDashBackwards);
            //}
            FinishAbility();
        }
    }
    private GameObject TileToDashBackwards(GameObject targetTile)
    {
        int index = FindIndexOfTile(targetTile);
        var directionVectors = new List<(int, int)>
        {
            (-1, 0),
            (0, -1),
            (1, 0),
            (0, 1)
        };
        bool isGround = CheckIfSpecificLayer(gameObject, directionVectors[index].Item1, directionVectors[index].Item2, groundLayer);
        bool isBlockingLayer = CheckIfSpecificLayer(gameObject, directionVectors[index].Item1, directionVectors[index].Item2, blockingLayer);
        if (isGround && !isBlockingLayer)
        {
            return GetSpecificGroundTile(gameObject, directionVectors[index].Item1, directionVectors[index].Item2, groundLayer);
        }
        return null;
    }
    bool isTargetIsolated(GameObject tile)
    {
        int isolationNumber = 0;
        var directionVectors = new List<(int, int)>
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
        };

        foreach (var x in directionVectors)
        {
            if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") && CheckIfSpecificTag(tile, x.Item1, x.Item2, blockingLayer, "Player") &&
                isAllegianceSame(GetSpecificGroundTile(tile, 0, 0, blockingLayer), GetSpecificGroundTile(tile, x.Item1, x.Item2, blockingLayer), blockingLayer))
            {
                isolationNumber++;
            }
        }
        if (isolationNumber == 0)
        {
            return true;
        }
        return false;
    }
    private int FindIndexOfTile(GameObject tileBeingSearched)
    { //indeksas platesniame liste

        var directionVectors = new List<(int, int)>
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
        };
        for (int i = 0; i < 4; i++)
        {
            if (CheckIfSpecificLayer(tileBeingSearched, 0, 0, groundLayer) &&
                CheckIfSpecificLayer(gameObject, directionVectors[i].Item1, directionVectors[i].Item2, groundLayer) &&
                GetSpecificGroundTile(gameObject, directionVectors[i].Item1, directionVectors[i].Item2, groundLayer) ==
                GetSpecificGroundTile(tileBeingSearched, 0, 0, groundLayer))
            {
                return i;
            }
        }
        return -1;
    }
    /*
    public override bool canTileBeClicked(GameObject tile)
    {
        if ((CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player")) && !isAllegianceSame(tile, blockingLayer))
        {
            return true;
        }
        else return false;
    }
    */
    public override void OnTileHover(GameObject tile)
    {
        EnableTextPreview(tile, "SILENCE");
    }
    public override GameObject PossibleAIActionTile()
    {
        List<GameObject> EnemyCharacterList = new List<GameObject>();
        if (CanGridBeEnabled())
        {
            CreateGrid();
            foreach (List<GameObject> MovementTileList in this.AvailableTiles)
            {
                foreach (GameObject tile in MovementTileList)
                {
                    if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player"))
                    {
                        GameObject character = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                        if (!isAllegianceSame(character) && CanTileBeClicked(tile.transform.position))
                        {
                            EnemyCharacterList.Add(character);
                        }
                    }

                }
            }
        }
        int actionChanceNumber = UnityEngine.Random.Range(0, 100); //ar paleist spella ar ne
        if (EnemyCharacterList.Count > 0 && actionChanceNumber <= 100)
        {
            return GetSpecificGroundTile(EnemyCharacterList[Random.Range(0, EnemyCharacterList.Count - 1)], 0, 0, groundLayer);
        }
        return null;
    }
}
