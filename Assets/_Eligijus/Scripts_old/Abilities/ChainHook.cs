using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChainHook : BaseAction
{
    //private string actionStateName = "ChainHook";
    //public int minAttackDamage = 0;
    //public int maxAttackDamage = 1;
    private bool grapplingHook = false;
    private Color alphaColor = new Color(1, 1, 1, 110 / 255f);
    private bool canTileBeHovered = true;

    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();

    void Start()
    {
        laserGrid = true;
        actionStateName = "ChainHook";
        isAbilitySlow = false;
    }

    protected override void AddSurroundingsToList(GameObject middleTile, int movementIndex, int x, int y) //special tile pattern
    {
        for (int i = 1; i <= AttackRange; i++)
        {
            //cia visur x ir y dauginama kad pasiektu tuos langelius kurie in range yra 
            bool isGround = CheckIfSpecificLayer(middleTile, x * i, y * i, groundLayer);
            bool isBlockingLayer = CheckIfSpecificLayer(middleTile, x * i, y * i, blockingLayer);
            bool isPlayer = CheckIfSpecificTag(middleTile, x * i, y * i, blockingLayer, "Player");
            bool grapplingHookTarget = grapplingHook && CheckIfSpecificTag(middleTile, x * i, y * i, blockingLayer, "Wall");
            bool isPreviousInThisListPlayer = false;
            if (i > 1)
            {
                if (CheckIfSpecificLayer(middleTile, x * (i - 1), y * (i - 1), blockingLayer))
                {
                    isPreviousInThisListPlayer = CheckIfSpecificTag(GetSpecificGroundTile(middleTile, x * (i - 1), y * (i - 1), blockingLayer), 0, 0, blockingLayer, "Player");
                }
            }
            if (i > 1 && isGround && (!isBlockingLayer || isPlayer || grapplingHookTarget) && !isPreviousInThisListPlayer)
            {
                GameObject AddableObject = GetSpecificGroundTile(middleTile, x * i, y * i, groundLayer);
                this.AvailableTiles[movementIndex].Add(AddableObject);
            }
            else if (i == 1 && isGround && (!isBlockingLayer || isPlayer))
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
    */
    /*
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
                tile.GetComponent<HighlightTile>().activeState = actionStateName;//teammates but not yourself
                tile.GetComponent<HighlightTile>().canAbilityTargetAllies = true;
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


    public override void ResolveAbility(GameObject clickedTile)
    {
        
        if (canTileBeClicked(clickedTile))
        {
            base.ResolveAbility(clickedTile);
            //Normal ability
            if (CheckIfSpecificTag(clickedTile, 0, 0, blockingLayer, "Player"))
            {
                //var hookVectors = new List<(int, int)>
                //{
                //(1, 0),
                //(0, 1),
                //(-1, 0),
                //(0, -1)
                //};
                if (FindIndexOfTile(clickedTile) != -1)
                {
                    GameObject target = GetSpecificGroundTile(clickedTile, 0, 0, blockingLayer);
                    target.transform.Find("VFX").Find("VFXImpact").gameObject.GetComponent<Animator>().SetTrigger("burgundy2");
                    //int tileIndex = FindIndexOfTile(clickedTile);
                    if (!isAllegianceSame(target))
                    {
                        int multiplier = GetMultiplier(target);
                        if (multiplier != 0)
                        {
                            DealRandomDamageToTarget(target, minAttackDamage + multiplier * 2, maxAttackDamage + multiplier * 2);
                        }
                    }
                    //target.transform.position = transform.position + new Vector3(hookVectors[tileIndex].Item1, hookVectors[tileIndex].Item2, 0f);
                    target.transform.position = TileToPullTo(target).transform.position;
                    transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");//hook animation
                    FinishAbility();
                }
            }
            //Grappling hook blessing
            else if (CheckIfSpecificTag(clickedTile, 0, 0, blockingLayer, "Wall") && grapplingHook) 
            {
                //GameObject target = GetSpecificGroundTile(clickedTile, 0, 0, blockingLayer);
                if (TileToDashTo(clickedTile) != null)
                {
                    transform.position = TileToDashTo(clickedTile).transform.position + new Vector3(0f, 0f, -1f);
                    GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().FocusSelectedCharacter(gameObject);
                }
                transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
                FinishAbility();
            }
        }
    }
    private int GetMultiplier(GameObject target)
    {
        Vector3 vector3 = target.transform.position - transform.position;
        int multiplier = Mathf.Abs((int)vector3.x + (int)vector3.y) - 1;//gal dar prireiks, o gal ziurek kitiem spellam taip padarysi ¯\_(ツ)_/¯
        return multiplier;
    }
    private GameObject TileToPullTo(GameObject targetTile)
    {
        var clickedTile = GetSpecificGroundTile(targetTile, 0, 0, groundLayer);
        if (clickedTile != null && FindIndexOfTile(clickedTile) != -1)
        {
            var hookVectors = new List<(int, int)>
                {
                (1, 0),
                (0, 1),
                (-1, 0),
                (0, -1)
                };
            int tileIndex = FindIndexOfTile(clickedTile);
            return GetSpecificGroundTile(gameObject, hookVectors[tileIndex].Item1, hookVectors[tileIndex].Item2, groundLayer);
        }
        else
        {
            print("nah dood");
            return null;
        }
    }
    private GameObject TileToDashTo(GameObject targetTile)
    {
        if (FindIndexOfTile(targetTile) != -1)
        {
            print("Index found");
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
        print("INDEX NOT FOUND");
        return null;
    }
    public override bool canTileBeClicked(GameObject tile)
    {
        if ((CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player")) || (grapplingHook && CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Wall")))
        {
            return true;
        }
        else return false;
    }
    public override void OnTileHover(GameObject tile)
    {
        if(CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player"))
        {
            var target = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
            int multiplier = GetMultiplier(target);
            if(multiplier != 0)
            {
                TileToPullTo(tile).transform.Find("mapTile").Find("Character").gameObject.SetActive(true);
                TileToPullTo(tile).transform.Find("mapTile").Find("Character").gameObject.GetComponent<SpriteRenderer>().sprite = target.transform.Find("CharacterModel").GetComponent<SpriteRenderer>().sprite;
                target.transform.Find("CharacterModel").GetComponent<SpriteRenderer>().color = alphaColor;
                if (!isAllegianceSame(target))
                {
                    EnableDamagePreview(TileToPullTo(tile), target, minAttackDamage + multiplier * 2, maxAttackDamage + multiplier * 2);
                }
            }
        }
        else if(CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Wall") && grapplingHook && canTileBeHovered)
        {
            TileToDashTo(tile).transform.Find("mapTile").Find("Character").gameObject.SetActive(true);
            TileToDashTo(tile).transform.Find("mapTile").Find("Character").gameObject.GetComponent<SpriteRenderer>().sprite = transform.Find("CharacterModel").GetComponent<SpriteRenderer>().sprite;
            transform.Find("CharacterModel").GetComponent<SpriteRenderer>().color = alphaColor;
        }
    }
    public override void OffTileHover(GameObject tile)
    {
        if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player"))
        {
            var target = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
            TileToPullTo(tile).transform.Find("mapTile").Find("Character").gameObject.SetActive(false);
            target.transform.Find("CharacterModel").GetComponent<SpriteRenderer>().color = Color.white;
            print("color set to white");
            DisablePreview(TileToPullTo(tile));
        }
        else if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Wall") && grapplingHook)
        {
            TileToDashTo(tile).transform.Find("mapTile").Find("Character").gameObject.SetActive(false);
            transform.Find("CharacterModel").GetComponent<SpriteRenderer>().color = Color.white;
            canTileBeHovered = false;
            StartCoroutine(ExecuteAfterFrames(1, () =>
            {
                canTileBeHovered = true;
            }));
        }
    }
    public override bool canPreviewBeShown(GameObject tile)
    {
        return true;
    }
    public override void BuffAbility()
    {
        if (DoesCharacterHaveBlessing("No escape"))
        {
            AttackRange++;
        }
        if (DoesCharacterHaveBlessing("Grappling hook"))
        {
            grapplingHook = true;
        }
    }
    public override BaseAction GetBuffedAbility(List<Blessing> blessings)
    {
        //Sukuriu kopija
        ChainHook ability = new ChainHook();
        ability.actionStateName = this.actionStateName;
        ability.AttackRange = this.AttackRange;
        ability.AbilityCooldown = this.AbilityCooldown;
        ability.minAttackDamage = this.minAttackDamage;
        ability.maxAttackDamage = this.maxAttackDamage;
        ability.isAbilitySlow = this.isAbilitySlow;
        ability.friendlyFire = this.friendlyFire;
        ability.grapplingHook = this.grapplingHook;

        //Ir pabuffinu
        if (blessings.Find(x => x.blessingName == "No escape") != null)
        {
            ability.AttackRange++;
        }
        if (blessings.Find(x => x.blessingName == "Grappling hook") != null)
        {
            ability.grapplingHook = true;
        }


        return ability;
    }
}