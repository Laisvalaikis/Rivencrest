using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class AIManager : MonoBehaviour
{
    public bool RespawnEnemyWaves;
    public int RespawnCount;
    public List<Vector3> AIStartingDestinationList;
    public List<Vector3> AIDestinations;
    public List<GameObject> EnemySpawnPoints;
    public List<GameObject> EnemyPrefabs;
    private RaycastHit2D raycast;
    public LayerMask blockingLayer;
    public LayerMask groundLayer;
    public GameObject cornerUIManager;
    public Data _data;
    private GameInformation _gameInformation;
    private PlayerTeams _playerTeams;
    private void Awake()
    {
        _gameInformation = GetComponent<GameInformation>();
        _playerTeams = GetComponent<PlayerTeams>();
        if (AIStartingDestinationList.Count > 0)
        {
            Vector3 startingDestination = AIStartingDestinationList[Random.Range(0, AIStartingDestinationList.Count - 1)];
            AIDestinations.Insert(0, startingDestination);
        }
    }
    private void Start()
    {   if(_data==null)
        {
            _data = Data.Instance; 
        } 
    }
    public void SpawnEnemiesAtSpawnPoints(int enemyCount, int teamIndex)
    {
    if (RespawnEnemyWaves && RespawnCount > 0)
    {
        HealTeamsExceptOne(teamIndex);
        List<GameObject> enemySpawnPointsCopy = new List<GameObject>(EnemySpawnPoints);
        for (int i = 0; i < enemyCount; i++)
        {
            StartCoroutine(ExecuteAfterTime(0.5f + i, () =>
            {
                int index = Random.Range(0, enemySpawnPointsCopy.Count);
                Vector3 position = enemySpawnPointsCopy[index].transform.position;
                if (!CheckIfSpecificLayer(enemySpawnPointsCopy[index], 0, 0, blockingLayer))
                {
                    GameObject spawnedEnemy = Instantiate(EnemyPrefabs[Random.Range(0, EnemyPrefabs.Count)], position, Quaternion.identity);
                    if (CheckIfSpecificLayer(enemySpawnPointsCopy[index], 0, 0, groundLayer) &&
                    !GetSpecificGroundTile(enemySpawnPointsCopy[index], 0, 0, groundLayer).GetComponent<HighlightTile>().FogOfWarTile.activeSelf)
                    {
// <<<<<<< HEAD
//                         GameObject spawnedEnemy = Instantiate(EnemyPrefabs[UnityEngine.Random.Range(0, EnemyPrefabs.Count)], position, Quaternion.identity);
//                         if (CheckIfSpecificLayer(EnemySpawnPointsCopy[index], 0, 0, groundLayer) &&
//                         !GetSpecificGroundTile(EnemySpawnPointsCopy[index], 0, 0, groundLayer).GetComponent<HighlightTile>().FogOfWarTile.activeSelf)
//                         {
//                             GetComponent<GameInformation>().FocusSelectedCharacter(spawnedEnemy);
//                         }
//                         else
//                         {
//                             // spawnedEnemy.GetComponent<PlayerMovement>().ChangeVisualActiveSelf(false);
//                         }
//                         //
//                         //spawnedEnemy.GetComponent<PlayerInformation>().CharactersTeam = PlayerTeams.allCharacterList.teams[teamIndex].teamName;
//                         // PlayerTeams.allCharacterList.teams[teamIndex].characters.Add(spawnedEnemy);
//                         GetComponent<PlayerTeams>().AddCharacterToCurrentTeam(spawnedEnemy);
//                         //
//                         /*
//                         GameObject portraitBoxPrefab = PlayerTeams.portraitBoxPrefab;
//                         Transform portraitBoxContainer = PlayerTeams.portraitBoxContainer;
//                         GameObject portraitBox = Instantiate(portraitBoxPrefab, portraitBoxContainer);
//                         portraitBox.GetComponent<Image>().sprite = PlayerTeams.allCharacterList.teams[teamIndex].teamPortraitBoxSprite;
//                         portraitBox.transform.GetChild(i).GetComponent<CharacterSelect>().characterOnBoard = spawnedEnemy;
//                         portraitBox.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite =
//                             spawnedEnemy.GetComponent<PlayerInformation>().CharacterPortraitSprite;
//                         */
//                         //
//                         cornerUIManager.GetComponent<ButtonManager>().CharacterOnBoard = spawnedEnemy;
//                         spawnedEnemy.GetComponent<PlayerInformation>().cornerPortraitBoxInGame = cornerUIManager;
//                         spawnedEnemy.GetComponent<AIBehaviour>()._data = _data;
//                         spawnedEnemy.GetComponent<GameInformation>()._data = _data;
//                         //
//                         spawnedEnemy.GetComponent<PlayerInformation>().Respawn = true;
//                         if (spawnedEnemy.transform.Find("VFX").Find("VFX9x9").GetComponent<Animator>().isActiveAndEnabled)
//                         {
//                             spawnedEnemy.transform.Find("VFX").Find("VFX9x9").GetComponent<Animator>().SetTrigger("enemySpawn");
//                         }
//                         if (spawnedEnemy.transform.Find("CharacterModel").GetComponent<Animator>().isActiveAndEnabled)
//                         {
//                             spawnedEnemy.transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerHit");
//                         }
// =======
                        _gameInformation.FocusSelectedCharacter(spawnedEnemy);
// >>>>>>> e03c1ef01d39b95daa08a229f2ef4e3878a4258c
                    }
                    else
                    {
                        // spawnedEnemy.GetComponent<PlayerMovement>().ChangeVisualActiveSelf(false);
                    }
                    
                    InitializeEnemy(spawnedEnemy);
                }
                else if (CheckIfSpecificTag(enemySpawnPointsCopy[index], 0, 0, blockingLayer, "Player"))
                {
                    GetSpecificGroundTile(enemySpawnPointsCopy[index], 0, 0, blockingLayer).GetComponent<PlayerInformation>().DealDamage(5, false, gameObject);
                }
                _gameInformation.FakeUpdate();
                enemySpawnPointsCopy.RemoveAt(index);
            }));
        }
        StartCoroutine(ExecuteAfterTime(0.5f + enemyCount, () =>
        {
            _gameInformation.EndAITurn(true);
        }));
        RespawnCount--;
    }
    }

private void InitializeEnemy(GameObject spawnedEnemy)
{
    _playerTeams.AddCharacterToCurrentTeam(spawnedEnemy);
    cornerUIManager.GetComponent<ButtonManager>().CharacterOnBoard = spawnedEnemy;
    var playerInfo = spawnedEnemy.GetComponent<PlayerInformation>();
    //Animatoriai dar neegzistuoja, tai dabar paliekam find
    Debug.Log("Panaikinti finda, kai animatoriai bus sukurti");
    var vfxAnimator = spawnedEnemy.transform.Find("VFX").Find("VFX9x9").GetComponent<Animator>();
    var characterModelAnimator = spawnedEnemy.transform.Find("CharacterModel").GetComponent<Animator>();

    playerInfo.cornerPortraitBoxInGame = cornerUIManager;
    playerInfo.Respawn = true;
    
    if (vfxAnimator != null && vfxAnimator.isActiveAndEnabled)
    {
        vfxAnimator.SetTrigger("enemySpawn");
    }

    if (characterModelAnimator != null && characterModelAnimator.isActiveAndEnabled)
    {
        characterModelAnimator.SetTrigger("playerHit");
    }
}

    private static RaycastHit2D GetRaycastHit(GameObject tile, int x, int y, LayerMask chosenLayer)
    {
        Vector3 firstPosition = tile.transform.position + new Vector3(0f, 0.5f, 0f) + new Vector3(x, y, 0f);
        Vector3 secondPosition = firstPosition + new Vector3(0.1f, 0f, 0f);
        return Physics2D.Linecast(firstPosition, secondPosition, chosenLayer);
    }
    private Transform GetSpecificGroundTile(GameObject tile, int x, int y, LayerMask chosenLayer)
    {
        raycast = GetRaycastHit(tile, x, y, chosenLayer);
        return raycast.transform;
    }

    private bool CheckIfSpecificLayer(GameObject tile, int x, int y, LayerMask chosenLayer)
    {
        raycast = GetRaycastHit(tile, x, y, chosenLayer);
        return ReferenceEquals(raycast.transform, null);
    }
    private bool CheckIfSpecificTag(GameObject tile, int x, int y, LayerMask chosenLayer, string tagName)
    {
        raycast = GetRaycastHit(tile, x, y, chosenLayer);
        if (ReferenceEquals(raycast.transform, null))
        {
            return false;
        }
        return raycast.transform.CompareTag(tagName);
    }
    //teamIndex is hardcoded as 1. Index of 1 is merchant team used for merchant mission (??)
    private void HealTeamsExceptOne(int teamIndex)
    {
        for (int i = 0; i < _playerTeams.allCharacterList.teams.Count; i++)
        {
            if (i != teamIndex)
            {
                for(int j = 0; j < _playerTeams.allCharacterList.teams[i].characters.Count; j++) //dabartine komanda
                {
                    GameObject characterInList = _playerTeams.allCharacterList.teams[i].characters[j];
                    PlayerInformation playerInformation = characterInList.GetComponent<PlayerInformation>();
                    if (playerInformation.health > 0)
                    {
                        playerInformation.Heal(10, false);
                    }
                }
            }
        }
    }
    IEnumerator ExecuteAfterTime(float time, Action task)
    {
        yield return new WaitForSeconds(time);
        task();
    }
}