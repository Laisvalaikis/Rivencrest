﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TownHall : MonoBehaviour
{
    public List<UpgradeButton> upgradeButtons;
    public TextMeshProUGUI upgradeNameText;
    public TextMeshProUGUI upgradeDescriptionText;
    public TextMeshProUGUI upgradeCostText;
    public Button buyButton;
    public GameObject backgroundForText;
    public ImageFadeController imageFadeController;
    public GameUi gameUi;
    public Sprite[] sprites;
    private Image imageComponent;
    private UpgradeButton SelectedUpgrade;
    private bool pause = false;
    private Data _data;
    private List<bool> buttonState;
    private void OnEnable()
    {
        if (_data == null)
        {
            imageComponent = GetComponent<Image>();
            _data = Data.Instance;
            buttonState = new List<bool>();
            for (int i = 0; i < upgradeButtons.Count; i++)
            {
                buttonState.Add(false);
            }
        }
        // _data.townData.hasClickedTH = true;
    }

    public void DisableAllButtons()
    {
        if (buttonState == null)
        {
            buttonState = new List<bool>();
        }
        else if (buttonState.Count > 0)
        {
            for (int i = 0; i < buttonState.Count; i++)
            {
                buttonState[i] = upgradeButtons[i].button.interactable;
                upgradeButtons[i].button.interactable = false;
                upgradeButtons[i].Pause(true);
            }
            pause = true;
        }
    }

    public void EnableAllButtons()
    {
        if (buttonState != null && buttonState.Count > 0)
        {
            for (int i = 0; i < upgradeButtons.Count; i++)
            {
                upgradeButtons[i].Pause(false);
                upgradeButtons[i].button.interactable = buttonState[i];
                
            }
            pause = false;
        }
    }

    public void SetupMerchantSprite()
    {
        if (_data.townData.townHall.damagedMerchant == 1)
        {
            imageComponent.sprite = sprites[0];
        }
        else if (_data.townData.townHall.damagedMerchant == 2)
        {
            imageComponent.sprite = sprites[1];
        }
        backgroundForText.SetActive(true);
    }
    public void UpdateButtons()
    {
        

            foreach (UpgradeButton button in upgradeButtons)
            {
                if (button.gameObject.activeInHierarchy)
                {
                    button.UpdateUpgradeButton();
                }
            }

            if (SelectedUpgrade != null)
            {
                upgradeNameText.gameObject.SetActive(true);
                upgradeDescriptionText.gameObject.SetActive(true);
                upgradeCostText.gameObject.SetActive(true);
                backgroundForText.gameObject.SetActive(true);

                upgradeNameText.text = SelectedUpgrade.upgradeData.upgradeName;
                upgradeDescriptionText.text = SelectedUpgrade.upgradeData.upgradeDescription;
                upgradeCostText.text = "-" + SelectedUpgrade.upgradeData.upgradeCost.ToString() + "g";
                buyButton.interactable = _data.townData.townGold >= SelectedUpgrade.upgradeData.upgradeCost;
            }
            else
            {
                upgradeNameText.gameObject.SetActive(false);
                upgradeDescriptionText.gameObject.SetActive(false);
                upgradeCostText.gameObject.SetActive(false);
                buyButton.gameObject.SetActive(false);
            }
        
    }

    public void CloseTownHall()
    {
        SelectedUpgrade = null;
        pause = false;
        UpdateButtons();
    }
    
    public void BuyUpgrade()
    {
        if (buyButton.interactable)
        {
            TownHallData townHall = _data.townData.townHall;
            townHall.SetByType((TownHallUpgrade)SelectedUpgrade.upgradeData.upgradeIndex, SelectedUpgrade.upgradeData.upgradeValue);
            GameManager.Instance.SpendGold(SelectedUpgrade.upgradeData.upgradeCost);
            gameUi.EnableGoldChange("-" + SelectedUpgrade.upgradeData.upgradeCost + "g");
            gameUi.UpdateTownCost();
            UpdateButtons();
        }
    }
    public void SelectUpgrade(UpgradeButton upgradeButton)
    {
        if (!pause)
        {
            if (upgradeButton != null)
            {
                if (SelectedUpgrade == upgradeButton)
                {
                    SelectedUpgrade = null;
                    imageFadeController.FadeOut();
                }
                else
                {
                    SelectedUpgrade = upgradeButton;
                    imageFadeController.FadeIn();
                }
            }
            else
            {
                Debug.LogError("Select upgrade null value");
                SelectedUpgrade = null;
                imageFadeController.FadeOut();
            }

            // upgradeButton.UpdateUpgradeButton();
            UpdateButtons();
        }
    }
}
