using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HighlightTile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer ColorGridTile;
    [SerializeField] private SpriteRenderer FogOfWarTile;
    [SerializeField] private SpriteRenderer ArrowTile;
    [SerializeField] private SpriteRenderer DangerUI;
    [SerializeField] private SpriteRenderer PlayerSelect;
    [SerializeField] private SpriteRenderer Preview;
    
    [SerializeField] private Sprite rightStartArrow;
    [SerializeField] private Sprite leftStartArrow;
    [SerializeField] private Sprite downStartArrow;
    [SerializeField] private Sprite upStartArrow;

    [SerializeField] private Sprite rightEndArrow;
    [SerializeField] private Sprite leftEndArrow;
    [SerializeField] private Sprite downEndArrow;
    [SerializeField] private Sprite upEndArrow;

    [SerializeField] private Sprite verticalIntermediateArrow;
    [SerializeField] private Sprite horizontalIntermediateArrow;

    [SerializeField] private Sprite topLeftCornerArrow;
    [SerializeField] private Sprite bottomLeftCornerArrow;
    [SerializeField] private Sprite topRightCornerArrow;
    [SerializeField] private Sprite bottomRightCornerArrow;
    
    //public Sprite HoverSprite;
    //private Sprite OriginalSprite;
    private Color AttackHighlightColor;
    private Color MovementHighlightColor;//lol
    //[HideInInspector] public Color HoverHighlightColor;
    public Color NotHoveredColor;
    private Color OtherHighlight;
    private Color InspectionHighlight;
    [HideInInspector] public bool fogOfWar = true;
    public bool isHighlighted = false;
    public string activeState;
    [HideInInspector] public bool canAbilityTargetAllies = false;
    [HideInInspector] public bool canAbilityTargetYourself = false;
    public bool isCharacterOnTop = false;//[HideInInspector] 
    private bool WasButtonClicked = false;
    private bool isFogOfWarEnabled = true;
    private bool isHovered = false;
    private RaycastHit2D raycast;
    private LayerMask blockingLayer;
    private ColorManager colorManager;
    
    public void ActivateColorGridTile(bool value)
    {
        ColorGridTile.enabled = value;
        isHighlighted = value;
    }
    
    public void SetPreviewSprite(Sprite sprite)
    {
        Preview.sprite = sprite;
    }

    public void TogglePreviewSprite(bool value)
    {
        Preview.enabled = value;
    }

    public void SetArrowSprite(int arrowType)
    {
        Sprite arrowSprite = null;

        switch (arrowType)
        {
            case 1:
                arrowSprite = rightStartArrow;
                break;
            case 2:
                arrowSprite = leftStartArrow;
                break;
            case 3:
                arrowSprite = downStartArrow;
                break;
            case 4:
                arrowSprite = upStartArrow;
                break;
            case 5:
                arrowSprite = rightEndArrow;
                break;
            case 6:
                arrowSprite = leftEndArrow;
                break;
            case 7:
                arrowSprite = downEndArrow;
                break;
            case 8:
                arrowSprite = upEndArrow;
                break;
            case 9:
                arrowSprite = verticalIntermediateArrow;
                break;
            case 10:
                arrowSprite = horizontalIntermediateArrow;
                break;
            case 11:
                arrowSprite = topLeftCornerArrow;
                break;
            case 12:
                arrowSprite = topRightCornerArrow;
                break;
            case 13:
                arrowSprite = bottomLeftCornerArrow;
                break;
            case 14:
                arrowSprite = bottomRightCornerArrow;
                break;
            default:
                // Debug.LogError("Invalid arrow type");
                return;
        }
        ArrowTile.sprite = arrowSprite;
        ArrowTile.enabled = true;
    }

    public void DeactivateArrowTile()
    {
        ArrowTile.enabled = false;
    }
    
    public void ActivatePlayerTile(bool value)
    {
        PlayerSelect.enabled = value;
    }
    
    public void SetHighlightColor(Color color)
    {
        ColorGridTile.color = color;
    }

    public void EnableDisableFogOfWar(bool value)
    {
        FogOfWarTile.enabled = value;
    }

    public bool FogOfWarIsEnabled()
    {
        return FogOfWarTile.enabled;
    }

    // void Update()
    // {
    //     //Updating isCharacterOnTop to prevent bug
    //     if (Input.GetMouseButtonUp(0) && isHovered)
    //     {
    //         isCharacterOnTop = CheckIfSpecificTag(gameObject, 0, 0, blockingLayer, "Player") || CheckIfSpecificTag(gameObject, 0, 0, blockingLayer, "Wall");
    //     }
    //
    //     //Mobile controls
    //     if (Input.GetMouseButtonUp(0) && isHovered && (!isCharacterOnTop)) //|| 
    //                                                                        //(!CheckIfSpecificTag(gameObject, 0, 0, blockingLayer, "Player") && !CheckIfSpecificTag(gameObject, 0, 0, blockingLayer, "Wall")) )) //???
    //     {
    //         if (notification)
    //         {
    //             Debug.Log(gameObject.ToString() + " " + transform.position.x + " " + transform.position.y);
    //         }
    //         //HighlightedByPlayerUI.GetComponent<SpriteRenderer>().color == HoverHighlightColor
    //         OnTileClick();
    //     }
    //     //Keitimas į movement antru pelės mygtuku iš pradžių tegul išjungia dabartinį gridą
    //     if (isHighlighted && Input.GetMouseButtonDown(1) && activeState != "Movement")
    //     {
    //         var gameInformation = GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>();
    //         if (gameInformation.SelectedCharacter != null)
    //         {
    //             gameInformation.SelectedCharacter.GetComponent<ActionManager>().FindActionByName(activeState).OffTileHover(gameObject);
    //         }
    //         else if (gameInformation.InspectedCharacter != null)
    //         {
    //             gameInformation.InspectedCharacter.GetComponent<ActionManager>().FindActionByName(activeState).OffTileHover(gameObject);
    //         }
    //     }
    //     if (FogOfWarTile.activeSelf)
    //     {
    //         HighlightedByPlayerUI.GetComponent<SpriteRenderer>().sortingLayerName = "UI";
    //     }
    //     else
    //     {
    //         HighlightedByPlayerUI.GetComponent<SpriteRenderer>().sortingLayerName = "Items";
    //     }
        /*
        if (fogOfWar && isFogOfWarEnabled)
        {
            FogOfWarTile.SetActive(true);
        }
        else
        {
            FogOfWarTile.SetActive(false);
            fogOfWar = true;
        }*/
    // }

    public void SetHighlightBool(bool statement)
    {
        ColorGridTile.enabled = statement;
        isHighlighted = statement;
        if (!statement)
        {
            transform.Find("mapTile").Find("Character").gameObject.SetActive(false);
            transform.Find("mapTile").Find("CharacterAlpha").gameObject.SetActive(false);
            transform.Find("mapTile").Find("Direction").gameObject.SetActive(false);
            transform.Find("mapTile").Find("DamageText").gameObject.SetActive(false);
            transform.Find("mapTile").Find("Object").gameObject.SetActive(false);
            transform.Find("mapTile").Find("Death").gameObject.SetActive(false);
            canAbilityTargetAllies = false;
            canAbilityTargetYourself = false;
        }
    }
    void Start()
    {
        // blockingLayer = LayerMask.GetMask("BlockingLayer");
        // FogOfWarTile.SetActive(true);
        // colorManager = GameObject.Find("GameInformation").gameObject.GetComponent<ColorManager>();
        // MovementHighlightColor = colorManager.MovementHighlight;//lol
        // HighlightedByPlayerUI.GetComponent<SpriteRenderer>().color = MovementHighlightColor;
        // NotHoveredColor = HighlightedByPlayerUI.GetComponent<SpriteRenderer>().color;
        // AttackHighlightColor = colorManager.AttackHighlight;
        // OtherHighlight = colorManager.OtherHighlight;
        // InspectionHighlight = colorManager.InspectionHighlight;
        // isFogOfWarEnabled = GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().isFogOfWarEnabled;
    }
    /*void Update()
    {
        if (GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().SelectedCharacter != null)
        {
            activeState = GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().
                SelectedCharacter.GetComponent<PlayerInformation>().currentState;
        }
    }*/
    public void ChangeBaseColor()
    {
        if (GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().InspectedCharacter != null)
        {
            NotHoveredColor = InspectionHighlight;
        }
        else if (activeState == "Movement")
        {
            NotHoveredColor = MovementHighlightColor;
        }
        else if (activeState == "Attack")
        {
            NotHoveredColor = AttackHighlightColor;
        }
        else
        {
            NotHoveredColor = OtherHighlight;
        }
        // if (activeState != "Movement" && GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().InspectedCharacter == null &&
        //     !GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().SelectedCharacter.GetComponent<ActionManager>()
        //     .FindActionByName(activeState).CanTileBeClicked(gameObject.transform.position) || !IsSkillAvailableInFOW())
        // {
        //     NotHoveredColor = (new Color(100f / 255f, 100f / 255f, 100f / 255f) + NotHoveredColor) / 2;
        //     //NotHoveredColor = NotHoveredColor + new Color(-20/255f,-20/255f,-20/255f, -50/255f);
        // }
        ColorGridTile.GetComponent<SpriteRenderer>().color = NotHoveredColor;
    }
    //void OnMouseDown()
    /*void OnMouseDown()
    {
        if (isHighlighted)
        {
            GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().isDragAvailable = false;
        }
        if (!PCControls)
        {
            OnEnter();
        }
    }*/
    /*void OnMouseUp()
    {
        //OnTileClick();
        if (isHighlighted)
        {
            GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().isDragAvailable = true;
        }
    }*/
    public void OnTileClick()
    {
        //OffHover(false);
        var gameInformation = GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>();
        if (!gameInformation.isBoardDisabled && gameInformation.InspectedCharacter == null && IsSkillAvailableInFOW())
        {
            //Jei movement
            if (isHighlighted && activeState == "Movement")
            {
                // UndoAction undoAction = new UndoAction(gameInformation.SelectedCharacter.transform.position,
                    // gameInformation.SelectedCharacter, gameInformation.SelectedCharacter.GetComponent<GridMovement>().MovementPointsToReachDestination(gameObject));
                // gameInformation.undoAction = undoAction;
                // gameInformation.SelectedCharacter.GetComponent<GridMovement>().DisableWayTiles();
                FlipSelectedCharacter(gameInformation);
                gameInformation.MoveSelectedCharacter(gameObject);
                
                // gameInformation.FocusSelectedCharacter(gameInformation.SelectedCharacter);
                // gameInformation.SelectedCharacter.GetComponent<GridMovement>().ClearWayList();
                // GameObject characterToSelect = gameInformation.SelectedCharacter;
                // characterToSelect.GetComponent<GridMovement>().DisableGrid();
                //characterToSelect.GetComponent<GridMovement>().EnableGrid();
                //gameInformation.DeselectTeam(gameInformation.SelectedCharacter); 
                //gameInformation.SelectACharacter(characterToSelect);
                // StartCoroutine(ExecuteAfterTime(0.01f, () =>
                // {
                    //gameInformation.SelectACharacter(characterToSelect);
                    // characterToSelect.GetComponent<GridMovement>().EnableGrid();
                // }));
            }
            //Jei kitas veiksmas
            // else if (isHighlighted
            //     && gameInformation.SelectedCharacter.GetComponent<ActionManager>().FindActionByName(activeState).canTileBeClicked(gameObject))
            // {
            //     FlipSelectedCharacter(gameInformation);
            //     gameInformation.undoAction.available = false;
            //     gameInformation.SelectedCharacter.GetComponent<ActionManager>().FindActionByName(activeState).ResolveAbility(gameObject);
            //     gameInformation.ChangeVisionTiles();
            //
            // }
        }
        //OffHover(false);
    }

    private void FlipSelectedCharacter(GameInformation gameInformation)
    {
        if (transform.position.x > gameInformation.SelectedCharacter.transform.position.x)
        {
            gameInformation.SelectedCharacter.GetComponent<PlayerMovement>().Flip(true);
        }
        else if (transform.position.x < gameInformation.SelectedCharacter.transform.position.x)
        {
            gameInformation.SelectedCharacter.GetComponent<PlayerMovement>().Flip(false);
        }
    }

    /*void OnMouseOver()
    {
        OnHover(false);
    }
    public void OnHover(bool clickedOnPlayer)
    {
        if (PCControls || Input.GetMouseButton(0))
        {
            var gameInformation = GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>();
            if (!gameInformation.isBoardDisabled && GameObject.Find("CM vcam1") != null && !GameObject.Find("CM vcam1").GetComponent<CameraController>().hasDraggingStarted)
            {
                if (activeState != "Movement" && isHighlighted)
                {
                    if (gameInformation.SelectedCharacter != null && gameInformation.SelectedCharacter.GetComponent<ActionManager>().FindActionByName(activeState).CanTileBeClicked(gameObject.transform.position)
                        && IsSkillAvailableInFOW())
                    {
                        HighlightedByPlayerUI.GetComponent<SpriteRenderer>().color = colorManager.MovementHighlightHover;
                        gameInformation.SelectedCharacter.GetComponent<ActionManager>().FindActionByName(activeState).OnTileHover(gameObject);
                    }
                    else if (gameInformation.InspectedCharacter != null && gameInformation.InspectedCharacter.GetComponent<ActionManager>().FindActionByName(activeState).CanTileBeClicked(gameObject.transform.position)
                        && IsSkillAvailableInFOW())
                    {
                        HighlightedByPlayerUI.GetComponent<SpriteRenderer>().color = colorManager.MovementHighlightHover;
                        gameInformation.InspectedCharacter.GetComponent<ActionManager>().FindActionByName(activeState).OnTileHover(gameObject);
                    }
                    else
                    {
                        HighlightedByPlayerUI.GetComponent<SpriteRenderer>().color = NotHoveredColor + new Color(-20 / 255f, -20 / 255f, -20 / 255f);
                    }
                }
                else
                {
                    HighlightedByPlayerUI.GetComponent<SpriteRenderer>().color = colorManager.MovementHighlightHover;
                }
                if (isHighlighted && activeState == "Movement")
                {
                    if (GameObject.Find("VisualCursor") != null)
                    {
                        GameObject.Find("VisualCursor").GetComponent<Image>().sprite = GameObject.Find("VisualCursor").GetComponent<CursorManager>().MovementHover;
                    }
                }
                isHovered = true;
            }
            else
            {
                WasButtonClicked = true;
                OffHover(clickedOnPlayer);
            }
            if (WasButtonClicked && !gameInformation.isBoardDisabled)
            {
                WasButtonClicked = false;
                OnEnter();
            }
        }
    }*/
    /*void OnMouseEnter()
    {
        if (PCControls || Input.GetMouseButton(0))
        {
            OnEnter();
        }
    }
    public void OnEnter()
    {
        var gameInformation = GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>();
        if (!gameInformation.isBoardDisabled && GameObject.Find("CM vcam1") != null && !GameObject.Find("CM vcam1").GetComponent<CameraController>().hasDraggingStarted)
        {
            if (isHighlighted && activeState == "Movement" && gameInformation.SelectedCharacter != null)
            {
                gameInformation.SelectedCharacter.GetComponent<GridMovement>().MakeWayList(gameObject);
                gameInformation.SelectedCharacter.GetComponent<GridMovement>().PreferredWayTile = transform.gameObject;
            }
        }
        else
        {
            OffHover(false);
        }
    }
    void OnMouseExit()
    {
        OffHover(false);
    }
    public void OffHover(bool clickedOnPlayer)
    {
        var gameInformation = GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>();

        HighlightedByPlayerUI.GetComponent<SpriteRenderer>().color = NotHoveredColor;
        if (isHighlighted && activeState == "Movement" && gameInformation.SelectedCharacter != null)
        {
            if (GameObject.Find("VisualCursor") != null)
            {
                GameObject.Find("VisualCursor").GetComponent<Image>().sprite = GameObject.Find("VisualCursor").GetComponent<CursorManager>().Default;
            }
            gameInformation.SelectedCharacter.GetComponent<GridMovement>().DisableWayTiles();
        }
        if (activeState != "Movement" && isHighlighted && gameInformation.SelectedCharacter != null)
        {
            gameInformation.SelectedCharacter.GetComponent<ActionManager>().FindActionByName(activeState).OffTileHover(gameObject);
        }
        else if (activeState != "Movement" && isHighlighted && gameInformation.InspectedCharacter != null)
        {
            gameInformation.InspectedCharacter.GetComponent<ActionManager>().FindActionByName(activeState).OffTileHover(gameObject);
        }
        isHovered = false;
    }*/
    bool IsSkillAvailableInFOW()
    {
        return (!FogOfWarTile.enabled || activeState == "CreateEye" || activeState == "CreatePortal");
    }
    IEnumerator ExecuteAfterTime(float time, Action task)
    {
        yield return new WaitForSeconds(time);
        task();
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
}
