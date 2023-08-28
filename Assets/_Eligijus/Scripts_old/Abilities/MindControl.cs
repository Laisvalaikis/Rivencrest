using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MindControl : BaseAction
{
    //private string actionStateName = "MindControl";
    //public int attackDamage = 60;
    public string impactVFXName = "";
    //public int minAttackDamage = 5;
    //public int maxAttackDamage = 7;

    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();
    //private List<GameObject> MergedTileList = new List<GameObject>();

    void Start()
    {
        actionStateName = "MindControl";
        //isAbilitySlow = false;
        //AttackAbility = true;
    }
    /*
    private void AddSurroundingsToList(GameObject middleTile, int movementIndex)
    {
        var directionVectors = new List<(int, int)>
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
        };

        foreach (var x in directionVectors)
        {
            bool isGroundLayer = CheckIfSpecificLayer(middleTile, x.Item1, x.Item2, groundLayer);
            bool isBlockingLayer = CheckIfSpecificLayer(middleTile, x.Item1, x.Item2, blockingLayer);
            bool isPlayer = CheckIfSpecificTag(middleTile, x.Item1, x.Item2, blockingLayer, "Player");
            if (isGroundLayer && (!isBlockingLayer || isPlayer))
            {
                GameObject AddableObject = GetSpecificGroundTile(middleTile, x.Item1, x.Item2, groundLayer);
                this.AvailableTiles[movementIndex].Add(AddableObject);
            }
        }
    }
    */
    /*
    public override void EnableGrid()
    {
        if (canGridBeEnabled())
        {
            CreateGrid();
            HighlightAll();
        }

    }
    */
    /*
    public override void CreateGrid()
    {
        transform.gameObject.GetComponent<PlayerInformation>().currentState = actionStateName;
        this.AvailableTiles.Clear();
        if (AttackRange > 0)
        {
            this.AvailableTiles.Add(new List<GameObject>());
            AddSurroundingsToList(transform.gameObject, 0);
        }

        for (int i = 1; i <= AttackRange - 1; i++)
        {
            this.AvailableTiles.Add(new List<GameObject>());

            foreach (var tileInPreviousList in this.AvailableTiles[i - 1])
            {
                AddSurroundingsToList(tileInPreviousList, i);
            }
        }
        //Merging into one list
        MergedTileList.Clear();
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                if (!MergedTileList.Contains(tile))
                {
                    MergedTileList.Add(tile);
                }
            }
        }
        if (CheckIfSpecificLayer(gameObject, 0, 0, groundLayer))
        {
            MergedTileList.Remove(GetSpecificGroundTile(gameObject, 0, 0, groundLayer));
        }
    }

    public override void DisableGrid()
    {
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                tile.GetComponent<HighlightTile>().SetHighlightBool(false);
            }
        }
    }
    public void HighlightAll()
    {
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                tile.GetComponent<HighlightTile>().SetHighlightBool(true);
                tile.GetComponent<HighlightTile>().activeState = actionStateName;
                tile.GetComponent<HighlightTile>().ChangeBaseColor();
            }
        }
        GetSpecificGroundTile(transform.gameObject, 0, 0, groundLayer).GetComponent<HighlightTile>().SetHighlightBool(false);
    }
    */
    public override void OnTurnStart()//pradzioj ejimo
    {
        if (GetComponent<PlayerInformation>().MindControlTarget != null)
        {
            transform.Find("CharacterModel").GetComponent<Animator>().SetBool("block", false);
            GetComponent<PlayerInformation>().MindControlTarget.GetComponent<PlayerInformation>().Silenced = false;
            GetComponent<PlayerInformation>().MindControlTarget.GetComponent<PlayerInformation>().MindControlled = false;
            GetComponent<PlayerInformation>().MindControlTarget = null;
        }
    }
    public override void ResolveAbility(Vector3 position)
    {
        
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
            GameObject target = GetSpecificGroundTile(position).GetCurrentCharacter();
            DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
            target.GetComponent<PlayerInformation>().ApplyDebuff("MindControl", gameObject);
            //clickedTile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger(impactVFXName);

            FinishAbility();
        }
    }
    public override void SpecificAbilityAction(GameObject character = null)
    {
        GetComponent<PlayerInformation>().MindControlTarget = character;
        transform.Find("CharacterModel").GetComponent<Animator>().SetBool("block", true);

    }
    /*
    public override bool canTileBeClicked(GameObject tile)
    {
        //foreach (GameObject tileInList in MergedTileList)

        if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") && !isAllegianceSame(GetSpecificGroundTile(tile, 0, 0, blockingLayer), blockingLayer))
        {
            return true;
        }

        return false;
    }
    */
    public override void OnTileHover(GameObject tile)
    {
        EnableDamagePreview(tile, minAttackDamage, maxAttackDamage);
    }
    public override GameObject PossibleAIActionTile()
    {
        List<GameObject> EnemyCharacterList = new List<GameObject>();
        if (CanGridBeEnabled())
        {
            CreateGrid();
            foreach (GameObject tile in MergedTileList)
            {
                if (CanTileBeClicked(tile.transform.position))
                {
                    GameObject character = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                    EnemyCharacterList.Add(character);
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
