using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HelpTable : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI abilityTitle;
    public TextMeshProUGUI abilityDescription;
    public TextMeshProUGUI cooldownText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI blessingsText;
    public GameObject damageIcon;
    public GameObject isAbilitySlow;
    public GameObject slowAbility;
    public GameObject fastAbility;
    
    
    public void closeHelpTable()
    {
        if(SceneManager.GetActiveScene().name != "PVPAbilitySelect" && SceneManager.GetActiveScene().name != "PVPCharacterSelect")
        {
            if (GameObject.Find("GameInformation") != null)
            {
                //GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().isBoardDisabled = false;
                GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().helpTableOpen = false;
                if (GameObject.Find("Canvas").transform.Find("HelpScreen") != null)
                {
                    GameObject.Find("Canvas").transform.Find("HelpScreen").gameObject.SetActive(false);
                    GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().enableBoardWithDelay(0.5f);
                }
            }
            else
            {
                for (int i = 0; i < GameObject.Find("Canvas").transform.Find("CharacterTable").transform.Find("Abilities").transform.childCount; i++)
                {
                    GameObject.Find("Canvas").transform.Find("CharacterTable").transform.Find("Abilities").transform.GetChild(i).transform.Find("ActionButtonFrame").GetComponent<Animator>().SetBool("select", false);
                }
            }
        }
        gameObject.SetActive(false);
    }
}
