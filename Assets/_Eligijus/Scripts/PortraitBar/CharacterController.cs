using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private CharacterTable characterTable;
    [SerializeField] private PortraitBar portraitBar;
    [SerializeField] private Data data;
    [SerializeField] private GameUi gameUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SellCharacter(int characterIndex)
    {
        int cost = data.AllAvailableCharacters.Find(x => x.prefab == data.Characters[characterIndex].prefab).cost;
        data.townData.townGold += cost / 2;
        gameUI.EnableGoldChange("+" + cost / 2 + "g");
        gameUI.UpdateTownCost();
        data.Characters.RemoveAt(characterIndex);
        characterTable.gameObject.SetActive(false);
        portraitBar.RemoveCharacter(characterIndex);
        // fix gameProgress
        characterTable.UpdateTable();
    }
    
}
