using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotCard : MonoBehaviour
{
    [SerializeField] private int slotIndex;
    [SerializeField] private GameObject addButton;
    [SerializeField] private GameObject slotMenu;
    [SerializeField] private TextMeshProUGUI slotTitle;
    // Start is called before the first frame update
    void Start()
    {
        bool saveExist = SaveSystem.DoesSaveFileExist(slotIndex);
        addButton.SetActive(!saveExist);
        slotMenu.SetActive(saveExist);
        if(saveExist)
        {
            slotTitle.text = SaveSystem.LoadTownData(slotIndex).slotName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
