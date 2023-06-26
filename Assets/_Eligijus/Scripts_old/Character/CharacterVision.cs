using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVision : MonoBehaviour
{
    public LayerMask groundLayer;
    public LayerMask blockingLayer;

    private RaycastHit2D raycast;
    public int VisionRange = 4;
    private Vector3 currentCharactersPosition;

    private List<List<GameObject>> AllVisionTiles = new List<List<GameObject>>();

    void Awake()
    {
        currentCharactersPosition = transform.position;
    }
    void Update()
    {

        if (currentCharactersPosition != transform.position)
        {
            GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().ChangeVisionTiles();
            //GetComponent<PlayerMovement>().OnAnyMove();
        }


        currentCharactersPosition = transform.position;
        /*
        if(GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().ActiveTeam == GetComponent<PlayerInformation>().CharactersTeam)
        {
            EnableGrid();
        }
        */
        /*if (Input.GetKeyDown("e"))
        {
            EnableGrid();
        }
        if (Input.GetKeyDown("r"))
        {
            DisableGrid();
        }*/
    }
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
            bool isWall = CheckIfSpecificTag(middleTile, x.Item1, x.Item2, blockingLayer, "Wall");
            bool isMiddleTileWall = CheckIfSpecificTag(middleTile, 0, 0, blockingLayer, "Wall");
            if (isGroundLayer && (!isBlockingLayer || isPlayer || isWall) && !isMiddleTileWall)
            {
                GameObject AddableObject = GetSpecificGroundTile(middleTile, x.Item1, x.Item2, groundLayer);
                if (movementIndex < 2 || !AllVisionTiles[movementIndex - 2].Contains(AddableObject))
                {
                    this.AllVisionTiles[movementIndex].Add(AddableObject);
                }
            }
        }
    }

    public  void EnableGrid()
    {
        if (GetComponent<PlayerInformation>().health > 0)
        {
            CreateGrid();
            HighlightAll();
        }
    }
    private void CreateGrid()
    {
        this.AllVisionTiles.Clear();
        if (VisionRange > 0)
        {
            this.AllVisionTiles.Add(new List<GameObject>());
            AddSurroundingsToList(transform.gameObject, 0);
        }

        for (int i = 1; i <= VisionRange - 1; i++)
        {
            this.AllVisionTiles.Add(new List<GameObject>());

            foreach (var tileInPreviousList in this.AllVisionTiles[i - 1])
            {
                AddSurroundingsToList(tileInPreviousList, i);
            }
        }
    }
    public void DisableGrid()
    {
        foreach (List<GameObject> MovementTileList in this.AllVisionTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                tile.GetComponent<HighlightTile>().FogOfWarTile.SetActive(true);
            }
        }
        GetSpecificGroundTile(gameObject, 0, 0, groundLayer).GetComponent<HighlightTile>().FogOfWarTile.SetActive(true);
    }
    public void HighlightAll()
    {
        foreach (List<GameObject> MovementTileList in this.AllVisionTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                //tile.GetComponent<HighlightTile>().fogOfWar = false;
                tile.GetComponent<HighlightTile>().FogOfWarTile.SetActive(false);
            }
        }
        GetSpecificGroundTile(gameObject,0,0,groundLayer).GetComponent<HighlightTile>().FogOfWarTile.SetActive(false);
    }
    protected GameObject GetSpecificGroundTile(GameObject tile, int x, int y, LayerMask chosenLayer)
    {
        Vector3 firstPosition = tile.transform.position + new Vector3(0f, 0.5f, 0f) + new Vector3(x, y, 0f);
        Vector3 secondPosition = firstPosition + new Vector3(0.1f, 0f, 0f);
        raycast = Physics2D.Linecast(firstPosition, secondPosition, chosenLayer);
        return raycast.transform.gameObject;
    }
    protected bool CheckIfSpecificLayer(GameObject tile, int x, int y, LayerMask chosenLayer)
    {
        Vector3 firstPosition = tile.transform.position + new Vector3(0f, 0.5f, 0f) + new Vector3(x, y, 0f);
        Vector3 secondPosition = firstPosition + new Vector3(0.1f, 0f, 0f);
        raycast = Physics2D.Linecast(firstPosition, secondPosition, chosenLayer);
        if (raycast.transform == null)
        {
            return false;
        }
        return true;
    }
    protected bool CheckIfSpecificTag(GameObject tile, int x, int y, LayerMask chosenLayer, string tagName)
    {
        Vector3 firstPosition = tile.transform.position + new Vector3(0f, 0.5f, 0f) + new Vector3(x, y, 0f);
        Vector3 secondPosition = firstPosition + new Vector3(0.1f, 0f, 0f);
        raycast = Physics2D.Linecast(firstPosition, secondPosition, chosenLayer);
        if (raycast.transform == null)
        {
            return false;
        }
        else if (raycast.transform.CompareTag(tagName))
        {
            return true;
        }
        return false;
    }
    //AI
    public List<GameObject> VisibleCharacterList()
    {
        List<GameObject> visibleCharacters = new List<GameObject>();
        foreach (List<GameObject> MovementTileList in this.AllVisionTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player"))
                {
                    visibleCharacters.Add(GetSpecificGroundTile(tile, 0, 0, blockingLayer));
                }
            }
        }
        return visibleCharacters;
    }
}