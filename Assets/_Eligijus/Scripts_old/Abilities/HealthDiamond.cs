using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDiamond : Consumable
{
    public OrbType type;
    public int HealthAmount = 50;
    public override void PickUp(GameObject WhoStepped)
    {
        if (WhoStepped.gameObject.tag == "Player")
        {
            switch (type)
            {
                case OrbType.Health:
                    WhoStepped.GetComponent<PlayerInformation>().MaxHealth += HealthAmount;
                    WhoStepped.GetComponent<PlayerInformation>().Heal(HealthAmount, false);
                    //WhoStepped.GetComponent<PlayerInformation>().health += HealthAmount;
                    WhoStepped.transform.Find("VFX").Find("VFXImpact").GetComponent<Animator>().SetTrigger("red1");
                    break;
                case OrbType.Healing:
                    WhoStepped.GetComponent<PlayerInformation>().Heal(HealthAmount, false);
                    //WhoStepped.transform.Find("VFX").Find("VFXImpact").GetComponent<Animator>().SetTrigger("red1");
                    break;
            }
            Destroy(transform.parent.gameObject);

        }
    }
}
public enum OrbType
{
    Health,
    Healing
}
