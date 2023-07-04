using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalButton : MonoBehaviour
{
    public GameObject ActivePortal;
    public void PressButton()
    {
        if (ActivePortal != null)
        {
            ActivePortal.GetComponent<Portal>().Teleport();
        }
    }
    void Update()
    {
        if(ActivePortal == null)
        {
            gameObject.SetActive(false);
        }
    }
    public void OnHover()
    {
        GameObject.Find("GameInformation").GetComponent<GameInformation>().isBoardDisabled = true;
    }
    public void OffHover()
    {
        GameObject.Find("GameInformation").GetComponent<GameInformation>().isBoardDisabled = false;

    }
}
