using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private bool ArrowMovement = false;
    private int horizontal = 0;
    private int vertical = 0;
    private LayerMask blockingLayer;
    private LayerMask groundLayer;
    private LayerMask fogLayer;
    private LayerMask whiteFieldLayer;
    public Transform firePoint;
    private bool Hovered;
    private bool isFacingRight = true;
    private Vector3 currentPosition;

    private RaycastHit2D raycast;

    void Start()
    {
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
        if (GetGroundTile(groundLayer).GetComponent<HighlightTile>().isHighlighted && GetComponent<PlayerInformation>().currentState == "Movement" &&
            GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().SelectedCharacter == gameObject)
        {
            GetGroundTile(groundLayer).GetComponent<HighlightTile>().SetHighlightBool(false);
        }
        if (Input.GetMouseButtonUp(0) && Hovered)
        {
            OnTileClick();
        }
        //
        if (ArrowMovement && GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().SelectedCharacter == gameObject)
        {
            horizontal = (int)(Input.GetAxisRaw("Horizontal"));
            vertical = (int)(Input.GetAxisRaw("Vertical"));

            if (horizontal != 0)
            {
                vertical = 0;
            } //nu cia db tai lievai nes jei laikai i sona mygtuka ir spaudineji zemyn aukstyn tai jis i sonus eina 
              //bet pataisysim tiesiog uzdedami laika tarp kada gali spaust judet ir taip pat gi padarysim ta kitoki vaiksciojima.

            if ((Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical")) && !isItBlockingTheWay(horizontal, vertical))
            {
                transform.position += new Vector3(horizontal, vertical, 0f);
            }
            else if ((Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical")))
            {
                transform.position += new Vector3(0f, 0f, 0f);
            }
        }
    }
    public void OnAnyMove() //Update Functions that happen only when any character moves.
    {
        //FOG/WATER
        if (CheckIfSpecificLayer(gameObject, 0, 0, fogLayer) || CheckIfSpecificTag(gameObject, 0, 0, groundLayer, "Water") || GetComponent<PlayerInformation>().Disarmed)
        {
            GetComponent<PlayerInformation>().CantAttackCondition = true;
            //transform.parent.GetComponent<PlayerInformation>().TurnOnCantAttack();
        }
        else
        {
            GetComponent<PlayerInformation>().CantAttackCondition = false;
        }
        //FOG OF WAR
        if (!CheckIfSpecificLayer(gameObject,0,0, groundLayer) || GetGroundTile(groundLayer).GetComponent<HighlightTile>().FogOfWarTile.activeSelf)
        {
            ChangeVisualActiveSelf(false);
        }
        else if(GetComponent<PlayerInformation>().health > 0)
        {
            ChangeVisualActiveSelf(true);
        }
        //WHITE FIELD
        if ((CheckIfSpecificLayer(gameObject, 0, 0, whiteFieldLayer) && GetGroundTile(whiteFieldLayer).GetComponent<WhiteField>().isCharacterProtected(gameObject))
            || GetComponent<PlayerInformation>().MistShield || GetComponent<PlayerInformation>().Debuffs.Contains("Protected"))
        {
            GetComponent<PlayerInformation>().Protected = true;
        }
        else
        {
            GetComponent<PlayerInformation>().Protected = false;
        }
        //DANGER ZONE
        if(CheckIfSpecificLayer(gameObject, 0, 0, groundLayer) && ((GetGroundTile(groundLayer).transform.Find("DangerUI").gameObject.activeSelf 
            || GetGroundTile(groundLayer).transform.Find("mapTile").Find("CometZone").gameObject.activeSelf
            || GetGroundTile(groundLayer).transform.Find("mapTile").Find("PinkZone").gameObject.activeSelf)
            || GetGroundTile(groundLayer).transform.Find("mapTile").Find("OrangeZone").gameObject.activeSelf
            || GetGroundTile(groundLayer).transform.Find("mapTile").Find("GreenZone").gameObject.activeSelf))
        {
            GetComponent<PlayerInformation>().Danger = true;
        }
        else
        {
            GetComponent<PlayerInformation>().Danger = false;
        }
        //Checking if this character has moved
        if (transform.position != currentPosition)
        {
            currentPosition = transform.position;
            if(GetComponent<PlayerInformation>().IsCreatingWhiteField)
            {
                GetComponent<CreateWhiteField>().OnTurnStart();//sunaikina white field
            }
        }
        //Changing Order in layer
        transform.Find("CharacterModel").GetComponent<SpriteRenderer>().sortingOrder = int.Parse(transform.position.y.ToString()) * -10;

     }
    public void ChangeVisualActiveSelf(bool condition)
    {
        transform.Find("CharacterModel").GetComponent<SpriteRenderer>().enabled = condition;
        transform.Find("SelectionUI")?.gameObject.SetActive(condition);
        transform.Find("VFX")?.gameObject.SetActive(condition);
    }
    public void FakeUpdate()
    {
        if (GetComponent<PlayerInformation>().health > 0)
        {
            GetComponent<PlayerInformation>().cornerPortraitBoxInGame.GetComponent<BottomCornerUI>().ChangeCooldownVisuals();
        }
        GetComponent<PlayerInformation>().cornerPortraitBoxInGame.GetComponent<ButtonManager>().UpdateDebuffIcons();
        
        Debug.LogError("Critical need to fix ui doesnt update");
        GetComponent<GridMovement>().FakeUpdate();
    }
    private bool isItBlockingTheWay(int x, int y)
    {
        GetComponent<BoxCollider2D>().enabled = false;
        raycast = Physics2D.Linecast(firePoint.position, (firePoint.position + new Vector3(x, y, 0f)), blockingLayer);
        GetComponent<BoxCollider2D>().enabled = true;
        EnemyInteractionWithAnEnemy(x, y, raycast);
        if (raycast.transform == null)
        {
            return false;
        }
        return true;
    }
    private void EnemyInteractionWithAnEnemy(int x, int y, RaycastHit2D raycast)
    {
        if (raycast.transform != null && (raycast.transform.CompareTag("Player") || raycast.transform.CompareTag("Enemy")))
        {
            if (raycast.transform.gameObject.GetComponent<PlayerInformation>().CharactersTeam != transform.gameObject.GetComponent<PlayerInformation>().CharactersTeam)
            {
                transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
                raycast.transform.gameObject.GetComponent<PlayerInformation>().DealDamage(10, false, gameObject);
            }
        }
    }
    public void OnTileClick() //reikia kad nepasispaustu ir mygtukas ir sitas vienu metu.
    {
        var gameInformation = GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>();
        var tileUnder = GetGroundTile(groundLayer).GetComponent<HighlightTile>();
        if (!gameInformation.isBoardDisabled)
        {
            //To select or not to select this character
            if (!GetGroundTile(groundLayer).GetComponent<HighlightTile>().FogOfWarTile.activeSelf 
                && GetComponent<PlayerInformation>().CharactersTeam == gameInformation.ActiveTeam 
                && ((!tileUnder.canAbilityTargetAllies && gameObject != gameInformation.SelectedCharacter)
                || (!tileUnder.canAbilityTargetYourself && gameObject == gameInformation.SelectedCharacter))) //jei paspaustas veikejas tavo komandoj ir gali buti paspaustas pasirinkimo prasme
            {
                tileUnder.OffHover(true);
                gameInformation.SelectACharacter(gameObject);
            }
            //To inspect or not to inspect this character
            else if (!tileUnder.FogOfWarTile.activeSelf 
                && transform.gameObject.GetComponent<PlayerInformation>().CharactersTeam != gameInformation.ActiveTeam
                && !tileUnder.isHighlighted && !transform.CompareTag("Wall"))
            {
                tileUnder.OffHover(true);
                gameInformation.InspectACharacter(transform.gameObject);
            }
        }
        //Activate tile's actions
        tileUnder.OnTileClick();
        tileUnder.isCharacterOnTop = false;
        if (!CompareTag("Wall"))
        {
            FakeUpdate();
        }
    }
    void OnMouseDown()
    {
        if ((!transform.CompareTag("Wall") || GetGroundTile(groundLayer).GetComponent<HighlightTile>().isHighlighted) && GetGroundTile(groundLayer).GetComponent<HighlightTile>().FogOfWarTile.activeSelf == false)
        {
            GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().isDragAvailable = false;
        }
    }
    void OnMouseUp()
    {
        GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().isDragAvailable = true;
    }
    void OnMouseEnter()
    {
        Hovered = true;
        if (GetGroundTile(groundLayer).GetComponent<HighlightTile>().FogOfWarTile.activeSelf == false) //&& !transform.CompareTag("Wall"))
        {
            if (GetGroundTile(groundLayer) != null)
            {
                GetGroundTile(groundLayer).GetComponent<HighlightTile>().OnHover(true);
                GetGroundTile(groundLayer).GetComponent<HighlightTile>().isCharacterOnTop = true;
            }
            if (GameObject.Find("VisualCursor") != null)
            {
                GameObject.Find("VisualCursor").GetComponent<Image>().sprite = GameObject.Find("VisualCursor").GetComponent<CursorManager>().CharacterHover;
            }
        }
    }
    void OnMouseOver()
    {
        if (GetGroundTile(groundLayer) != null)
        {
            GetGroundTile(groundLayer).GetComponent<HighlightTile>().OnHover(true);
            GetGroundTile(groundLayer).GetComponent<HighlightTile>().isCharacterOnTop = true;
        }
    }
    void OnMouseExit()
    {
        Hovered = false;
        if (GetGroundTile(groundLayer) != null) //&& GetGroundTile(groundLayer).GetComponent<HighlightTile>().isActive == true
        {
            GetGroundTile(groundLayer).GetComponent<HighlightTile>().OffHover(true);
            GetGroundTile(groundLayer).GetComponent<HighlightTile>().isCharacterOnTop = false;
        }
        if (GameObject.Find("VisualCursor") != null)
        {
            GameObject.Find("VisualCursor").GetComponent<Image>().sprite = GameObject.Find("VisualCursor").GetComponent<CursorManager>().Default;
        }
    }
    private GameObject GetGroundTile(LayerMask chosenLayer)
    {
        Vector3 firstPosition = firePoint.transform.position;
        Vector3 secondPosition = firstPosition + new Vector3(0.1f, 0f, 0f);
        raycast = Physics2D.Linecast(firstPosition, secondPosition, chosenLayer);
        if (raycast.transform == null)
        {
            return null;
        }
        return raycast.transform.gameObject;
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
    public void Flip(bool shouldCharacterFaceRight)
    {
        if(isFacingRight != shouldCharacterFaceRight)
        {
            isFacingRight = !isFacingRight;
            transform.Find("CharacterModel").Rotate(0f, 180f, 0f);
        }
    }
}
