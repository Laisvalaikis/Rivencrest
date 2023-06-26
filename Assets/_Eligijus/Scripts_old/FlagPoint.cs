using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagPoint : Consumable
{
    public string team;
    private RaycastHit2D raycast;
    public LayerMask blockingLayer;

    void Update()
    {
        if(CheckIfSpecificTag(transform.gameObject, 0, 0, blockingLayer, "Player"))
        {
            PickUp(GetSpecificGroundTile(transform.gameObject, 0, 0, blockingLayer));
        }
    }
    /*private void OnTriggerEnter2D(Collider2D collision)
    {

         if(collision.gameObject.GetComponent<PlayerInformation>().FlagInHand != null
             && collision.gameObject.GetComponent<PlayerInformation>().CharactersTeam == team)
         {
             collision.gameObject.GetComponent<PlayerInformation>().FlagInHand.GetComponent<BoxCollider2D>().enabled = false;
             collision.gameObject.GetComponent<PlayerInformation>().FlagInHand.GetComponent<Flag>().followThePlayer = false;
         }
    }*/
    public override void PickUp(GameObject WhoStepped)
    {
        if (WhoStepped.gameObject.GetComponent<PlayerInformation>().FlagInHand != null
             && WhoStepped.gameObject.GetComponent<PlayerInformation>().CharactersTeam == team)
        {
            WhoStepped.gameObject.GetComponent<PlayerInformation>().FlagInHand.transform.parent.transform.position = transform.position - new Vector3(0f, 0.25f, 0f);
            WhoStepped.gameObject.GetComponent<PlayerInformation>().FlagInHand.GetComponent<BoxCollider2D>().enabled = false;
            WhoStepped.gameObject.GetComponent<PlayerInformation>().FlagInHand.GetComponent<Flag>().followThePlayer = false;
            WhoStepped.gameObject.GetComponent<PlayerInformation>().FlagInHand = null;
            //string winMessage = team.ToString() + " team wins!";
            //Debug.Log(winMessage);
            GameObject.Find("GameInformation").GetComponent<PlayerTeams>().ChampionTeam = team.ToString();
            GameObject.Find("GameInformation").GetComponent<GameInformation>().GameOver();
        }
    }
    protected GameObject GetSpecificGroundTile(GameObject tile, int x, int y, LayerMask chosenLayer)
    {
        Vector3 firstPosition = tile.transform.position + new Vector3(0f, 0.5f, 0f) + new Vector3(x, y, 0f);
        Vector3 secondPosition = firstPosition + new Vector3(0.1f, 0f, 0f);
        raycast = Physics2D.Linecast(firstPosition, secondPosition, chosenLayer);
        return raycast.transform.gameObject;
    }
    protected bool CheckIfSpecificTag(GameObject tile, int x, int y, LayerMask chosenLayer, string tagName)
    {
        Vector3 firstPosition = tile.transform.position + new Vector3(0f, 0.5f, 0f) + new Vector3(x, y, 0f);
        Vector3 secondPosition = firstPosition + new Vector3(0.1f, 0f, 0f);
        raycast = Physics2D.Linecast(firstPosition, secondPosition, chosenLayer);
        if (raycast.transform == null)
        {
            return false;
        }
        else if (raycast.transform.CompareTag(tagName))
        {
            return true;
        }
        return false;
    }
}
