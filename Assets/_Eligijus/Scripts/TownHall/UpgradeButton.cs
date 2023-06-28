using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public Sprite DefaultSprite;
    public Sprite UpgradedSprite;
    public UpgradeData upgradeData;
    public Image frame;
    public TextMeshProUGUI text;
    public ImageFadeController imageFadeController;
    private Button _button;
    private Data _data;

    private void Start()
    {
        _button = GetComponent<Button>();
        _data = Data.Instance;
        imageFadeController.gameObject.SetActive(true);
    }

    public void UpdateUpgradeButton()
    {
        TownHallData townHall = _data.townData.townHall;
        if (townHall.GetByType((TownHallUpgrade)upgradeData.upgradeIndex) + 1 < upgradeData.upgradeValue)//negalimi pirkti nes per auksti
        {
            _button.interactable = false;
        }
        else if (townHall.GetByType((TownHallUpgrade)upgradeData.upgradeIndex) + 1 > upgradeData.upgradeValue) //nupirkti
        {
            _button.interactable = false;
            frame.sprite = UpgradedSprite;
            text.color = Color.white;
        }
        else {
            _button.interactable = true;
        }//galimas pirkti
    }
}
