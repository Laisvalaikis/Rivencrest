using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Volley : BaseAction
{
    //private string actionStateName = "Volley";
    public int spellDamage = 6;
    private Color alphaColor = new Color(1, 1, 1, 110 / 255f);


    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();

    void Start()
    {
        actionStateName = "Volley";
    }

    private void AddSurroundingsToList(GameObject middleTile)
    {
        int directionIndex = 0;
        var directionVectors = new List<(int, int)>
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
        };

        foreach (var x in directionVectors)
        {
            this.AvailableTiles.Add(new List<GameObject>());
            bool isGroundLayer = CheckIfSpecificLayer(middleTile, x.Item1, x.Item2, groundLayer);
            bool isTargetGroundLayer = CheckIfSpecificLayer(middleTile, x.Item1 * 2, x.Item2 * 2, groundLayer);
            bool isBlockingLayer = CheckIfSpecificLayer(middleTile, x.Item1, x.Item2, blockingLayer);
            bool isPlayer = CheckIfSpecificTag(middleTile, x.Item1, x.Item2, blockingLayer, "Player");
            bool isTargetBlockingLayer = CheckIfSpecificLayer(middleTile, x.Item1 * 2, x.Item2 * 2, blockingLayer);
            bool isTargetPlayer = CheckIfSpecificTag(middleTile, x.Item1 * 2, x.Item2 * 2, blockingLayer, "Player");
            if (isGroundLayer && (!isBlockingLayer || isPlayer) && isTargetGroundLayer && (!isTargetBlockingLayer || isTargetPlayer))
            {
                GameObject AddableObject = GetSpecificGroundTile(middleTile, x.Item1 * 2, x.Item2 * 2, groundLayer);
                bool isDirectionFromPlayerHorizontal = x.Item2 == 0;
                this.AvailableTiles[directionIndex].Add(AddableObject);
                AddAdjacentTiles(AddableObject, isDirectionFromPlayerHorizontal, directionIndex);
            }
            directionIndex++;
        }
    }

    private void AddAdjacentTiles(GameObject centerTile, bool isDirectionFromPlayerHorizontal, int directionIndex)
    {
        List<(int, int)> directionVectors;
        if(isDirectionFromPlayerHorizontal)
        {
            directionVectors = new List<(int, int)> { (0, 1), (0, -1) };
        }
        else
        {
            directionVectors = new List<(int, int)> { (1, 0), (-1, 0) };
        }
        foreach(var x in directionVectors)
        {
            bool isGroundLayer = CheckIfSpecificLayer(centerTile, x.Item1, x.Item2, groundLayer);
            bool isBlockingLayer = CheckIfSpecificLayer(centerTile, x.Item1, x.Item2, blockingLayer);
            bool isPlayer = CheckIfSpecificTag(centerTile, x.Item1, x.Item2, blockingLayer, "Player");
            if (isGroundLayer && (!isBlockingLayer || isPlayer))
            {
                GameObject AddableObject = GetSpecificGroundTile(centerTile, x.Item1, x.Item2, groundLayer);
                this.AvailableTiles[directionIndex].Add(AddableObject);
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
    public override void CreateGrid()
    {
        // transform.gameObject.GetComponent<PlayerInformation>().currentState = actionStateName;
        this.AvailableTiles.Clear();
        AddSurroundingsToList(transform.gameObject);
        MergeIntoOneList();
    }
    /*
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
    */
    /*
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
        if(isGround && !isBlockingLayer)
        {
            return GetSpecificGroundTile(gameObject, directionVectors[index].Item1, directionVectors[index].Item2, groundLayer);
        }
        return null;
    }
    public override void ResolveAbility(GameObject clickedTile)
    {

        if (FindIndexOfTile(clickedTile) != -1)
        {
            base.ResolveAbility(clickedTile);
            foreach (GameObject tile in AvailableTiles[FindIndexOfTile(clickedTile)])
            {
                if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") && (!isAllegianceSame(tile) || friendlyFire))
                {
                    GameObject target = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                    DealRandomDamageToTarget(target, spellDamage, spellDamage);
                    if (DoesCharacterHaveBlessing("Noxious arrows"))
                    {
                        target.GetComponent<PlayerInformation>().Poisons.Add(new PlayerInformation.Poison(gameObject, 2, 2));
                    }
                    else
                    {
                        target.GetComponent<PlayerInformation>().Poisons.Add(new PlayerInformation.Poison(gameObject, 2, 1));
                    }
                  
                }
                tile.transform.Find("mapTile").Find("VFXImpact").gameObject.GetComponent<Animator>().SetTrigger("forest3");
            }
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell1");
            GameObject tileToDashBackwards = TileToDashBackwards(clickedTile);
            if(tileToDashBackwards != null)
            {
                GameObject.Find("GameInformation").GetComponent<GameInformation>().MoveSelectedCharacter(tileToDashBackwards);
            }
            FinishAbility();
        }
    }
    public override void OnTileHover(GameObject tile)
    {
        if(TileToDashBackwards(tile) != null)
        {
            TileToDashBackwards(tile).transform.Find("mapTile").Find("Character").gameObject.SetActive(true);
            TileToDashBackwards(tile).transform.Find("mapTile").Find("Character").gameObject.GetComponent<SpriteRenderer>().sprite = transform.Find("CharacterModel").GetComponent<SpriteRenderer>().sprite;
            /*if (CheckIfSpecificLayer(gameObject, 0, 0, groundLayer))
            {
                GetSpecificGroundTile(gameObject, 0, 0, groundLayer).transform.Find("mapTile").Find("CharacterAlpha").gameObject.SetActive(true);
                GetSpecificGroundTile(gameObject, 0, 0, groundLayer).transform.Find("mapTile").Find("CharacterAlpha").
                    gameObject.GetComponent<SpriteRenderer>().sprite = transform.Find("CharacterModel").GetComponent<SpriteRenderer>().sprite;
            }*/
            transform.Find("CharacterModel").GetComponent<SpriteRenderer>().color = alphaColor;
        }
        if (FindIndexOfTile(tile) != -1)
        {
            EnableDamagePreview(tile, AvailableTiles[FindIndexOfTile(tile)], spellDamage);
        }
    }

    public override void OffTileHover(GameObject tile)
    {
        if (TileToDashBackwards(tile) != null)
        {
            TileToDashBackwards(tile).transform.Find("mapTile").Find("Character").gameObject.SetActive(false);
            if (CheckIfSpecificLayer(gameObject, 0, 0, groundLayer))
            {
                GetSpecificGroundTile(gameObject, 0, 0, groundLayer).transform.Find("mapTile").Find("CharacterAlpha").gameObject.SetActive(false);
            }
            transform.Find("CharacterModel").GetComponent<SpriteRenderer>().color = Color.white;
        }
        if (FindIndexOfTile(tile) != -1)
        {
            DisablePreview(tile, AvailableTiles[FindIndexOfTile(tile)]);
        }
    }

}
