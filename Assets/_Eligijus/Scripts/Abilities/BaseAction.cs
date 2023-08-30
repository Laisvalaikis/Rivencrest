using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


[RequireComponent(typeof(AssignSound))]
    public abstract class BaseAction : CharacterAction
    {

        // audio effect indexes
        [Header("Sound Effect")]
        public int selectedEffectIndex;
        public int selectedSongIndex;

        [Header("Base Action")] 
        [SerializeField] protected PlayerInformation playerInformation;
        public string actionStateName;
        protected LayerMask groundLayer;
        protected LayerMask blockingLayer;
        protected LayerMask consumablesLayer;
        protected LayerMask fogLayer;
        protected LayerMask portalLayer;
        protected LayerMask whiteFieldLayer;
        protected GameInformation gameInformation;
        protected bool laserGrid = false;
        //private RaycastHit2D raycast;
        public int AttackRange = 1;
        public int AbilityCooldown = 1;
        public int minAttackDamage = 0;
        public int maxAttackDamage = 0;
        public bool isAbilitySlow = true;
        public bool friendlyFire = false;
        [HideInInspector] public bool AttackAbility = false;
        [HideInInspector] public int AvailableAttacks = 1;//Kiek zaidejas gali naudoti ability per ejima
        [HideInInspector] public int AbilityPoints; //dabartiniai pointsai, jie turi buti lygus arba didesni uz AbilityCooldown, kad galetum leist ability
        [HideInInspector] public bool isDisabled = false;
        [HideInInspector] public GameObject spawnedCharacter;
        protected List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();
        protected List<GameObject> MergedTileList = new List<GameObject>();
        private AssignSound _assignSound;
        private PlayerInformationData _playerInformationData;
        private List<ChunkData> _chunkList;
        
        void Awake()
        {
            playerInformation = GetComponent<PlayerInformation>();
            _playerInformationData = new PlayerInformationData();
            _playerInformationData.CopyData(playerInformation.playerInformationData);
            _chunkList = new List<ChunkData>();
            AbilityPoints = AbilityCooldown;
            groundLayer = LayerMask.GetMask("Ground");
            blockingLayer = LayerMask.GetMask("BlockingLayer");
            consumablesLayer = LayerMask.GetMask("Consumables");
            fogLayer = LayerMask.GetMask("Fog");
            portalLayer = LayerMask.GetMask("Portal");
            whiteFieldLayer = LayerMask.GetMask("WhiteField");
            _assignSound = GetComponent<AssignSound>();
        }
        
        
        public override void CreateGrid()
        {
            //CreateAvailableTileList();
            //MergeIntoOneList();
            CreateAvailableChunkList();
        }
        public override void ClearGrid()
        {
            foreach (var chunk in _chunkList)
            {
                chunk.DisableTileRendering();
                chunk.DisableTileRenderingGameObject();
                chunk.GetTileHighlight().ActivateMovementTile(false);
            }
            _chunkList.Clear();
        }

        //Creates a list of available chunks for attack
        private void CreateAvailableChunkList()
        {
            ChunkData startChunk = GameTileMap.Tilemap.GetChunk(transform.position);
            if(laserGrid)
            {
                GeneratePlusPattern(startChunk, AttackRange);
            }
            else
            {
                GenerateDiamondPattern(startChunk, AttackRange);
            }
        }

        public List<ChunkData> ReturnGeneratedChunks()
        {
            return _chunkList;
        }

        protected override List<ChunkData> GeneratePattern(ChunkData centerChunk, ChunkData[,] chunksArray, int length)
        {
            return base.GeneratePattern(centerChunk, chunksArray, length);
        }

        private void GenerateDiamondPattern(ChunkData centerChunk, int radius)
        {
            (int centerX, int centerY) = centerChunk.GetIndexes();
            ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray(); 
            GameTileMap.Tilemap.EnableAllTiles();
            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    if (Mathf.Abs(x) + Mathf.Abs(y) <= radius)
                    {
                        int targetX = centerX + x;
                        int targetY = centerY + y;

                        // Ensuring we don't go out of array bounds.
                        if (targetX >= 0 && targetX < chunksArray.GetLength(0) && targetY >= 0 && targetY < chunksArray.GetLength(1))
                        {
                            ChunkData chunk = chunksArray[targetX, targetY];
                            if (chunk != null && !chunk.TileIsLocked())
                            {
                                _chunkList.Add(chunk);
                                chunk.EnableTileRenderingGameObject();
                                chunk.EnableTileRendering();
                                chunk.GetTileHighlight().ActivateMovementTile(true);
                            }
                        }
                    }
                }
            }
        }
        
        private void GeneratePlusPattern(ChunkData centerChunk, int length)
        {
            (int centerX, int centerY) = centerChunk.GetIndexes();
            ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray();
            GameTileMap.Tilemap.EnableAllTiles();

            for (int i = 1; i <= length; i++)
            {
                List<(int, int)> positions = new List<(int, int)> 
                {
                    (centerX, centerY + i),  // Up
                    (centerX, centerY - i),  // Down
                    (centerX + i, centerY),  // Right
                    (centerX - i, centerY)   // Left
                };

                foreach (var (x, y) in positions)
                {
                    if (x >= 0 && x < chunksArray.GetLength(0) && y >= 0 && y < chunksArray.GetLength(1))
                    {
                        ChunkData chunk = chunksArray[x, y];
                        if (chunk != null && !chunk.TileIsLocked())
                        {
                            _chunkList.Add(chunk);
                            chunk.EnableTileRenderingGameObject();
                            chunk.EnableTileRendering();
                            chunk.GetTileHighlight().ActivateMovementTile(true);
                        }
                    }
                }
            }
        }
        public virtual void OnTileHover(GameObject tile)//parodo kas bus
        {
            
        }
        public virtual void EnableDamagePreview(GameObject tile, int minAttackDamage, int maxAttackDamage = -1)//damage texto ijungimui
        {
            if(!tile.GetComponent<HighlightTile>().FogOfWarIsEnabled() && (CanPreviewBeShown(tile.transform.position) || CanTileBeClicked(tile.transform.position)))
            {
                if ((CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") || CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Wall"))
                    && GetSpecificGroundTile(tile, 0, 0, blockingLayer).GetComponent<PlayerInformation>().health - minAttackDamage <= 0 && CanPreviewBeShown(tile.transform.position))
                {
                    tile.transform.Find("mapTile").Find("Death").gameObject.SetActive(true);
                    tile.transform.Find("mapTile").Find("DamageText").position = tile.transform.position + new Vector3(0f, 0.65f, 0f);
                }
                tile.GetComponent<HighlightTile>().SetHighlightColor(GameObject.Find("GameInformation").GetComponent<ColorManager>().MovementHighlightHover);
                //ziurim ar random damage ar ne
                if((CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") || CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Wall")) && CanPreviewBeShown(tile.transform.position))
                {
                    tile.transform.Find("mapTile").Find("DamageText").gameObject.SetActive(true);
                    if (maxAttackDamage == -1)
                    {
                        tile.transform.Find("mapTile").Find("DamageText").gameObject.GetComponent<TextMeshPro>().text = "-" + minAttackDamage.ToString();
                    }
                    else
                    {
                        tile.transform.Find("mapTile").Find("DamageText").gameObject.GetComponent<TextMeshPro>().text = minAttackDamage.ToString() + "-" + maxAttackDamage.ToString();
                    }
                }
            }
        }
       
        public virtual void EnableDamagePreview(GameObject tile, List<GameObject> tileList, int minAttackDamage, int maxAttackDamage = -1)//damage texto ijungimui
        {
            foreach(GameObject tileInList in tileList)
            {
                EnableDamagePreview(tileInList, minAttackDamage, maxAttackDamage);
            }
        }

        protected virtual void EnableTextPreview(GameObject tile, string text)
        {
            if(!tile.GetComponent<HighlightTile>().FogOfWarIsEnabled() && (CanTileBeClicked(tile.transform.position) || CanPreviewBeShown(tile.transform.position)))
            {
                tile.transform.Find("mapTile").Find("DamageText").gameObject.SetActive(true);
                tile.transform.Find("mapTile").Find("DamageText").gameObject.GetComponent<TextMeshPro>().text = text;
                tile.GetComponent<HighlightTile>().SetHighlightColor(GameObject.Find("GameInformation").GetComponent<ColorManager>().MovementHighlightHover);//tile.GetComponent<HighlightTile>().HoverHighlightColor;
            }
        }

        protected virtual void EnableTextPreview(GameObject tile, List<GameObject> tileList, string text)
        {
            foreach(GameObject tileInList in tileList)
            {
                EnableTextPreview(tileInList, text);
            }
        }
        public virtual void OffTileHover(GameObject tile)//isjungia ka ijunge onTileHover
        {
            DisablePreview(tile);
        }

        protected void DisablePreview(GameObject tile)//damage texto isjungimui
        {
            tile.transform.Find("mapTile").Find("Death").gameObject.SetActive(false);
            tile.transform.Find("mapTile").Find("DamageText").position = tile.transform.position + new Vector3(0f, 0.50f, 0f);
            tile.transform.Find("mapTile").Find("DamageText").gameObject.SetActive(false);
            tile.transform.Find("mapTile").Find("Character").gameObject.SetActive(false);
            tile.transform.Find("mapTile").Find("CharacterAlpha").gameObject.SetActive(false);
            tile.transform.Find("mapTile").Find("Object").gameObject.SetActive(false);
            tile.transform.Find("mapTile").Find("Highlight").gameObject.SetActive(false);
            tile.GetComponent<HighlightTile>().SetHighlightColor(tile.GetComponent<HighlightTile>().NotHoveredColor);
        }

        protected void DisablePreview(GameObject tile, List<GameObject> tileList)
        {
            foreach (var tileInList in tileList)
            {
                DisablePreview(tileInList);
            }
        }
        public virtual bool CanTileBeClicked(Vector3 position)//ar veiks ability
        {
            if (CheckIfSpecificTag(position, 0, 0, blockingLayer, "Player") && !isAllegianceSame(position))
            {
                return true;
            }

            return false;
        }

        protected bool CanPreviewBeShown(Vector3 position)//ar rodys preview
        {
            return CanTileBeClicked(position) && (!(CheckIfSpecificLayer(position, 0, 0, blockingLayer) && isAllegianceSame(position)) || friendlyFire);
        }
        public virtual bool CanGridBeEnabled()
        {
            if (!isDisabled && AbilityPoints >= AbilityCooldown && (!AttackAbility || (!GetComponent<PlayerInformation>().CantAttackCondition && AvailableAttacks != 0)))
            {
                return true;
            }
            return false;
        }
        public virtual void EnableGrid()
        {
            if (CanGridBeEnabled())
            {
                CreateGrid();
                HighlightAll();
            }

        }

        protected virtual void HighlightAll()
        {
            foreach (GameObject tile in MergedTileList)
            {
                tile.GetComponent<HighlightTile>().SetHighlightBool(true);
                tile.GetComponent<HighlightTile>().activeState = actionStateName;
                tile.GetComponent<HighlightTile>().ChangeBaseColor();
                tile.GetComponent<HighlightTile>().canAbilityTargetAllies = true;
                tile.GetComponent<HighlightTile>().canAbilityTargetYourself = true;
            }
            GetSpecificGroundTile(transform.gameObject, 0, 0, groundLayer).GetComponent<HighlightTile>().SetHighlightBool(false);
        }
        
