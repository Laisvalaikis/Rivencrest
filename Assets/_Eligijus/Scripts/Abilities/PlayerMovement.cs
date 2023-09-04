using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : BaseAction
{
    private bool ArrowMovement = false;
    private int horizontal = 0;
    private int vertical = 0;
    private LayerMask blockingLayer;
    private LayerMask groundLayer;
    private LayerMask fogLayer;
    private LayerMask whiteFieldLayer;
    public Vector3 playerCenter = new Vector3(0, 0.5f, 0);
    private bool Hovered;
    private bool isFacingRight = true;
    private Vector3 currentPosition;
    private RaycastHit2D raycast;
    private GameTileMap _gameTileMap;

    void Start()
    {
        _gameTileMap = GameTileMap.Tilemap;
        blockingLayer = LayerMask.GetMask("BlockingLayer");
        groundLayer = LayerMask.GetMask("Ground");
        fogLayer = LayerMask.GetMask("Fog");
        whiteFieldLayer = LayerMask.GetMask("WhiteField");
        //boardManager = GameObject.Find("GameManager(Clone)").GetComponent<BoardManager>();
        currentPosition = transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
    }

    void Update()
    {

        // if (Input.GetMouseButtonUp(0))
        // {
        //     // Vector3 mousePos = Input.mousePosition;  
        //     // Camera mainCamera = Camera.main;
        //     // mousePos.z = mainCamera.nearClipPlane;
        //     // Vector3 worldpos = mainCamera.ScreenToWorldPoint(mousePos);
        //     // OnTileClick(worldpos);
        // }
       
    }

    protected override void HighlightGridTile(ChunkData chunkData)
    {
        if (chunkData.GetCurrentCharacter() == null)
        {
            chunkData.GetTileHighlight().ActivateColorGridTile(true);
        }
    }
    
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        if (!GameTileMap.Tilemap.CharacterIsOnTile(position))
        {
            GameTileMap.Tilemap.MoveSelectedCharacter(position, new Vector3(0, 0.5f, 1));
        }
        FinishAbility();
        CreateGrid();
    }
    
    
    public override void OnTileClick(Vector3 mousePosition)
    {
        // if (!GameTileMap.Tilemap.CharacterIsOnTile(mousePosition))
        // {
        //     GameTileMap.Tilemap.MoveSelectedCharacter(mousePosition, new Vector3(0, 0.5f, 1));
        // }
        //
        // Debug.Log("We are in Character");
        // base.OnTileClick(mousePosition);
    }
   
    public void Flip(bool shouldCharacterFaceRight)
    {
        if(isFacingRight != shouldCharacterFaceRight)
        {
            isFacingRight = !isFacingRight;
            transform.Find("CharacterModel").Rotate(0f, 180f, 0f);
        }
    }
}
