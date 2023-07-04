using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    public GameObject cagedCharacter;
    
    void Start()
    {
        cagedCharacter.GetComponent<PlayerInformation>().Caged = true;
        cagedCharacter.GetComponent<BoxCollider2D>().enabled = false;
    }
    public void FreeCagedCharacter()
    {
        cagedCharacter.GetComponent<PlayerInformation>().Caged = false;
        cagedCharacter.GetComponent<BoxCollider2D>().enabled = true;
        var destinations = GameObject.Find("GameInformation").gameObject.GetComponent<AIManager>().AIDestinations;
        if (destinations.Count == 1 && destinations[0] == null)
        {
            destinations[0] = cagedCharacter;
        }
    }
}
