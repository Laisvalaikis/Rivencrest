using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AssignSound))]
    public abstract class BaseAction : MonoBehaviour
    {
        // audio effect indexes
        [Header("Sound Effect")]
        public int selectedEffectIndex;
        public int selectedSongIndex;

        //Turn managing, ability lifetime
        public int turnsSinceCast = 0;
        public int turnLifetime = 1;
        
        [Header("Base Action")] 
        [SerializeField] protected PlayerInformation playerInformation;
        [SerializeField] protected bool laserGrid = false;
        [HideInInspector] public GameObject spawnedCharacter;
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
        public float efficiencyCoefficient = 0.5f;
        private AssignSound _assignSound;
        private PlayerInformationData _playerInformationData;
        protected List<ChunkData> _chunkList;

        [Header("Highlight colors")] 
        protected Color32 AttackHighlight = new Color32(0xB2,0x5E,0x55,0xff);
        protected Color32 AttackHighlightHover = new Color32(0x9E, 0x4A, 0x41, 0xff);
        protected Color32 AttackHoverCharacter = new Color32(255, 227, 0, 255);
        protected Color32 OtherOnGrid = new Color32(113, 113, 113, 255);
        protected Color32 CharacterOnGrid = new Color32(0xFF, 0x59, 0x47, 0xff);

        void Awake()
        {
            playerInformation = GetComponent<PlayerInformation>();
            _playerInformationData = new PlayerInformationData();
            _playerInformationData.CopyData(playerInformation.playerInformationData);
            _chunkList = new List<ChunkData>();
            AbilityPoints = AbilityCooldown;
            _assignSound = GetComponent<AssignSound>();
        }
        protected virtual void HighlightGridTile(ChunkData chunkData)
        {
            if (chunkData.GetCurrentCharacter() != GameTileMap.Tilemap.GetCurrentCharacter())
            {
                SetNonHoveredAttackColor(chunkData);
                chunkData.GetTileHighlight().ActivateColorGridTile(true);
            }
        }
        protected void HighlightAllGridTiles()
        {
            foreach (var chunk in _chunkList)
            {
                HighlightGridTile(chunk);
            }
        }
        public bool IsPositionInGrid(Vector3 position)
        {
            return _chunkList.Contains(GameTileMap.Tilemap.GetChunk(position));
        }
        public virtual void CreateGrid()
        {
            CreateAvailableChunkList(AttackRange);
            HighlightAllGridTiles();
        }
        public void ClearGrid()
        {
            foreach (var chunk in _chunkList)
            {
                HighlightTile highlightTile = chunk.GetTileHighlight();
                highlightTile.ActivateColorGridTile(false);
                DisableDamagePreview(chunk);
            }
            _chunkList.Clear();
        }        
        //Creates a list of available chunks for attack
        public virtual void CreateAvailableChunkList(int attackRange)
        {
            ChunkData startChunk = GameTileMap.Tilemap.GetChunk(transform.position);
            if(laserGrid)
            {
                GeneratePlusPattern(startChunk, attackRange);
            }
            else
            {
                GenerateDiamondPattern(startChunk, attackRange);
            }
        }

        public List<ChunkData> GetChunkList()
        {
            return _chunkList;
        }
        protected void GenerateDiamondPattern(ChunkData centerChunk, int radius)
        {
            (int centerX, int centerY) = centerChunk.GetIndexes();
            ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray(); 
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
                            }
                        }
                    }
                }
            }
        }        
        protected virtual void GeneratePlusPattern(ChunkData centerChunk, int length)
        {
            (int centerX, int centerY) = centerChunk.GetIndexes();
            ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray();

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
                        }
                    }
                }
            }
        }        
        protected virtual void SetNonHoveredAttackColor(ChunkData chunkData)
        {
            if (chunkData.GetCurrentCharacter() == null || (chunkData.GetCurrentCharacter() != null && !CanTileBeClicked(chunkData)))
            {
                chunkData.GetTileHighlight().SetHighlightColor(AttackHighlight);
            }
            else
            {
                chunkData.GetTileHighlight().SetHighlightColor(CharacterOnGrid);
            }
        }
        protected virtual void SetHoveredAttackColor(ChunkData chunkData)
        {
            if (chunkData.GetCurrentCharacter() == null || (chunkData.GetCurrentCharacter() != null && !CanTileBeClicked(chunkData)))
            {
                chunkData.GetTileHighlight().SetHighlightColor(AttackHighlightHover);
            }
            else
            {
                chunkData.GetTileHighlight().SetHighlightColor(AttackHoverCharacter);
                EnableDamagePreview(chunkData);
            }
        }
        public virtual void OnMoveArrows(ChunkData hoveredChunk, ChunkData previousChunk)
        {
            
        }
        public virtual void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
        {
            HighlightTile previousChunkHighlight = previousChunk?.GetTileHighlight();
            HighlightTile hoveredChunkHighlight = hoveredChunk?.GetTileHighlight();

            if (previousChunkHighlight != null && (hoveredChunk == null || !hoveredChunkHighlight.isHighlighted))
            {
                SetNonHoveredAttackColor(previousChunk);
                DisableDamagePreview(previousChunk);
            }
            if (hoveredChunkHighlight == null || hoveredChunk == previousChunk)
            {
                return;
            }
            if (hoveredChunkHighlight.isHighlighted)
            {
                SetHoveredAttackColor(hoveredChunk);
            }
            if (previousChunkHighlight != null)
            {
                SetNonHoveredAttackColor(previousChunk);
                DisableDamagePreview(previousChunk);
            }
        }
        protected Side ChunkSideByCharacter(ChunkData playerChunk, ChunkData chunkDataTarget)
        {
        
            (int x, int y) playerChunkIndex = playerChunk.GetIndexes();
            (int x, int y) chunkIndex = chunkDataTarget.GetIndexes();
            if (playerChunkIndex.x > chunkIndex.x)
            {
                return Side.isFront;
            }
            else if (playerChunkIndex.x < chunkIndex.x)
            {
                return Side.isBack;
            }
            else if (playerChunkIndex.y < chunkIndex.y)
            {
                return Side.isRight;
            }
            else if (playerChunkIndex.y > chunkIndex.y)
            {
                return Side.isLeft;
            }
            return Side.none;
        }
        protected int2 GetSideVector(Side side)
        {
            int2 sideVector = int2.zero;
            switch (side)
            {
                case Side.isFront:
                    sideVector = new int2(-1, 0);
                    break;
                case Side.isBack:
                    sideVector = new int2(1, 0);
                    break;
                case Side.isRight:
                    sideVector = new int2(0, 1);
                    break;
                case Side.isLeft:
                    sideVector = new int2(0, -1);
                    break;
                case Side.none:
                    sideVector = int2.zero;
                    break;
            }
            return sideVector;
        }
        protected void MovePlayerToSide(ChunkData player, int2 sideVector, ChunkData positionTile = null)
        {
            (int x, int y) indexes = player.GetIndexes();
            if (positionTile != null)
            {
                indexes = positionTile.GetIndexes();
            }
            int2 tempIndexes = new int2(indexes.x + sideVector.x, indexes.y + sideVector.y);
            if (GameTileMap.Tilemap.CheckBounds(tempIndexes.x, tempIndexes.y))
            {
                ChunkData tempTile = GameTileMap.Tilemap.GetChunkDataByIndex(tempIndexes.x, tempIndexes.y);
                if (!tempTile.CharacterIsOnTile())
                {
                    GameTileMap.Tilemap.MoveSelectedCharacter(tempTile.GetPosition(), new Vector3(0, 0.5f, 1), player.GetCurrentCharacter());
                }
            }
        }
        protected virtual void EnableDamagePreview(ChunkData chunk, string customText=null)
        {
            HighlightTile highlightTile = chunk.GetTileHighlight();
            if (customText != null)
            {
                highlightTile.SetDamageText(customText);
            }
            else
            {
                if (maxAttackDamage == minAttackDamage)
                {
                    highlightTile.SetDamageText(maxAttackDamage.ToString());
                }
                else
                {
                    highlightTile.SetDamageText($"{minAttackDamage}-{maxAttackDamage}");
                }

                if (chunk.GetCurrentPlayerInformation().GetHealth() <= minAttackDamage)
                {
                    highlightTile.ActivateDeathSkull(true);
                }
            }
        }
        protected virtual void EnableDamagePreview(List<ChunkData> chunks, string customText=null)
        {
            foreach(ChunkData chunk in chunks)
            {
                EnableDamagePreview(chunk, customText);
            }
        }
        
        protected virtual void DisableDamagePreview(ChunkData chunk)
        {
            HighlightTile highlightTile = chunk.GetTileHighlight();
            highlightTile.ActivateDeathSkull(false);
            highlightTile.DisableDamageText();
        }
        protected virtual void DisableDamagePreview(List<ChunkData> chunks)
        {
            foreach(ChunkData chunk in chunks)
            {
                DisableDamagePreview(chunk);
            }
        }
        public virtual bool CanTileBeClicked(Vector3 position)
        {
            ChunkData chunk = GetSpecificGroundTile(position);
            return CheckIfSpecificInformationType(chunk, InformationType.Player) && !IsAllegianceSame(chunk);
        }
        
        public virtual bool CanTileBeClicked(ChunkData chunk)
        {
            return CheckIfSpecificInformationType(chunk, InformationType.Player) && !IsAllegianceSame(chunk);
        }
        public virtual void OnTurnStart()
        {
            
        }
        public virtual void OnTurnEnd()
        {
            RefillActionPoints();
            turnsSinceCast++;
        }
        public virtual void RemoveActionPoints()//panaudojus action
        {
            AvailableAttacks--;
        }

        protected virtual void RefillActionPoints()//pradzioj ejimo
        {
            AvailableAttacks = 1;
            AbilityPoints++;
        }
        public virtual void ResolveAbility(Vector3 position)
        {
            _assignSound.PlaySound(selectedEffectIndex, selectedSongIndex);
            Debug.LogWarning("PlaySound");
            ClearGrid();
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
            if (isAbilitySlow)
            {
                //DisableGrid();
            }
            GameTileMap.Tilemap.DeselectCurrentCharacter();
        }
        protected ChunkData GetSpecificGroundTile(Vector3 position)
        {
            return GameTileMap.Tilemap.GetChunk(position);
        }
        protected static bool CheckIfSpecificInformationType(ChunkData chunk, InformationType informationType)
        {
            return chunk.GetInformationType()==informationType;
        }
        protected bool IsAllegianceSame(ChunkData chunk)
        {
            return chunk == null || chunk.GetCurrentPlayerInformation().GetPlayerTeam() == playerInformation.GetPlayerTeam() || !friendlyFire;
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
        private void DodgeActivation(ref int damage, PlayerInformation target) //Dodge temporarily removed
        {
            int dodgeNumber = Random.Range(0, 100);
            if (dodgeNumber > _playerInformationData.accuracy - target.playerInformationData.dodgeChance)
            {
                damage = -1;
            }
        }
        protected void DealRandomDamageToTarget(ChunkData chunkData, int minDamage, int maxDamage)
        {
            if (chunkData != null && chunkData.GetCurrentCharacter() != null && IsAllegianceSame(chunkData))
            {
                int randomDamage = Random.Range(minDamage, maxDamage);
                bool crit = IsItCriticalStrike(ref randomDamage);
                DodgeActivation(ref randomDamage, chunkData.GetCurrentPlayerInformation());
                chunkData.GetCurrentPlayerInformation().DealDamage(randomDamage, crit, gameObject);
            }
        }
        protected void DealDamage(ChunkData chunkData, int damage, bool crit)
        {
            if (chunkData != null && chunkData.GetCurrentCharacter() != null && IsAllegianceSame(chunkData))
            {
                chunkData.GetCurrentPlayerInformation().DealDamage(damage, crit, gameObject);
            }
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
    }

