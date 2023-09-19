using UnityEngine;

public class IceQuake : BaseAction
{
    private const int RootDamage = 5;
    void Start()
    {
        AttackHighlight = new Color32(123,156, 178,255);
        AttackHighlightHover = new Color32(103, 136, 158, 255);
        CharacterOnGrid = new Color32(146, 212, 255, 255);
    }
    public override void ResolveAbility(Vector3 position)
    {
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            ChunkData targetChunk = GameTileMap.Tilemap.GetChunk(position);
            GameObject character = targetChunk.GetCurrentCharacter();
            PlayerInformation playerInformationLocal = character?.GetComponent<PlayerInformation>();
            int bonusDamage = 0;

            //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell3");

            if (playerInformationLocal != null)
            {
                if (playerInformationLocal.Slow1 || playerInformationLocal.Slow2 || playerInformationLocal.Slow3)
                {
                    //transform.Find("VFX").Find("VFXIceFreeze").GetComponent<Animator>().SetBool("iceFreeze", false);
                    //transform.Find("VFX").Find("VFXOilSlow").GetComponent<Animator>().SetBool("oilSlow", false);

                    playerInformationLocal.Slow1 = false;
                    playerInformationLocal.Slow2 = false;
                    playerInformationLocal.Slow3 = false;
                    playerInformationLocal.ApplyDebuff("CantMove");
                    bonusDamage += RootDamage;
                }
                else
                {
                    playerInformationLocal.ApplyDebuff("IceSlow");
                }
            }

            // character.transform.Find("mapTile").Find("VFX9x9Below").gameObject.GetComponent<Animator>()
            //     .SetTrigger("iceQuake");

            DealRandomDamageToTarget(targetChunk, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
            FinishAbility();
        }
    }
}
    

