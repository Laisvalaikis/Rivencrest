using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableWall : MonoBehaviour
{
    public bool Rock = false;
    public LayerMask groundLayer;
    public GameObject FogOfWarTile;
    public Transform firePoint;
    private RaycastHit2D raycast;
    void Update() //!!!!!! FAKE UPDATE
    {
        if(GetGroundTile(groundLayer).GetComponent<HighlightTile>().FogOfWarTile.activeSelf == true && GetComponent<PlayerInformation>().health > 0) //db po milisekundes:/
        {
            if (!Rock)
            {
                FogOfWarTile.SetActive(true);
                //transform.Find("CharacterModel").gameObject.SetActive(false);
            }
            else 
            {
                transform.Find("CharacterModel").GetComponent<SpriteRenderer>().color = Color.gray;
            }
        }
        else
        {
            if (!Rock)
            {
                FogOfWarTile.SetActive(false);
                //transform.Find("CharacterModel").gameObject.SetActive(true);
            }
            else
            {
                transform.Find("CharacterModel").GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
    private GameObject GetGroundTile(LayerMask chosenLayer)
    {
        Vector3 firstPosition = firePoint.transform.position;
        Vector3 secondPosition = firstPosition + new Vector3(0.1f, 0f, 0f);
        raycast = Physics2D.Linecast(firstPosition, secondPosition, chosenLayer);
        if (raycast.transform == null)
        {
            return null;
        }
        return raycast.transform.gameObject;
    }
}
