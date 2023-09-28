using UnityEngine;

public class IsolatedStrike : BaseAction
{
    //private string actionStateName = "IsolatedStrike";
    //public int attackDamage = 60;
    //public int minAttackDamage = 4;
    //public int maxAttackDamage = 8;
    private int isolationDamage = 7;
    
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        int bonusDamage = 0;
        //Isolation
        if (IsTargetIsolated(chunk))
        {
            bonusDamage += isolationDamage;
        }

        //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
        DealRandomDamageToTarget(chunk, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
        FinishAbility();
    }

    public void OnTileHover(Vector3 position)
    {
        ChunkData chunk = GameTileMap.Tilemap.GetChunk(position);
        int showMinDamage = minAttackDamage;
        int shownMaxDamage = maxAttackDamage;
        if (IsTargetIsolated(chunk))
        {
            showMinDamage += isolationDamage;
            shownMaxDamage += isolationDamage;
        }

        //EnableDamagePreview(position, showMinDamage, shownMaxDamage);
    }
    private bool IsTargetIsolated(ChunkData target)
    {
        ChunkData[,] chunks = GameTileMap.Tilemap.GetChunksArray();
        (int y, int x) indexes = target.GetIndexes();
        int x = indexes.x;
        int y = indexes.y;

        int[] dx = { 0, 0, 1, -1 };
        int[] dy = { 1, -1, 0, 0 };

        for (int i = 0; i < 4; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];

            if (GameTileMap.Tilemap.CheckBounds(ny, nx) && chunks[ny, nx]?.GetCurrentCharacter() != null && IsAllegianceSame(chunks[ny, nx]))
            {
                return false;
            }
            
        }
        return true;
    }
}
