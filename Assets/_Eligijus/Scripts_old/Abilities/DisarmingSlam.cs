using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisarmingSlam : BaseAction
{
    //private string actionStateName = "DisarmingSlam";
    //public int minAttackDamage = 4;
    //public int maxAttackDamage = 7;
    private Color alphaColor = new Color(1, 1, 1, 110 / 255f);

    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();
    void Start()
    {
        laserGrid = true;
        actionStateName = "DisarmingSlam";
    }
    protected void AddSurroundingsToList(GameObject middleTile, int movementIndex, int x, int y)
    {
        for (int i = 1; i <= AttackRange; i++)
        {
            //cia visur x ir y dauginama kad pasiektu tuos langelius kurie in range yra 
            bool isGround = CheckIfSpecificLayer(middleTile, x * i, y * i, groundLayer);
            bool isBlockingLayer = CheckIfSpecificLayer(middleTile, x * i, y * i, blockingLayer);
            bool isPlayer = CheckIfSpecificTag(middleTile, x * i, y * i, blockingLayer, "Player");
            bool isWall = CheckIfSpecificTag(middleTile, x * i, y * i, blockingLayer, "Wall");

            bool isMiddleTileBlocked = false;
            if (i > 1)
            {
                isMiddleTileBlocked = CheckIfSpecificLayer(middleTile, x * (i - 1), y * (i - 1), blockingLayer);
            }
            if (isGround && (!isBlockingLayer || isPlayer) && (!isMiddleTileBlocked))
            {
                GameObject AddableObject = GetSpecificGroundTile(middleTile, x * i, y * i, groundLayer);
                this.AvailableTiles[movementIndex].Add(AddableObject);
            }
            else
            {
                break; //kad neitu kiaurai sienas
            }
        }
    }
    /*
    private void AddSurroundingsToList(GameObject middleTile, int movementIndex, int x, int y)
    {
        for (int i = 1; i <= AttackRange; i++)
        {
            //cia visur x ir y dauginama kad pasiektu tuos langelius kurie in range yra 
            bool isGround = CheckIfSpecificLayer(middleTile, x * i, y * i, groundLayer);
            bool isBlockingLayer = CheckIfSpecificLayer(middleTile, x * i, y * i, blockingLayer);
            bool isPlayer = CheckIfSpecificTag(middleTile, x * i, y * i, blockingLayer, "Player");
            bool isWall = CheckIfSpecificTag(middleTile, x * i, y * i, blockingLayer, "Wall");

            bool isMiddleTileBlocked = false;
            if (i > 1)
            {
                isMiddleTileBlocked = CheckIfSpecificLayer(middleTile, x * (i - 1), y * (i - 1), blockingLayer);
            }
            if (isGround && (!isBlockingLayer || isPlayer) && (!isMiddleTileBlocked))
            {
                GameObject AddableObject = GetSpecificGroundTile(middleTile, x * i, y * i, groundLayer);
                this.AvailableTiles[movementIndex].Add(AddableObject);
            }
            else
            {
                break; //kad neitu kiaurai sienas
            }
        }
    }

    public override void EnableGrid()
    {
        if (canGridBeEnabled())
        {
            CreateGrid();
            HighlightAll();
        }
    }
    public override void CreateGrid()
    {
        transform.gameObject.GetComponent<PlayerInformation>().currentState = actionStateName;
        this.AvailableTiles.Clear();
        this.AvailableTiles.Add(new List<GameObject>());
        var directionVectors = new List<(int, int)>
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
        };
        int i = 0;
        foreach (var x in directionVectors)
        {
            this.AvailableTiles.Add(new List<GameObject>());
            AddSurroundingsToList(transform.gameObject, i, x.Item1, x.Item2);
            i++;
        }
    }
    public override void DisableGrid()
    {
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                tile.GetComponent<HighlightTile>().canAbilityTargetAllies = false;
                tile.GetComponent<HighlightTile>().SetHighlightBool(false);
            }
        }
    }
    */
    /*
    public void HighlightAll()
    {
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                tile.GetComponent<HighlightTile>().SetHighlightBool(true);
                tile.GetComponent<HighlightTile>().canAbilityTargetAllies = true;
                tile.GetComponent<HighlightTile>().activeState = actionStateName;
                tile.GetComponent<HighlightTile>().ChangeBaseColor();
            }
        }
    }
    */
    private int FindIndexOfTile(GameObject tileBeingSearched)
    { //indeksas platesniame liste

        for (int j = 0; j < AvailableTiles.Count; j++)
        {
            for (int i = 0; i < AvailableTiles[j].Count; i++)
            {
                if (AvailableTiles[j][i] == tileBeingSearched)
                {
                    return j;
                }
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
        return false;
    }
    */

    public override void ResolveAbility(Vector3 position)
    {
        
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("blockSpell1");
            GameObject target = GetSpecificGroundTile(position);
            DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);//??was crit false
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
            target.GetComponent<PlayerInformation>().ApplyDebuff("Disarmed");
            /*if (TileToDashTo(clickedTile) != null)
            {
                transform.position = TileToDashTo(clickedTile).transform.position + new Vector3(0f, 0f, -1f);
                GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().FocusSelectedCharacter(gameObject);
            }*/
            //clickedTile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("yellow2");
            
            FinishAbility();
        }
    }
    private GameObject TileToDashTo(GameObject targetTile)
    {
        if (FindIndexOfTile(targetTile) != -1)
        {
            var directionVectors = new List<(int, int)>
             {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
            };
            int directionIndex = FindIndexOfTile(targetTile);
            bool isBlockingLayer = CheckIfSpecificLayer(targetTile, -directionVectors[directionIndex].Item1, -directionVectors[directionIndex].Item2, blockingLayer);
            bool isGround = CheckIfSpecificLayer(targetTile, -directionVectors[directionIndex].Item1, -directionVectors[directionIndex].Item2, groundLayer);
            if (!isBlockingLayer && isGround)
            {
                return GetSpecificGroundTile(targetTile, -directionVectors[directionIndex].Item1, -directionVectors[directionIndex].Item2, groundLayer);
            }
        }
        return null;
    }
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
            foreach (List<GameObject> MovementTileList in this.AvailableTiles)
            {
                foreach (GameObject tile in MovementTileList)
                {
                    if (CanTileBeClicked(tile.transform.position))
                    {
                        GameObject character = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                        EnemyCharacterList.Add(character);
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
    public override void BuffAbility()
    {
        if (DoesCharacterHaveBlessing("Cavalry"))
        {
            AttackRange++;
        }
    }
    public override BaseAction GetBuffedAbility(List<Blessing> blessings)
    {
        //Sukuriu kopija
        DisarmingSlam ability = new DisarmingSlam();
        ability.actionStateName = this.actionStateName;
        ability.AttackRange = this.AttackRange;
        ability.AbilityCooldown = this.AbilityCooldown;
        ability.minAttackDamage = this.minAttackDamage;
        ability.maxAttackDamage = this.maxAttackDamage;
        ability.isAbilitySlow = this.isAbilitySlow;
        ability.friendlyFire = this.friendlyFire;

        //Ir pabuffinu
        if (blessings.Find(x => x.blessingName == "Cavalry") != null)
        {
            ability.AttackRange++;
        }

        return ability;
    }
}