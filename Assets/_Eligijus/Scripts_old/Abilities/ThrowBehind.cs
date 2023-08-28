using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThrowBehind : BaseAction
{
    private Color alphaColor = new Color(1, 1, 1, 110 / 255f);

    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();
    void Start()
    {
        laserGrid = true;
        actionStateName = "ThrowBehind";
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

            if (TileToThrowTo(tile) != null)
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
            GameObject target = GetSpecificGroundTile(clickedTile, 0, 0, blockingLayer);
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell1");
            /*if (!isAllegianceSame(clickedTile))
            {
                DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
            }*/
            //perkelimas
            StartCoroutine(ActivateAimArrow(0.16f, clickedTile, target));
            
            //
            FinishAbility();
        }
    }
    IEnumerator ActivateAimArrow(float secs, GameObject clickedTile, GameObject target)
    {
        yield return new WaitForSeconds(secs);
        if (TileToThrowTo(clickedTile) != null)
        {
            target.transform.position = TileToThrowTo(clickedTile).transform.position + new Vector3(0f, 0f, -1f);
        }
    }
    public override void OnTileHover(GameObject tile)
    {
       /* if (TileToThrowTo(tile) != null)
        {
            TileToThrowTo(tile).transform.Find("mapTile").Find("Character").gameObject.SetActive(true);
            TileToThrowTo(tile).transform.Find("mapTile").Find("Character").gameObject.GetComponent<SpriteRenderer>().sprite = transform.Find("CharacterModel").GetComponent<SpriteRenderer>().sprite;
            transform.Find("CharacterModel").GetComponent<SpriteRenderer>().color = alphaColor;
        }
       */
       /* if (!isAllegianceSame(tile)) //jei priesas, tada jam ikirs
        {
            EnableDamagePreview(tile, minAttackDamage, maxAttackDamage);
        }*/
    }
    public override void OffTileHover(GameObject tile)
    {
       /* if (TileToThrowTo(tile) != null)
        {
            TileToThrowTo(tile).transform.Find("mapTile").Find("Character").gameObject.SetActive(false);
            if (CheckIfSpecificLayer(gameObject, 0, 0, groundLayer))
            {
                GetSpecificGroundTile(gameObject, 0, 0, groundLayer).transform.Find("mapTile").Find("CharacterAlpha").gameObject.SetActive(false);
            }
            transform.Find("CharacterModel").GetComponent<SpriteRenderer>().color = Color.white;
        }*/
        DisablePreview(tile);
    }
    private GameObject TileToThrowTo(GameObject targetTile)
    {
        if (FindIndexOfTile(targetTile) != -1)
        {
            var directionVectors = new List<(int, int)>
             {
            (-1, 0),
            (0, -1),
            (1, 0),
            (0, 1)
            };
            int directionIndex = FindIndexOfTile(targetTile);
            bool isBlockingLayer = CheckIfSpecificLayer(gameObject, directionVectors[directionIndex].Item1, directionVectors[directionIndex].Item2, blockingLayer);
            bool isGround = CheckIfSpecificLayer(gameObject, directionVectors[directionIndex].Item1, directionVectors[directionIndex].Item2, groundLayer);
            if (!isBlockingLayer && isGround)
            {
                return GetSpecificGroundTile(gameObject, directionVectors[directionIndex].Item1, directionVectors[directionIndex].Item2, groundLayer);
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
                    if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player"))
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
            return GetSpecificGroundTile(EnemyCharacterList[Random.Range(0, EnemyCharacterList.Count - 1)], 0, 0, groundLayer);
        }
        return null;
    }
}