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
    public Button button;
    private bool enabled = true;
    private Data _data;

    private void OnEnable()
    {
        if (_data == null && Data.Instance != null)
        {
            _data = Data.Instance;
        }

        UpdateUpgradeButton();
    }

    private void Start()
    {
        imageFadeController.gameObject.SetActive(true);
    }

    public void UpdateUpgradeButton()
    {
        if (enabled)
        {

            TownHallData townHall = _data.townData.townHall;
            if (townHall.GetByType((TownHallUpgrade)upgradeData.upgradeIndex) + 1 <
                upgradeData.upgradeValue) //negalimi pirkti nes per auksti
            {
                button.interactable = false;
            }
            else if (townHall.GetByType((TownHallUpgrade)upgradeData.upgradeIndex) + 1 >
                     upgradeData.upgradeValue) //nupirkti
            {
                button.interactable = false;
                frame.sprite = UpgradedSprite;
                text.color = Color.white;
            }
            else
            {
                button.interactable = true;
            } //galimas pirkti
        }
    }

    public void Pause(bool pause)
    {
        enabled = !pause;
    }
}
