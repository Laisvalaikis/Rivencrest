using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public class EndTurn : MonoBehaviour
{
    public bool transitions = false;
    public float countDown = 0.5f;
    public GameObject Camera;
    public Sprite highlightedSprite;
    public Sprite normalSprite;
    public Color highlightedColor;
    public Color normalColor;
    private bool highlighted = false;
    [HideInInspector] public bool confirmState = false;

    public void EndPlayersTurn()
    {
        GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().EndTurn();
    }

    public void DisplayEndTurnScreen()
    {
        if(transitions)
        {
            GameObject character = GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().SelectedCharacter;
            if (character != null)
            {
                GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().SelectACharacter(character);
            }
            int activeTeamIndex = GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().activeTeamIndex;
            if (activeTeamIndex + 1 == GameObject.Find("GameInformation").gameObject.GetComponent<PlayerTeams>().allCharacterList.teams.Count)
            {
                activeTeamIndex = 0;
            }
            else
            {
                activeTeamIndex++;
            }
            string teamName = GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().GetComponent<PlayerTeams>().allCharacterList.teams[activeTeamIndex].teamName;
            GameObject.Find("Canvas").transform.Find("PortraitBoxesContainer").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("EndTurn").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("TopRightCornerUI").transform.Find("HelpButton").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("CornerUIManagerContainer").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("TopRightCornerUI").transform.Find("PortalButton").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("UndoTurn").gameObject.SetActive(false);
            GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().DisableVisionTiles();
            /*
            if (GameObject.Find("GameInformation").gameObject.GetComponent<PlayerTeams>().allCharacterList.teams[activeTeamIndex].lastSelectedPlayer.GetComponent<PlayerInformation>().health > 0)
            {
                GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().FocusSelectedCharacter(GameObject.Find("GameInformation").gameObject.GetComponent<PlayerTeams>().allCharacterList.teams[activeTeamIndex].lastSelectedPlayer);
            }
            else if (GameObject.Find("GameInformation").gameObject.GetComponent<PlayerTeams>().FirstAliveCharacter(activeTeamIndex) != null)
            {
                GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().FocusSelectedCharacter(GetComponent<PlayerTeams>().FirstAliveCharacter(activeTeamIndex));
            }
            Camera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 1;
            Camera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = 1;
            Camera.GetComponent<CinemachineVirtualCamera>().Follow = GameObject.Find("MapMiddle").transform;
            */
            Color teamColor = ColorStorage.TeamColor(teamName);
            //GameObject.Find("Canvas").transform.Find("EndTurnScreen").transform.Find("EndTurnStripe").GetComponent<Image>().color = new Color(teamColor.r, teamColor.g, teamColor.b, 190/255f);
            GameObject.Find("Canvas").transform.Find("EndTurnScreen").transform.Find("EndTurnStripeWhite1").GetComponent<Image>().color = teamColor;
            GameObject.Find("Canvas").transform.Find("EndTurnScreen").transform.Find("EndTurnStripeWhite2").GetComponent<Image>().color = teamColor;
            GameObject.Find("Canvas").transform.Find("EndTurnScreen").transform.Find("TeamName").GetComponent<Text>().text = "Team " + teamName;
            GameObject.Find("Canvas").transform.Find("EndTurnScreen").transform.Find("TeamName").GetComponent<Text>().color = teamColor;
            GameObject.Find("Canvas").transform.Find("EndTurnScreen").transform.Find("PressAnywhere").GetComponent<Text>().color = teamColor;
            GameObject.Find("Canvas").transform.Find("EndTurnScreen").gameObject.SetActive(true);
            GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().isBoardDisabled = true;
            GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().isEndTurnScreenEnabled = true;
            GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().endTurnCountDown = countDown;
            //endTurnScreenEnabled = true;
        }

        else
        {
            EndPlayersTurn();
        }
    }

    public void ChangeSprite(bool highlightButton)
    {
        if (highlightButton)
        {
            GetComponent<Image>().sprite = highlightedSprite;
            transform.Find("Text").GetComponent<TextMeshProUGUI>().color = highlightedColor;
            highlighted = true;
            Debug.Log("Change This");
        }
        else
        {
            GetComponent<Image>().sprite = normalSprite;
            transform.Find("Text").GetComponent<TextMeshProUGUI>().color = normalColor;
            highlighted = false;
            Debug.Log("Change This");
        }
    }

    public void EndTurnButtonClicked()
    {
        if(confirmState || highlighted)
        {
            DisplayEndTurnScreen();
            confirmState = false;
            transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "END TURN";
            Debug.Log("Reikia sutvarkyti cia");
        }
        else
        {
            confirmState = true;
            transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "CONFIRM";
            Debug.Log("Reikia sutvarkyti cia");
        }
    }

    /*void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("LOL");
            GameObject.Find("Canvas").transform.Find("PortraitBoxesContainer").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("EndTurn").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("HelpButton").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("CornerUIManagerContainer").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("PortalButton").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("EndTurnScreen").gameObject.SetActive(false);
            GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().buttonClicked = false;
            endTurnScreenEnabled = false;
            EndPlayersTurn();
        }
    }*/
}
