using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PreviewMode
{
    static public void EnablePreviewMode(List<GameObject> characters, GameObject Overlay)//, GameObject WallsFolder,  
    {
        Overlay?.SetActive(true);
        foreach (GameObject x in characters) 
        {
            int health = x.GetComponent<PlayerInformation>().health;
            if (health > 0) 
            {
                x.transform.Find("VFX").Find("PreviewCharacter").gameObject.SetActive(true);
                string team = x.GetComponent<PlayerInformation>().CharactersTeam;
                x.transform.Find("VFX").Find("PreviewCharacter").gameObject.GetComponent<SpriteRenderer>().color = ColorStorage.TeamColor(team);
                x.transform.Find("VFX").Find("DamageText").gameObject.SetActive(true);
                x.transform.Find("VFX").Find("DamageText").gameObject.GetComponent<TextMeshPro>().text = health.ToString() + "HP";
            }
        }
    }
    static public void DisablePreviewMode(List<GameObject> characters, GameObject Overlay)//, GameObject WallsFolder, GameObject Overlay) 
    {
        Overlay?.SetActive(false);
        foreach (GameObject x in characters)
        {
                x.transform.Find("VFX").Find("PreviewCharacter").gameObject.SetActive(false);
                x.transform.Find("VFX").Find("DamageText").gameObject.SetActive(false);
        }
    }
}
