using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShadowBlink : BaseAction
{
    //private string actionStateName = "ShadowBlink";
    //public int minAttackDamage = 5;
    //public int maxAttackDamage = 8;
    public string abilityAnimationName = "playerChop";
    private Color alphaColor = new Color(1, 1, 1, 110 / 255f);

    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();
    void Start()
    {
        laserGrid = true;
        actionStateName = "ShadowBlink";
    }
    protected override void AddSurroundingsToList(GameObject middleTile, int movementIndex, int x, int y)
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
                isMiddleTileBlocked = CheckIfSpecificLayer(middleTile, x * (i-1), y * (i-1), blockingLayer) ;
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
    public bool canTileBeClicked(GameObject tile)
    {
        if ((CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player")))
        {

            if (TileToBlinkTo(tile) != null)
            {
                return true;
            }
        }
        return false;
    }

    public override void ResolveAbility(GameObject clickedTile)
    {
        if (canTileBeClicked(clickedTile))
        {
            base.ResolveAbility(clickedTile);
            bool wasTargetAlly = true;
            GameObject target = GetSpecificGroundTile(clickedTile, 0, 0, blockingLayer);
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger(abilityAnimationName);
            if (!isAllegianceSame(clickedTile))
            {
                DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
                wasTargetAlly = false;
            }
            //perkelimas

            if (TileToBlinkTo(clickedTile) != null)
            {
                transform.position = TileToBlinkTo(clickedTile).transform.position + new Vector3(0f, 0f, -1f);
            }
            //
            FinishAbility();
            if (DoesCharacterHaveBlessing("Swift strike"))
            {
                GetComponent<GridMovement>().AvailableMovementPoints++;
            }
            if (DoesCharacterHaveBlessing("Pat on the back") && wasTargetAlly)
            {
                AbilityPoints++;
            }
        }
    }
    public override void OnTileHover(GameObject tile)
    {
        if (TileToBlinkTo(tile) != null)
        {
            TileToBlinkTo(tile).transform.Find("mapTile").Find("Character").gameObject.SetActive(true);
            TileToBlinkTo(tile).transform.Find("mapTile").Find("Character").gameObject.GetComponent<SpriteRenderer>().sprite = transform.Find("CharacterModel").GetComponent<SpriteRenderer>().sprite;
            transform.Find("CharacterModel").GetComponent<SpriteRenderer>().color = alphaColor;
        }
        if (!isAllegianceSame(tile)) //jei priesas, tada jam ikirs
        {
            EnableDamagePreview(tile, minAttackDamage, maxAttackDamage);
        }
    }
    public override void OffTileHover(GameObject tile)
    {
        if (TileToBlinkTo(tile) != null)
        {
            TileToBlinkTo(tile).transform.Find("mapTile").Find("Character").gameObject.SetActive(false);
            if (CheckIfSpecificLayer(gameObject, 0, 0, groundLayer))
            {
                GetSpecificGroundTile(gameObject, 0, 0, groundLayer).transform.Find("mapTile").Find("CharacterAlpha").gameObject.SetActive(false);
            }
            transform.Find("CharacterModel").GetComponent<SpriteRenderer>().color = Color.white;
        }
        DisablePreview(tile);
    }
    private GameObject TileToBlinkTo(GameObject targetTile)
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
            bool isBlockingLayer = CheckIfSpecificLayer(targetTile, directionVectors[directionIndex].Item1, directionVectors[directionIndex].Item2, blockingLayer);
            bool isGround = CheckIfSpecificLayer(targetTile, directionVectors[directionIndex].Item1, directionVectors[directionIndex].Item2, groundLayer);
            if (!isBlockingLayer && isGround)
            {
                return GetSpecificGroundTile(targetTile, directionVectors[directionIndex].Item1, directionVectors[directionIndex].Item2, groundLayer);
            }
        }
            return null;
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
                    if (CheckIfSpecificTag(tile, 0,0, blockingLayer, "Player"))
                    {
                        GameObject character = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                        if (!isAllegianceSame(character) && canTileBeClicked(tile))
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
            return GetSpecificGroundTile( EnemyCharacterList[Random.Range(0, EnemyCharacterList.Count - 1)], 0, 0, groundLayer);
        }
        return null;
    }
}