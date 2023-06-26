using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteField : MonoBehaviour
{
    public List<GameObject> Owners = new List<GameObject>();

    public void RemoveOwner(GameObject Owner)
    {
        Owners.Remove(Owner);
        if(Owners.Count == 0)
        {
            Destroy(gameObject);
        }
    }
    public bool isCharacterProtected(GameObject character)
    {
        foreach(GameObject owner in Owners)
        {
            if(isAllegianceSame(owner, character))
            {
                return true;
            }
        }
        return false;
    }

    private bool isAllegianceSame(GameObject character1, GameObject character2)
    {
        return GameObject.Find("GameInformation").GetComponent<PlayerTeams>().FindTeamAllegiance(character1.GetComponent<PlayerInformation>().CharactersTeam)
            == GameObject.Find("GameInformation").GetComponent<PlayerTeams>().FindTeamAllegiance(character2.GetComponent<PlayerInformation>().CharactersTeam);
    }

}
