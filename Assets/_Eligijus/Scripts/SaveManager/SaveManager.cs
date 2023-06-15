using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class SaveManager: MonoBehaviour
{
    [SerializeField] private InputField slotNameInput;
    [SerializeField] private Button buttonForCreation;
    private int _difficulty;
    private string _color;


    void Start()
    {
        _difficulty = -1;
        _color = "";
        buttonForCreation.interactable = false;
    }
    public void OnInputValueChanged()
    {
        if(slotNameInput.text == " ")
        {
            slotNameInput.text = "";
        }
        slotNameInput.text = slotNameInput.text.ToUpper();
        if (slotNameInput.text != "" && _difficulty != -1 && _color != "")
        {
            buttonForCreation.interactable = true;
        }
        else
        {
            buttonForCreation.interactable = false;
        }
    }

    public void StartNewGame()
    {
        string slotName = slotNameInput.text;
        SaveSystem.SaveTownData(TownData.NewGameData(_color, _difficulty, slotName));
    }

    public void SaveData(int slotIndex)
    {
        SaveSystem.SaveCurrentSlot(slotIndex);
        if (SaveSystem.LoadStatistics(true) == null)
        {
            SaveSystem.SaveStatistics(new Statistics(), true);
        }
        if (SaveSystem.LoadStatistics() == null)
        {
            SaveSystem.SaveStatistics(new Statistics());
        }
    }

    public void DeleteSlot(int slotIndex)
    {
        SaveSystem.DeleteSlot(slotIndex);
    }

    public void ClearGameData()
    {
        SaveSystem.ClearGameData();
    }

    public void SetDifficulty(int difficulty)
    {
        _difficulty = difficulty;
        if (slotNameInput.text != "" && difficulty != -1 && _color != "")
        {
            buttonForCreation.interactable = true;
        }
        else
        {
            buttonForCreation.interactable = false;
        }
    }

    public void SetColor(string color)
    {
        _color = color;
        if (slotNameInput.text != "" && _difficulty != -1 && color != "")
        {
            buttonForCreation.interactable = true;
        }
        else
        {
            buttonForCreation.interactable = false;
        }
    }

    public void ResetData()
    {
        _difficulty = -1;
        _color = "";
        buttonForCreation.interactable = false;
    }
}
