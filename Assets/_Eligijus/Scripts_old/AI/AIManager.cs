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
    public List<GameObject> AIStartingDestinationList;
    public List<GameObject> AIDestinations;
    public List<GameObject> EnemySpawnPoints;
    public List<GameObject> EnemyPrefabs;
    private RaycastHit2D raycast;
    public LayerMask blockingLayer;
    public LayerMask groundLayer;
    public GameObject cornerUIManager;
    public Data _data;

    void Awake()
    {
        if (AIStartingDestinationList.Count > 0)
        {
            GameObject StartingDestination = AIStartingDestinationList[Random.Range(0, AIStartingDestinationList.Count - 1)];
            AIDestinations.Insert(0, StartingDestination);
        }
    }
    public void SpawnEnemiesAtSpawnPoints(int enemyCount, int teamIndex)
    {
        if (RespawnEnemyWaves && RespawnCount > 0)
        {
            HealTeamsExceptOne(teamIndex);
            List<GameObject> EnemySpawnPointsCopy = new List<GameObject>(EnemySpawnPoints);
            var PlayerTeams = GetComponent<PlayerTeams>();
            for (int i = 0; i < enemyCount; i++)
            {
                StartCoroutine(ExecuteAfterTime(0.5f + i, () =>
                {
                    int index = UnityEngine.Random.Range(0, EnemySpawnPointsCopy.Count);
                    Vector3 position = EnemySpawnPointsCopy[index].transform.position;
                    if (!CheckIfSpecificLayer(EnemySpawnPointsCopy[index], 0, 0, blockingLayer))
                    {
                        GameObject spawnedEnemy = Instantiate(EnemyPrefabs[UnityEngine.Random.Range(0, EnemyPrefabs.Count)], position, Quaternion.identity);
                        if (CheckIfSpecificLayer(EnemySpawnPointsCopy[index], 0, 0, groundLayer) &&
                        !GetSpecificGroundTile(EnemySpawnPointsCopy[index], 0, 0, groundLayer).GetComponent<HighlightTile>().FogOfWarTile.activeSelf)
                        {
                            GetComponent<GameInformation>().FocusSelectedCharacter(spawnedEnemy);
                        }
                        else
                        {
                            spawnedEnemy.GetComponent<PlayerMovement>().ChangeVisualActiveSelf(false);
                        }
                        //
                        //spawnedEnemy.GetComponent<PlayerInformation>().CharactersTeam = PlayerTeams.allCharacterList.teams[teamIndex].teamName;
                        // PlayerTeams.allCharacterList.teams[teamIndex].characters.Add(spawnedEnemy);
                        GetComponent<PlayerTeams>().AddCharacterToCurrentTeam(spawnedEnemy);
                        //
                        /*
                        GameObject portraitBoxPrefab = PlayerTeams.portraitBoxPrefab;
                        Transform portraitBoxContainer = PlayerTeams.portraitBoxContainer;
                        GameObject portraitBox = Instantiate(portraitBoxPrefab, portraitBoxContainer);
                        portraitBox.GetComponent<Image>().sprite = PlayerTeams.allCharacterList.teams[teamIndex].teamPortraitBoxSprite;
                        portraitBox.transform.GetChild(i).GetComponent<CharacterSelect>().characterOnBoard = spawnedEnemy;
                        portraitBox.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite =
                            spawnedEnemy.GetComponent<PlayerInformation>().CharacterPortraitSprite;
                        */
                        //
                        cornerUIManager.GetComponent<ButtonManager>().CharacterOnBoard = spawnedEnemy;
                        spawnedEnemy.GetComponent<PlayerInformation>().cornerPortraitBoxInGame = cornerUIManager;
                        spawnedEnemy.GetComponent<AIBehaviour>()._data = _data;
                        spawnedEnemy.GetComponent<GameInformation>()._data = _data;
                        //
                        spawnedEnemy.GetComponent<PlayerInformation>().Respawn = true;
                        if (spawnedEnemy.transform.Find("VFX").Find("VFX9x9").GetComponent<Animator>().isActiveAndEnabled)
                        {
                            spawnedEnemy.transform.Find("VFX").Find("VFX9x9").GetComponent<Animator>().SetTrigger("enemySpawn");
                        }
                        if (spawnedEnemy.transform.Find("CharacterModel").GetComponent<Animator>().isActiveAndEnabled)
                        {
                            spawnedEnemy.transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerHit");
                        }
                    }
                    else if (CheckIfSpecificTag(EnemySpawnPointsCopy[index], 0, 0, blockingLayer, "Player"))
                    {
                        GetSpecificGroundTile(EnemySpawnPointsCopy[index], 0, 0, blockingLayer).GetComponent<PlayerInformation>().DealDamage(5, false, gameObject);
                    }
                    GetComponent<GameInformation>().FakeUpdate();
                    EnemySpawnPointsCopy.RemoveAt(index);
                }));
            }
            StartCoroutine(ExecuteAfterTime(0.5f + enemyCount, () =>
            {
                GetComponent<GameInformation>().EndAITurn(true);
            }));
            //Debug.Log("Respawned enemies");
            RespawnCount--;
        }
    }

    /*
    void Update()
    {
        if(Input.GetKeyDown("x"))
        {
            SpawnEnemiesAtSpawnPoints(3, 1);
        }
    }
    */

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
    protected void HealTeamsExceptOne(int teamIndex)
    {
        for (int i = 0; i < GetComponent<PlayerTeams>().allCharacterList.teams.Count; i++)
        {
            if (i != teamIndex)
            {
                for (int j = 0; j < GetComponent<PlayerTeams>().allCharacterList.teams[i].characters.Count; j++) //dabartine komanda
                {
                    GameObject characterInList = GetComponent<PlayerTeams>().allCharacterList.teams[i].characters[j];
                    if (characterInList.GetComponent<PlayerInformation>().health > 0)
                    {
                        characterInList.GetComponent<PlayerInformation>().Heal(10, false);
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