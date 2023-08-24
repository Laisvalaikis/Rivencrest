using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SummonOrb : BaseAction
{
    //private string actionStateName = "SummonOrb";
    public GameObject OrbPrefab;
    //public int minAttackDamage = 4;
    //public int maxAttackDamage = 6;
    //private int i = 0;


    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();
    //private List<GameObject> MergedTileList = new List<GameObject>();

    void Start()
    {
        actionStateName = "SummonOrb";
        isAbilitySlow = false;
    }
    /*
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
            if (isGroundLayer && (!isBlockingLayer || isPlayer))
            {
                GameObject AddableObject = GetSpecificGroundTile(middleTile, x.Item1, x.Item2, groundLayer);
                this.AvailableTiles[movementIndex].Add(AddableObject);
            }
        }
    }

    public override void EnableGrid()
    {
        if (canGridBeEnabled())
        {
            CreateGrid();
            HighlightAll();
        }
    }
    public override void CreateGrid()
    {
        transform.gameObject.GetComponent<PlayerInformation>().currentState = actionStateName;
        this.AvailableTiles.Clear();
        if (AttackRange > 0)
        {
            this.AvailableTiles.Add(new List<GameObject>());
            AddSurroundingsToList(transform.gameObject, 0);
        }

        for (int i = 1; i <= AttackRange - 1; i++)
        {
            this.AvailableTiles.Add(new List<GameObject>());

            foreach (var tileInPreviousList in this.AvailableTiles[i - 1])
            {
                AddSurroundingsToList(tileInPreviousList, i);
            }
        }
        //Merging into one list
        MergedTileList.Clear();
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                if (!MergedTileList.Contains(tile))
                {
                    MergedTileList.Add(tile);
                }
            }
        }
    }
    public override void DisableGrid()
    {
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                tile.GetComponent<HighlightTile>().SetHighlightBool(false);
                tile.GetComponent<HighlightTile>().canAbilityTargetAllies = false;
            }
        }
    }
    public void HighlightAll()
    {
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                //bool isBlockingLayer = CheckIfSpecificLayer(tile, 0, 0, blockingLayer);
                //bool isPlayer = CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player");
                //if (!isBlockingLayer) //|| isPlayer)
                //{
                tile.GetComponent<HighlightTile>().SetHighlightBool(true);
                tile.GetComponent<HighlightTile>().canAbilityTargetAllies = true;
                tile.GetComponent<HighlightTile>().activeState = actionStateName;
                tile.GetComponent<HighlightTile>().ChangeBaseColor();
                // }
            }
        }
        GetSpecificGroundTile(transform.gameObject, 0, 0, groundLayer).GetComponent<HighlightTile>().SetHighlightBool(false);
    }
    public void HighlightOuter()
    {
        if (AttackRange != 0)
        {
            foreach (GameObject tile in this.AvailableTiles[this.AvailableTiles.Count - 1])
            {
                tile.GetComponent<HighlightTile>().SetHighlightBool(true);
                tile.GetComponent<HighlightTile>().canAbilityTargetAllies = true;
                tile.GetComponent<HighlightTile>().activeState = actionStateName;
                tile.GetComponent<HighlightTile>().ChangeBaseColor();
            }
            for (int i = this.AvailableTiles.Count - 1; i > 0; i--)
            {
                foreach (GameObject tile in this.AvailableTiles[i - 1])
                {
                    tile.GetComponent<HighlightTile>().SetHighlightBool(false);
                }
            }
            GetSpecificGroundTile(transform.gameObject, 0, 0, groundLayer).GetComponent<HighlightTile>().SetHighlightBool(false);
        }
    }
    */
    public override void OnTurnStart()//pradzioj ejimo
    {
        if(spawnedCharacter != null && spawnedCharacter.GetComponent<PlayerInformation>().health > 0)
        {
            //spawnedCharacter.GetComponent<PlayerInformation>().DealDamage(5, false, spawnedCharacter);
 
            spawnedCharacter.GetComponent<PlayerInformation>().DeathStart();
            spawnedCharacter.transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell2");
    
            StartCoroutine(ExecuteAfterTime(0.2f, () =>
            {
                //EXPLOSION
                //
                var pushDirectionVectors = new List<(int, int)>
                {
                    (1, 0),
                    (0, 1),
                    (-1, 0),
                    (0, -1)
                };
                foreach (var x in pushDirectionVectors)
                {
                    if (CheckIfSpecificLayer(spawnedCharacter, x.Item1, x.Item2, groundLayer) && !CheckIfSpecificTag(spawnedCharacter, x.Item1, x.Item2, blockingLayer, "Wall")) //animation on ground
                    {
                        GetSpecificGroundTile(spawnedCharacter, x.Item1, x.Item2, groundLayer).transform.Find("mapTile").Find("VFXImpactBelow").gameObject.GetComponent<Animator>().SetTrigger("forest4");
                    }
                    if (CheckIfSpecificTag(spawnedCharacter, x.Item1, x.Item2, blockingLayer, "Player"))
                    {
                        GameObject target = GetSpecificGroundTile(spawnedCharacter, x.Item1, x.Item2, blockingLayer);
                        int randomDamage = UnityEngine.Random.Range(minAttackDamage, maxAttackDamage);
                        bool crit = IsItCriticalStrike(ref randomDamage);
                        dodgeActivation(ref randomDamage, target);
                        if (isAllegianceSame(target))
                        {
                            randomDamage /= 3;
                        }
                        target.GetComponent<PlayerInformation>().DealDamage(randomDamage, crit, gameObject);
                    }
                }
                //
            }));
            //
        }
    }
    public override void ResolveAbility(Vector3 position)
    {
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
            //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("createFog");
            spawnedCharacter = Instantiate(OrbPrefab, position - new Vector3(0f, 0f, 1f), Quaternion.identity) as GameObject;
            spawnedCharacter.transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell1");
            spawnedCharacter.GetComponent<PlayerInformation>().wasThisCharacterSpawned = true;
            spawnedCharacter.GetComponent<PlayerInformation>().isThisObject = true;
            //spawnedObject.GetComponent<PlayerInformation>().wasThisCharacterSpawned = true;
            //spawnedBear.GetComponent<PlayerInformation>().ApplyDebuff("Stun");
            GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().ChangeVisionTiles();
            //i = 0;
            FinishAbility();
            // GameObject.Find("GameInformation").gameObject.GetComponent<PlayerTeams>().AddCharacterToCurrentTeam(spawnedBear);
        }
    }

    public override bool CanTileBeClicked(Vector3 position)
    {
       // var gameInformation = GameObject.Find("GameInformation").gameObject;
       // bool isTeamNotFull = gameInformation.GetComponent<PlayerTeams>().allCharacterList.teams[gameInformation.GetComponent<GameInformation>().activeTeamIndex].characters.Count < 8;
        bool isBlockingLayer = CheckIfSpecificLayer(position, 0, 0, blockingLayer);

        if (!isBlockingLayer)
        {
            return true;
        }
        return false;
    }
    public override void OnTileHover(GameObject tile)
    {
        //tile.transform.Find("mapTile").Find("CharacterAlpha").gameObject.SetActive(true);
        // tile.transform.Find("mapTile").Find("CharacterAlpha").gameObject.GetComponent<SpriteRenderer>().sprite = BearPrefab.transform.Find("CharacterModel").GetComponent<SpriteRenderer>().sprite;
    }
    public override void OffTileHover(GameObject tile)
    {
        //tile.transform.Find("mapTile").Find("CharacterAlpha").gameObject.SetActive(false);
    }
    public override GameObject PossibleAIActionTile()
    {
        List<GameObject> PossibleTileList = new List<GameObject>();
        if (CanGridBeEnabled())
        {
            CreateGrid();
            foreach (GameObject tile in MergedTileList)
            {
                if (CanTileBeClicked(tile.transform.position) && AreEnemiesNearbyTile(tile))
                {
                    GameObject possibleTile = GetSpecificGroundTile(tile, 0, 0, groundLayer);
                    PossibleTileList.Add(possibleTile);
                }
            }
        }
        int actionChanceNumber = UnityEngine.Random.Range(0, 100); //ar paleist spella ar ne
        if (PossibleTileList.Count > 0 && actionChanceNumber <= 100)
        {
            return PossibleTileList[Random.Range(0, PossibleTileList.Count - 1)];
        }
        return null;
    }
    private bool AreEnemiesNearbyTile(GameObject tile)
    {
        //
        var pushDirectionVectors = new List<(int, int)>
                {
                    (1, 0),
                    (0, 1),
                    (-1, 0),
                    (0, -1)
                };
        foreach (var x in pushDirectionVectors)
        {
            if (CheckIfSpecificLayer(tile, x.Item1, x.Item2, groundLayer) && CheckIfSpecificTag(tile, x.Item1, x.Item2, blockingLayer, "Player")
                && !isAllegianceSame(GetSpecificGroundTile(tile, x.Item1, x.Item2, blockingLayer)))
            {
                return true;
            }
        }
        return false;
    }
    IEnumerator ExecuteAfterTime(float time, System.Action task)
    {
        yield return new WaitForSeconds(time);
        task();
    }
}
