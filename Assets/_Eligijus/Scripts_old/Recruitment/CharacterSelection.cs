using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : PortraitButton
{
    public int characterIndex = 0;
    public CharacterSelect chharacterSelect;

    public override void OnPortraitClick()
    {
        chharacterSelect.OnCharacterButtonClick(characterIndex);
    }

    public void DisplayCharacterInfo()
    {
        GameObject.Find("Canvas").transform.Find("CharacterTable").GetComponent<CharacterTable>().DisplayCharacterTable(characterIndex);
        // GameObject.Find("GameProgress").GetComponent<GameProgress>().DisplayCharacterTable(characterIndex);
        Debug.Log("Pakeisti sita vieta");
    }
}
