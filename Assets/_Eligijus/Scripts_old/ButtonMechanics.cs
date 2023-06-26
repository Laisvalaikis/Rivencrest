using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMechanics : MonoBehaviour
{
    [HideInInspector] public bool hovered = false;
    public void OnHover()
    {
        hovered = true;
        //characterPortrait.GetComponent<Animator>().SetBool("hover", true);
        if(GameObject.Find("GameInformation") != null && GameObject.Find("GameInformation").GetComponent<GameInformation>().canButtonsBeClicked == true)
            GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().isBoardDisabled = true;
    }
    public void OffHover()
    {
        hovered = false;
        //characterPortrait.GetComponent<Animator>().SetBool("hover", false);
        if(GameObject.Find("GameInformation") != null && GameObject.Find("GameInformation").GetComponent<GameInformation>().canButtonsBeClicked == true)
            GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().isBoardDisabled = false;
    }
}
