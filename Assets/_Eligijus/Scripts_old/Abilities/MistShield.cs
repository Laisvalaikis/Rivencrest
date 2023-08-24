using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistShield : BaseAction
{
    //private string actionStateName = "MistShield";

    private bool isAbilityActive = false;

    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();

    void Start()
    {
        actionStateName = "MistShield";
        isAbilitySlow = false;
    }
    private void AddSurroundingsToList(GameObject middleTile, int movementIndex)
    {
        GameObject addableObject = GetSpecificGroundTile(middleTile, 0, 0, groundLayer); //kad galima butu pasirinkt tik save
        this.AvailableTiles[movementIndex].Add(addableObject);
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
        this.AvailableTiles.Add(new List<GameObject>());
        AddSurroundingsToList(transform.gameObject, 0);
        MergeIntoOneList();
    }
    /*
    public override void DisableGrid()
    {
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                tile.GetComponent<HighlightTile>().canAbilityTargetAllies = false;
                tile.GetComponent<HighlightTile>().canAbilityTargetYourself = false;
                tile.GetComponent<HighlightTile>().SetHighlightBool(false);
            }
        }
    }
    */

    public override void HighlightAll()
    {
        foreach (List<GameObject> movementTileList in AvailableTiles)
        {
            foreach (GameObject tile in movementTileList)
            {
                tile.GetComponent<HighlightTile>().SetHighlightBool(true);
                tile.GetComponent<HighlightTile>().activeState = actionStateName;
                tile.GetComponent<HighlightTile>().ChangeBaseColor();
                tile.GetComponent<HighlightTile>().canAbilityTargetAllies = true;
                tile.GetComponent<HighlightTile>().canAbilityTargetYourself = true;
            }
        }
    }
    public override void OnTurnStart()//pradzioj ejimo
    {
        if (isAbilityActive)
        {
            GetComponent<PlayerInformation>().MistShield = false;
        }
        isAbilityActive = false;
        StartCoroutine(ExecuteAfterTime(0.5f, () =>
        {
            // GetComponent<PlayerMovement>().OnAnyMove();
            GetComponent<PlayerInformation>().Protected = false;
        }));
    }
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
            isAbilityActive = true;
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell1");
            GetComponent<PlayerInformation>().MistShield = true;
            GetComponent<PlayerInformation>().Protected = true;
            transform.Find("VFX").Find("Protected").gameObject.SetActive(true);
            FinishAbility();
            // GetComponent<PlayerMovement>().OnAnyMove();
    }

    public override GameObject PossibleAIActionTile()
    {
        bool isActionPossible = false;
        if (CanGridBeEnabled())
        {
            List<GameObject> characterList = GetComponent<AIBehaviour>().GetCharactersInGrid(2);
            foreach (GameObject character in characterList)
            {
                if (!isAllegianceSame(character))
                {
                    isActionPossible = true;
                    
                }
            }
        }
        int actionChanceNumber = UnityEngine.Random.Range(0, 100); //ar paleist spella ar ne
        if (isActionPossible && actionChanceNumber <= 80)
        {
            return gameObject;
        }
        return null;
    }
        IEnumerator ExecuteAfterTime(float time, Action task)
    {
        yield return new WaitForSeconds(time);
        task();
    }
}
