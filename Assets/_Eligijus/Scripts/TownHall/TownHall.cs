using System.Collections.Generic;
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
    private Data _data;
    private void Start()
    {
        imageComponent = GetComponent<Image>();
        _data = Data.Instance;
        // _data.townData.hasClickedTH = true;
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
            button.UpdateUpgradeButton();
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
        UpdateButtons();
        gameObject.SetActive(false);
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

        UpdateButtons();
    }
}
