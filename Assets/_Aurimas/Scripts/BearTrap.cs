using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : Consumable
{
    public int trapDamage = 6;
    public GameObject creator;
    private PlayerTeams _playerTeams;
    public override void PickUp(GameObject whoStepped)
    {
        if (whoStepped.gameObject.CompareTag("Player") && !isAllegianceSame(whoStepped, creator))
        {
            whoStepped.GetComponent<PlayerInformation>().DealDamage(trapDamage,false,creator);
            Destroy(transform.parent.gameObject);
        }
    }

    private bool isAllegianceSame(GameObject tile1, GameObject tile2)
    {
        return _playerTeams.FindTeamAllegiance(tile1.GetComponent<PlayerInformation>().CharactersTeam)
               == _playerTeams.FindTeamAllegiance(tile2.GetComponent<PlayerInformation>().CharactersTeam);
    }
}
