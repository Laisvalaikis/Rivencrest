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
        PlayerInformation information = character?.GetComponent<PlayerInformation>();

        int bonusDamage = 0;

        //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell3");

        if (information != null)
        {
            if (information.Slow1 || information.Slow2 || information.Slow3)
            {
                //transform.Find("VFX").Find("VFXIceFreeze").GetComponent<Animator>().SetBool("iceFreeze", false);
                //transform.Find("VFX").Find("VFXOilSlow").GetComponent<Animator>().SetBool("oilSlow", false);

                information.Slow1 = false;
                information.Slow2 = false;
                information.Slow3 = false;
                information.ApplyDebuff("CantMove");
                bonusDamage += rootDamage;
            }
            else
            {
                information.ApplyDebuff("IceSlow");
            }
        }

        // character.transform.Find("mapTile").Find("VFX9x9Below").gameObject.GetComponent<Animator>()
        //     .SetTrigger("iceQuake");

        DealRandomDamageToTarget(targetChunk, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
        FinishAbility();
    }

}
    

