using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebuffManager : MonoBehaviour
{
    private List<string> debuffs;
    public GameObject CharacterOnBoard;
    public float offsetStartPosition = 0f;
    public float offsetDebuff = 80f;
    public void UpdateDebuffs()
    {
        UpdateDebuffList();
        UpdateDebuffIcons();
        if(GameObject.Find("GameInformation").GetComponent<GameInformation>().SelectedCharacter != CharacterOnBoard
            && GameObject.Find("GameInformation").GetComponent<GameInformation>().InspectedCharacter != CharacterOnBoard)
        {
            DisableDebuffText();
        }
    }
    private void UpdateDebuffList()
    {
        int k = 0;
        var character = CharacterOnBoard.GetComponent<PlayerInformation>();
        debuffs = new List<string>();
        if (character.IsCreatingWhiteField)
        {
            debuffs.Add("WhiteField");
            k++;
        }
        if (character.Protected)
        {
            debuffs.Add("Protected");
            k++;
            character.transform.Find("VFX").Find("Protected").gameObject.SetActive(true);
        }
        else
        {
            character.transform.Find("VFX").Find("Protected").gameObject.SetActive(false);
        }
        if (character.Blocker)
        {
            debuffs.Add("Blocker");
            k++;
        }
        if (character.BlockingAlly != null && character.BlockingAlly.GetComponent<PlayerInformation>().health > 0)
        {
            debuffs.Add("Block");
            k++;
            character.transform.Find("VFX").Find("Block").gameObject.SetActive(true);
        }
        else
        {
            character.transform.Find("VFX").Find("Block").gameObject.SetActive(false);
        }
        if (character.BarrierProvider != null)
        {
            debuffs.Add("Barrier");
            k++;
        }
        if (character.CantAttackCondition)
        {
            debuffs.Add("CantAttack");
            k++;
            character.transform.Find("VFX").Find("Disarm").gameObject.SetActive(true);
        }
        else
        {
            character.transform.Find("VFX").Find("Disarm").gameObject.SetActive(false);
        }
    
        if (character.Marker != null)
        {
            debuffs.Add("Marked");
            k++;
            //character.transform.Find("VFX").Find("Mark").GetComponent<Animator>().SetTrigger("start");
            character.transform.Find("VFX").Find("Mark").GetComponent<Animator>().SetBool("mark", true);
            //character.transform.Find("VFX").Find("Mark").gameObject.SetActive(true);
        }
        else
        {
            //character.transform.Find("VFX").Find("Mark").GetComponent<Animator>().SetTrigger("end");
            character.transform.Find("VFX").Find("Mark").GetComponent<Animator>().SetBool("mark", false);
            //character.transform.Find("VFX").Find("Mark").gameObject.SetActive(false);
        }
        if (character.PinkWeakSpot != null)
        {
            debuffs.Add("WeakSpot");
            k++;
            character.transform.Find("VFX").Find("PinkWeakSpot").gameObject.SetActive(true);
        }
        else
        {
            character.transform.Find("VFX").Find("PinkWeakSpot").gameObject.SetActive(false);
        }
        if (character.Debuffs.Contains("Stun"))
        {
            debuffs.Add("Stun");
            k++;
        }
        if (character.Stasis)
        {
            debuffs.Add("Stasis");
            k++;
        }
        if (character.CantMove)
        {
            debuffs.Add("CantMove");
            k++;
            character.transform.Find("VFX").Find("CantMove").gameObject.SetActive(true);
        }
        else
        {
            character.transform.Find("VFX").Find("CantMove").gameObject.SetActive(false);
        }

        if (character.Slow1)
        {
            debuffs.Add("Slow1");
            k++;
        }
        if (character.Slow2)
        {
            debuffs.Add("Slow2");
            k++;
        }
        if (character.Slow3)
        {
            debuffs.Add("Slow3");
            k++;
        }
        if (character.Silenced)
        {
            debuffs.Add("Silenced");
            k++;
            character.transform.Find("VFX").Find("Silenced").gameObject.SetActive(true);
            character.transform.Find("VFX").Find("Silenced").GetComponent<Animator>().SetBool("mindControl", character.MindControlled);
        }
        else
        {
            character.transform.Find("VFX").Find("Silenced").gameObject.SetActive(false);
        }
        if (character.MindControlled)
        {
            //debuffs.Add("MindControlled");
        }
        if (character.Aflame != null)
        {
            character.transform.Find("VFX").Find("Aflame").GetComponent<Animator>().SetBool("aflame", true);
            debuffs.Add("Aflame");
            k++;
        }
        else if (character.transform.Find("VFX").Find("Aflame").GetComponent<Animator>().GetBool("aflame"))
        {
            character.transform.Find("VFX").Find("Aflame").GetComponent<Animator>().SetBool("aflame", false);
        }
        if (character.Poisons.Count > 0)
        {
            debuffs.Add("Poison");
            k++;
            character.transform.Find("VFX").Find("Poison").gameObject.SetActive(true);
        }
        else
        {
            character.transform.Find("VFX").Find("Poison").gameObject.SetActive(false);
        }
    
        if (character.Danger)
        {
            debuffs.Add("Danger");
            k++;
        }
        // transform.Find("HelpButton").gameObject.SetActive(k != 0);
        // pratesti
    }
    private void UpdateDebuffIcons()
    {
        float yPosition = offsetStartPosition;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            if (debuffs.Find(x => x == transform.GetChild(i).name) != null)
            {
                if(transform.GetChild(i).name == "Poison")
                {
                    transform.GetChild(i).gameObject.transform.Find("PoisonText").gameObject.GetComponent<Text>().text = 
                        CharacterOnBoard.GetComponent<PlayerInformation>().TotalPoisonDamage().ToString();
                    transform.GetChild(i).gameObject.transform.Find("DebuffText").gameObject.GetComponent<Text>().text = "Poisoned by " +
                        CharacterOnBoard.GetComponent<PlayerInformation>().TotalPoisonDamage().ToString() + ".";
                }
                transform.GetChild(i).gameObject.SetActive(true);
                transform.GetChild(i).transform.localPosition = new Vector3(0f, yPosition, 0f);
                yPosition += offsetDebuff;
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

    }
    // public void DisplayDebuffText()
    // {
    //     for (int i = 0; i < transform.childCount - 1; i++)
    //     {
    //         if(transform.GetChild(i).transform.Find("DebuffText").gameObject.activeSelf)
    //         {
    //             transform.GetChild(i).transform.Find("DebuffText").gameObject.SetActive(false);
    //         }
    //         else transform.GetChild(i).transform.Find("DebuffText").gameObject.SetActive(true);
    //     }
    // }
    private void DisableDebuffText()
    {
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            transform.GetChild(i).transform.Find("DebuffText").gameObject.SetActive(false);
        }
    }
    // public void Update()
    // {
    //     if(Input.GetKeyDown("i"))
    //     {
    //         DisplayDebuffText();
    //     }
    // }
}
