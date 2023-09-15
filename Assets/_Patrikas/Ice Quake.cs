using UnityEngine;

public class IceQuake : BaseAction
{
    public int rootDamage = 5;
    void Start()
    {
        actionStateName = "IceQuake";
    }
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);

        ChunkData targetChunk = GameTileMap.Tilemap.GetChunk(position);
        GameObject character = targetChunk.GetCurrentCharacter();
        PlayerInformation playerInformation = character?.GetComponent<PlayerInformation>();

        int bonusDamage = 0;

        //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell3");

        if (playerInformation != null)
        {
            if (playerInformation.Slow1 || playerInformation.Slow2 || playerInformation.Slow3)
            {
                //transform.Find("VFX").Find("VFXIceFreeze").GetComponent<Animator>().SetBool("iceFreeze", false);
                //transform.Find("VFX").Find("VFXOilSlow").GetComponent<Animator>().SetBool("oilSlow", false);

                playerInformation.Slow1 = false;
                playerInformation.Slow2 = false;
                playerInformation.Slow3 = false;
                playerInformation.ApplyDebuff("CantMove");
                bonusDamage += rootDamage;
            }
            else
            {
                playerInformation.ApplyDebuff("IceSlow");
            }
        }

        // character.transform.Find("mapTile").Find("VFX9x9Below").gameObject.GetComponent<Animator>()
        //     .SetTrigger("iceQuake");

        DealRandomDamageToTarget(targetChunk, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
        FinishAbility();
    }

}
    

