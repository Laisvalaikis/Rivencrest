using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : Consumable
{
    public int trapDamage = 6;
    [HideInInspector] public GameObject creator;
    public override void PickUp(GameObject WhoStepped)
    {
        if (WhoStepped.gameObject.tag == "Player" && !isAllegianceSame(WhoStepped, creator))
        {
            WhoStepped.GetComponent<PlayerInformation>().DealDamage(trapDamage, false, creator);
            Destroy(transform.parent.gameObject);
        }
    }
    private bool isAllegianceSame(GameObject tile1, GameObject tile2)
    {
        return GameObject.Find("GameInformation").GetComponent<PlayerTeams>().FindTeamAllegiance(tile1.GetComponent<PlayerInformation>().CharactersTeam)
            == GameObject.Find("GameInformation").GetComponent<PlayerTeams>().FindTeamAllegiance(tile2.GetComponent<PlayerInformation>().CharactersTeam);
    }
}