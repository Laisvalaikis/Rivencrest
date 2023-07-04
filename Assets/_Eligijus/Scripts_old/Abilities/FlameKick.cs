using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlameKick : BaseAction
{
    //private string actionStateName = "FlameKick";
    //public int minAttackDamage = 3;
    //public int maxAttackDamage = 4;
    private GameObject previewTile;
    private GameObject previewTarget;
    private GameObject secondaryTarget;
    private Color alphaColor = new Color(1, 1, 1, 110 / 255f);
    private bool canTileBeHovered = true;


    void Start()
    {
        actionStateName = "FlameKick";
        isAbilitySlow = false;
    }
    public override void ResolveAbility(GameObject clickedTile)
    {
        
        if (canTileBeClicked(clickedTile))
        {
            base.ResolveAbility(clickedTile);
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell2");
            GameObject target = GetSpecificGroundTile(clickedTile, 0, 0, blockingLayer);
            bool isThisEnemy = !isAllegianceSame(target);
            int bonusDamage = 0;
            // // Push
            Vector3 pushDirection = target.transform.position - gameObject.transform.position;
            // If pushed into another enemy
            if (DoesCharacterHaveBlessing("Martial artist") && //if you have the blessing
                CheckIfSpecificLayer(target, (int)pushDirection.x, (int)pushDirection.y, groundLayer) && //if there is ground
                CheckIfSpecificTag(target, (int)pushDirection.x, (int)pushDirection.y, blockingLayer, "Player") && //if there is a character
                !isAllegianceSame(GetSpecificGroundTile(target, (int)pushDirection.x, (int)pushDirection.y, blockingLayer))) //if it is an enemy
            {
                //Blessing MARTIAL ARTIST
                GameObject secondTarget = GetSpecificGroundTile(target, (int)pushDirection.x, (int)pushDirection.y, blockingLayer);
                DealRandomDamageToTarget(secondTarget, minAttackDamage, maxAttackDamage);
                secondTarget.GetComponent<PlayerInformation>().Aflame = gameObject;
                //Blessing COMBO KICK
                if (DoesCharacterHaveBlessing("Combo kick"))
                {
                    bonusDamage = 3;
                }

            }
            // // Deal damage to original target
            if (isThisEnemy)
            {
                DealRandomDamageToTarget(target, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
                target.GetComponent<PlayerInformation>().Aflame = gameObject;
            }
            if (CheckIfSpecificLayer(target, (int)pushDirection.x, (int)pushDirection.y, groundLayer)
                && !CheckIfSpecificLayer(target, (int)pushDirection.x, (int)pushDirection.y, blockingLayer))
            {
                target.transform.position += pushDirection;
            }
            FinishAbility();
        }
    }
    public override bool canTileBeClicked(GameObject tile)
    {
        if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player"))
        {
            return true;
        }

        return false;
    }
    public override void OnTileHover(GameObject tile)
    {
        if (canTileBeHovered)
        {
            Vector3 pushDirection = tile.transform.position - transform.position;
            bool isEnemy = !isAllegianceSame(GetSpecificGroundTile(tile, 0, 0, blockingLayer));
            //if (!isAllegianceSame(GetSpecificGroundTile(tile, 0, 0, blockingLayer)))
            //{
            //    EnableDamagePreview(tile, minAttackDamage, maxAttackDamage);
            //}
            if (CheckIfSpecificLayer(tile, (int)pushDirection.x, (int)pushDirection.y, groundLayer))
            {
                previewTile = GetSpecificGroundTile(tile, (int)pushDirection.x, (int)pushDirection.y, groundLayer);
                previewTarget = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                if (!CheckIfSpecificLayer(tile, (int)pushDirection.x, (int)pushDirection.y, blockingLayer))
                {
                    previewTile.transform.Find("mapTile").Find("Character").gameObject.SetActive(true);
                    previewTile.transform.Find("mapTile").Find("Character").GetComponent<SpriteRenderer>().sprite = GetSpecificGroundTile(tile, 0, 0, blockingLayer).transform.Find("CharacterModel").GetComponent<SpriteRenderer>().sprite;
                    previewTarget.transform.Find("CharacterModel").GetComponent<SpriteRenderer>().color = alphaColor;
                    if (isEnemy) EnableDamagePreview(previewTile, previewTarget, minAttackDamage, maxAttackDamage);
                }
                else if (isEnemy)
                {
                    var bonusDamage = 0;
                    if (CheckIfSpecificTag(previewTile, 0, 0, blockingLayer, "Player") &&
                        !isAllegianceSame(GetSpecificGroundTile(previewTile, 0, 0, blockingLayer)) &&
                        DoesCharacterHaveBlessing("Martial artist"))
                    {
                        secondaryTarget = GetSpecificGroundTile(previewTile, 0, 0, blockingLayer);
                        EnableDamagePreview(previewTile, secondaryTarget, minAttackDamage, maxAttackDamage);
                        if(DoesCharacterHaveBlessing("Combo kick"))
                        {
                            bonusDamage = 3;
                        }
                    }
                    EnableDamagePreview(tile, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
                }
            }
        }
    }
    public override void OffTileHover(GameObject tile)
    {
        if (previewTile != null)
        {
            previewTile.transform.Find("mapTile").Find("Character").gameObject.SetActive(false);
            DisablePreview(previewTile);
            previewTile = null;
        }
        if (previewTarget != null)
        {
            previewTarget.transform.Find("CharacterModel").GetComponent<SpriteRenderer>().color = Color.white;
            DisablePreview(GetSpecificGroundTile(previewTarget, 0, 0, groundLayer));
            previewTarget = null;
        }
        if (secondaryTarget != null)
        {
            DisablePreview(GetSpecificGroundTile(secondaryTarget, 0, 0, groundLayer));
            secondaryTarget = null;
        }
        canTileBeHovered = false;
        StartCoroutine(ExecuteAfterFrames(1, () =>
        {
            canTileBeHovered = true;
        }));
    }
    public override bool canPreviewBeShown(GameObject tile)
    {
        return true;
    }
    public override GameObject PossibleAIActionTile()
    {
        List<GameObject> EnemyCharacterList = new List<GameObject>();
        if (canGridBeEnabled())
        {
            CreateGrid();
            foreach (GameObject tile in MergedTileList)
            {
                if (canTileBeClicked(tile))
                {
                    GameObject character = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                    EnemyCharacterList.Add(character);
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
