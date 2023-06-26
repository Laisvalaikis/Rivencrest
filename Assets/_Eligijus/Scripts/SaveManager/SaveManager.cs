using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class SaveManager: MonoBehaviour
{
    [SerializeField] private Button buttonForCreation;
    private int _difficulty;
    private string _color;
    private string _teamName;
    void Start()
    {
        _difficulty = -1;
        _color = "";
        if (buttonForCreation != null) 
        {
            buttonForCreation.interactable = false;
        }
    }

    public void TextInput(string text)
    {
        if(text.Length > 0 && text[0] == ' ')
        {
            text = "";
            _teamName = "";
        }
        text = text.ToUpper();
        _teamName = text;
        if (text != "" && _difficulty != -1 && _color != "")
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
        SaveSystem.SaveTownData(TownData.NewGameData(_color, _difficulty, _teamName));
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
        if (_teamName != "" && difficulty != -1 && _color != "")
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
        if (_teamName != "" && _difficulty != -1 && color != "")
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
