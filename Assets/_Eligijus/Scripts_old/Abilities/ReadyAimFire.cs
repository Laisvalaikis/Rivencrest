using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReadyAimFire : BaseAction
{
    private int aimDirection = -1;
    void Start()
    {
        laserGrid = true;
        actionStateName = "ReadyAimFire";
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
        if ((CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player")) && !isAllegianceSame(tile))
        {
            return true;
        }
        return false;
    }

    public override void OnTurnStart()//pradzioj ejimo
    {
        if (aimDirection != -1)
        {
            ShootInDirection(aimDirection);
            transform.Find("CharacterModel").GetComponent<Animator>().SetBool("block", false);
            transform.Find("VFX").GetComponent<VFXContainer>().AimArrow.GetComponent<Animator>().SetBool("aim", false);
            //GetComponent<PlayerInformation>().Blocker = false;
            aimDirection = -1;
        }
    }
    //
    private void ShootInDirection(int directionIndex)
    {
        var directionVectors = new List<(int, int)>
             {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
            };
        for (int i = 0; i < AttackRange; i++)
        {
            int x = directionVectors[directionIndex].Item1 * (i + 1);
            int y = directionVectors[directionIndex].Item2 * (i + 1);
            bool isGround = CheckIfSpecificLayer(gameObject, x, y, groundLayer);
            bool isBlockingLayer = CheckIfSpecificLayer(gameObject, x, y, blockingLayer);
            bool isPlayer = CheckIfSpecificTag(gameObject, x, y, blockingLayer, "Player");

            // If a player blocks the shot
            if (isGround && isPlayer)
            {
                GameObject target = GetSpecificGroundTile(gameObject, x, y, blockingLayer);
                GameObject ground = GetSpecificGroundTile(gameObject, x, y, groundLayer);
                ground.transform.Find("mapTile").Find("VFXImpact").gameObject.GetComponent<Animator>().SetTrigger("bandit2");
                //if (!isAllegianceSame(target)) 
                //{
                DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
                // }
                return;
            }
            // If a wall blocks the shot
            else if (isBlockingLayer) 
            {
                return;
            }
        }
    }
    //
    public override void ResolveAbility(GameObject clickedTile)
    {
        if (canTileBeClicked(clickedTile))
        {
            aimDirection = FindIndexOfTile(clickedTile);
            GameObject target = GetSpecificGroundTile(clickedTile, 0, 0, blockingLayer);
            transform.Find("CharacterModel").GetComponent<Animator>().SetBool("block", true);
            StartCoroutine(ActivateAimArrow(0.45f));
            
            //clickedTile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("green1");
            //
            //DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
            FinishAbility();
        }
    }

    IEnumerator ActivateAimArrow(float secs)
    {
        GameObject AimVFX = transform.Find("VFX").GetComponent<VFXContainer>().AimArrow;
        AimVFX.SetActive(true);
        yield return new WaitForSeconds(secs);
        Animator AimAnimator = AimVFX.GetComponent<Animator>();
        AimAnimator.SetBool("aim", true);
        AimAnimator.SetFloat("Direction", aimDirection + 1);
        AimAnimator.SetInteger("DirectionInt", aimDirection + 1);
    }
    public override void OnTileHover(GameObject tile)
    {
        if (!isAllegianceSame(tile)) //jei priesas, tada jam ikirs
        {
            EnableDamagePreview(tile, minAttackDamage, maxAttackDamage);
        }
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
                    if (canTileBeClicked(tile))
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
}