private bool IsTileAccessible(GameObject middleTile, int xOffset, int yOffset, bool canWallsBeTargeted)
{
    bool isGround = CheckIfSpecificLayer(middleTile, xOffset, yOffset, groundLayer);
    bool isBlockingLayer = CheckIfSpecificLayer(middleTile, xOffset, yOffset, blockingLayer);
    bool isPlayer = CheckIfSpecificTag(middleTile, xOffset, yOffset, blockingLayer, "Player");
    bool isWall = CheckIfSpecificTag(middleTile, xOffset, yOffset, blockingLayer, "Wall");
    bool isMiddleTileWall = CheckIfSpecificTag(middleTile, 0, 0, blockingLayer, "Wall");

    return isGround && (!isBlockingLayer || isPlayer || (canWallsBeTargeted && isWall)) && (!canWallsBeTargeted || !isMiddleTileWall);
}


        public virtual void DisableGrid()
        {
            foreach (List<GameObject> movementTileList in this.AvailableTiles)
            {
                foreach (GameObject tile in movementTileList)
                {
                    tile.GetComponent<HighlightTile>().SetHighlightBool(false);
                }
            }
        }
        public virtual void OnTurnStart()
        {
        }
        public virtual void OnTurnStartSecondAction() //Veiksmai, kurie vyksta po OnTurnStart
        {
        }
        public virtual void OnTurnEnd()
        {
            RefillActionPoints();
            
        }
        public virtual void RemoveActionPoints()//panaudojus action
        {
            AvailableAttacks--;
        }
        public virtual void RefillActionPoints()//pradzioj ejimo
        {
            AvailableAttacks = 1;
            AbilityPoints++;
        }
        
        public override void ResolveAbility(Vector3 position)
        {
            base.ResolveAbility(position);
            _assignSound.PlaySound(selectedEffectIndex, selectedSongIndex);
            Debug.LogWarning("PlaySound");
            ClearGrid();
        }
        public virtual void ResolveAbility(GameObject gameobject)
        {
            Debug.Log("Fake aah method");
            //Dont remove until all abilities are updated
        }

        protected virtual void FinishAbility()
        {
            AbilityPoints = 0;//Cooldown counter
            // if (isAbilitySlow)
            // {
            //     GetComponent<ActionManager>().RemoveAllActionPoints();
            // }
            // if (AttackAbility)
            // {
            //     GetComponent<ActionManager>().RemoveAttackActionPoints();
            // }
            GetComponent<PlayerInformation>().currentState = "Movement";
            if (isAbilitySlow)
            {
                DisableGrid();
            }
        }
        
        public GameObject GetSpecificGroundTile(GameObject tile, int x, int y, LayerMask chosenLayer)
        {
            Vector3 firstPosition = tile.transform.position + new Vector3(0f, 0.5f, 0f) + new Vector3(x, y, 0f);
            Vector3 secondPosition = firstPosition + new Vector3(0.1f, 0f, 0f);
            RaycastHit2D raycast = Physics2D.Linecast(firstPosition, secondPosition, chosenLayer);
            return raycast.transform.gameObject;
        }
        
        // public static 

        public ChunkData GetSpecificGroundTile(Vector3 position)
        {
            return GameTileMap.Tilemap.GetChunk(position);
        }
        
        public static bool CheckIfSpecificLayer(Vector3 position, int x, int y, LayerMask chosenLayer)
        {
            Vector3 firstPosition = position + new Vector3(0f, 0.5f, 0f) + new Vector3(x, y, 0f);
            Vector3 secondPosition = firstPosition + new Vector3(0.1f, 0f, 0f);
            RaycastHit2D raycast = Physics2D.Linecast(firstPosition, secondPosition, chosenLayer);
            if (raycast.transform == null)
            {
                return false;
            }
            return true;
        }
        public static bool CheckIfSpecificLayer(GameObject tile, int x, int y, LayerMask chosenLayer)
        {
            Vector3 firstPosition = tile.transform.position + new Vector3(0f, 0.5f, 0f) + new Vector3(x, y, 0f);
            Vector3 secondPosition = firstPosition + new Vector3(0.1f, 0f, 0f);
            RaycastHit2D raycast = Physics2D.Linecast(firstPosition, secondPosition, chosenLayer);
            if (raycast.transform == null)
            {
                return false;
            }
            return true;
        }

        public static bool CheckIfSpecificTag(Vector3 position, int x, int y, LayerMask chosenLayer, string tagName)
        {
            Vector3 firstPosition = position + new Vector3(0f, 0.5f, 0f) + new Vector3(x, y, 0f);
            Vector3 secondPosition = firstPosition + new Vector3(0.1f, 0f, 0f);
            RaycastHit2D raycast = Physics2D.Linecast(firstPosition, secondPosition, chosenLayer);
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
        public static bool CheckIfSpecificTag(GameObject tile, int x, int y, LayerMask chosenLayer, string tagName)
        {
            Vector3 firstPosition = tile.transform.position + new Vector3(0f, 0.5f, 0f) + new Vector3(x, y, 0f);
            Vector3 secondPosition = firstPosition + new Vector3(0.1f, 0f, 0f);
            RaycastHit2D raycast = Physics2D.Linecast(firstPosition, secondPosition, chosenLayer);
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

        protected bool isAllegianceSame(Vector3 position)
        {
            ChunkData chunkData = GameTileMap.Tilemap.GetChunk(position);
            if (chunkData != null && chunkData.GetCurrentPlayerInformation().GetPlayerTeam() != playerInformation.GetPlayerTeam() || friendlyFire)
            {
                return true;
            }
            else
            {
                return false;
            }

            // var playerTeams = gameInformation.GetComponent<PlayerTeams>();
            // return playerTeams.FindTeamAllegiance(GameTileMap.Tilemap.GetChunk(position).GetCurrentCharacter().GetComponent<PlayerInformation>().CharactersTeam)
            //     == playerTeams.FindTeamAllegiance(GetComponent<PlayerInformation>().CharactersTeam);
        }
        
        
        protected bool isAllegianceSame(GameObject position)
        {
            Debug.Log("Fix allegiances");
            var playerTeams = gameInformation.GetComponent<PlayerTeams>();
            return playerTeams.FindTeamAllegiance(GetSpecificGroundTile(position, 0, 0, blockingLayer).GetComponent<PlayerInformation>().CharactersTeam)
                == playerTeams.FindTeamAllegiance(GetComponent<PlayerInformation>().CharactersTeam);
        }
        protected bool isAllegianceSame(GameObject tile1, GameObject tile2, LayerMask chosenLayer)
        {
            var playerTeams = gameInformation.GetComponent<PlayerTeams>();
            return playerTeams.FindTeamAllegiance(GetSpecificGroundTile(tile1, 0, 0, chosenLayer).GetComponent<PlayerInformation>().CharactersTeam)
                == playerTeams.FindTeamAllegiance(GetSpecificGroundTile(tile2, 0, 0, chosenLayer).GetComponent<PlayerInformation>().CharactersTeam);
        }
        
        protected bool IsItCriticalStrike(ref int damage)
        {
            int critNumber = Random.Range(0, 100);
            bool crit;
            if (critNumber > _playerInformationData.critChance)
            {
                crit = false;
            }
            else
            {
                damage += 3;
                crit = true;
            }
            return crit;
        }

        protected void dodgeActivation(ref int damage, PlayerInformation target) //Dodge temporarily removed
        {
            int dodgeNumber = Random.Range(0, 100);
            if (dodgeNumber > _playerInformationData.accuracy - target.playerInformationData.dodgeChance)
            {
                damage = -1;
                Debug.Log("Dodge");
            }
        }

        protected void DealRandomDamageToTarget(Vector3 targetChunk, int minAttackDamage, int maxAttackDamage)
        {
            ChunkData chunkData = GameTileMap.Tilemap.GetChunk(targetChunk);
            if (chunkData != null && chunkData.GetCurrentCharacter() != null && isAllegianceSame(targetChunk))
            {
                int randomDamage = Random.Range(minAttackDamage, maxAttackDamage);
                bool crit = IsItCriticalStrike(ref randomDamage);
                dodgeActivation(ref randomDamage, chunkData.GetCurrentPlayerInformation());
                chunkData.GetCurrentPlayerInformation().DealDamage(randomDamage, crit, gameObject);
                
            }
        }
        public virtual void PrepareForAIAction()
        {
            if (CanGridBeEnabled())
            {
                CreateGrid();
            }
        }
        public virtual GameObject PossibleAIActionTile()
        {
            return null;
        }
        public virtual void SpecificAbilityAction(GameObject character = null)
        {
        }
       
        protected bool DoesCharacterHaveBlessing(string blessingName)
        {
            return _playerInformationData.BlessingsAndCurses.Find(x => x.blessingName == blessingName) != null;
        }
        public virtual void BuffAbility()
        {
        }
        public virtual BaseAction GetBuffedAbility(List<Blessing> blessings)
        {
            return this;
        }
        public virtual string GetDamageString()
        {
            return $"{minAttackDamage}-{maxAttackDamage}";
        }
        protected IEnumerator ExecuteAfterTime(float time, Action task)
        {
            yield return new WaitForSeconds(time);
            task();
        }
        protected IEnumerator ExecuteAfterFrames(int frameCount, Action task)
        {
            for(int i = 0; i < frameCount; i++)
            {
                yield return null;
            }
            task();
        }
    }

