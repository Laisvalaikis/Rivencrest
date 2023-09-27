using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : Consumable
{
    [HideInInspector] public bool followThePlayer = false;
    private GameObject FlagParent;
    private GameObject toFollow;
    public string FlagsTeam;
    void Start()
    {
        FlagParent = transform.parent.gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        if(followThePlayer == true)
        {
            FlagParent.transform.position = toFollow.transform.position + new Vector3(0f,0f,2f);
        }
    }
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<PlayerInformation>().CharactersTeam != FlagsTeam)
        {
            followThePlayer = true;
            toFollow = collision.gameObject;
            collision.gameObject.GetComponent<PlayerInformation>().FlagInHand = gameObject; 
        }
    }*/
    public override void PickUp(GameObject WhoStepped)
    {
        if (WhoStepped.gameObject.tag == "Player" && WhoStepped.gameObject.GetComponent<PlayerInformation>().CharactersTeam != FlagsTeam
            && WhoStepped.gameObject.GetComponent<PlayerInformation>().FlagInHand == null)
        {
            if (WhoStepped.gameObject.GetComponent<PlayerInformation>().GetHealth() > 0)
            {

                followThePlayer = true;
                toFollow = WhoStepped.gameObject;
                WhoStepped.gameObject.GetComponent<PlayerInformation>().FlagInHand = gameObject;
            }
        }
    }
}
