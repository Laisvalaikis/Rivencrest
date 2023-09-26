using UnityEngine;

public class LongShot : BaseAction
{
    //public int minAttackDamage = 5;
    //public int maxAttackDamage = 7;

    private void Start()
    {
        isAbilitySlow = false;
        AttackHighlight = new Color32(123,156, 178,255);
        AttackHighlightHover = new Color32(103, 136, 158, 255);
        CharacterOnGrid = new Color32(146, 212, 255, 255);
    }

    public override void ResolveAbility(Vector3 position)
    {
            base.ResolveAbility(position);
            //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell2");
            ChunkData target = GameTileMap.Tilemap.GetChunk(position);
            DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
            //if (DoesCharacterHaveBlessing("Poisonous shot"))
            //{
           //     target.GetComponent<PlayerInformation>().Poisons.Add(new PlayerInformation.Poison(gameObject, 2, 1));
           // }
           // if (DoesCharacterHaveBlessing("Caught one"))
           // {
           //     target.GetComponent<PlayerInformation>().ApplyDebuff("IceSlow");
           //     target.GetComponent<PlayerInformation>().ApplyDebuff("IceSlow");
           // }
           // clickedTile.transform.Find("mapTile").Find("VFX9x9Upper").gameObject.GetComponent<Animator>().SetTrigger("rainOfArrows");
           // clickedTile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("green1");

            FinishAbility();
        
    }

    public override void CreateAvailableChunkList(int attackRange)
    {
        ChunkData centerChunk = GameTileMap.Tilemap.GetChunk(transform.position);
        (int centerX, int centerY) = centerChunk.GetIndexes();
        _chunkList.Clear();
        ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray(); 
        for (int y = -attackRange; y <= attackRange; y++)
        {
            for (int x = -attackRange; x <= attackRange; x++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) == attackRange)
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
}
