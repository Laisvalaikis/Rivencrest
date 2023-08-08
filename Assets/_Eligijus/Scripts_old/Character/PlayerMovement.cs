using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : CharacterAction
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

        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mousePos = Input.mousePosition;  
            Camera mainCamera = Camera.main;
            mousePos.z = mainCamera.nearClipPlane;
            Vector3 worldpos = mainCamera.ScreenToWorldPoint(mousePos);
            OnTileClick(worldpos);
        }
       
    }
    
    public override void OnTileClick(Vector3 mousePosition)
    {
        
        base.OnTileClick(mousePosition);
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
