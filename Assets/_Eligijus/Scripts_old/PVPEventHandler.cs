using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPEventHandler : MonoBehaviour
{
    private void Awake()
    {
        if (PVPManager.instance == null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("PVPCharacterSelect");
        }
        else
        {
            PVPManager.instance.OnAwake();
        }
    }

    private void Start()
    {
        if(PVPManager.instance == null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("PVPCharacterSelect");
        }
        else
        {
            PVPManager.instance.OnStart();
        }
    }

    public void OnCharacterButtonClick(int characterIndex)
    {
        PVPManager.instance.OnCharacterButtonClick(characterIndex);
    }

    public void OnTeamPortraitClick(int index)
    {
        PVPManager.instance.OnTeamPortraitClick(index);
    }

    public void OnClearButtonClick()
    {
        PVPManager.instance.OnClearButtonClick();
    }

    public void OnNextButtonClick()
    {
        PVPManager.instance.OnNextButtonClick();
    }

    public void OnBackButtonClick()
    {
        PVPManager.instance.OnBackButtonClick();
    }

    public void OnTeamPortraitHover(int index)
    {
        PVPManager.instance.OnTeamPortraitHover(index);
    }

    public void OffTeamPortraitHover(int index)
    {
        PVPManager.instance.OffTeamPortraitHover(index);
    }

    public void OnLongCharacterButtonPress(int index)
    {
        PVPManager.instance.OnLongCharacterButtonPress(index);
    }
}
