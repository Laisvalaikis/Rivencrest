using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TownHall : MonoBehaviour
{
    [HideInInspector] public GameObject SelectedUpgrade;
    public Data _data;

    public List<UpgradeButton> upgradeButtons;
    public TextMeshProUGUI upgradeNameText;
    public TextMeshProUGUI upgradeDescriptionText;
    public TextMeshProUGUI upgradeCostText;
    public Button buyButton;
    public GameObject backgroundForText;

    public Sprite[] sprites;
    private Image imageComponent;

    private void Start()
    {
        imageComponent = GetComponent<Image>();
        _data.townData.hasClickedTH = true;
    }
    public void SetupMerchantSprite()
    {
        imageComponent = GetComponent<Image>();
        if (_data.townData.townHall[5].ToString() == 1.ToString())
        {
            imageComponent.sprite = sprites[0];
        }
        else if (_data.townData.townHall[5].ToString() == 2.ToString())
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
            UpgradeButton selectedUpgradeButton = SelectedUpgrade.GetComponent<UpgradeButton>();

            upgradeNameText.gameObject.SetActive(true);
            upgradeDescriptionText.gameObject.SetActive(true);
            upgradeCostText.gameObject.SetActive(true);
            backgroundForText.gameObject.SetActive(true);

            upgradeNameText.text = selectedUpgradeButton.upgradeName;
            upgradeDescriptionText.text = selectedUpgradeButton.upgradeDescription;
            upgradeCostText.text = "-" + selectedUpgradeButton.upgradeCost.ToString() + "g";
            buyButton.interactable = _data.townData.townGold >= selectedUpgradeButton.upgradeCost;
        }
        else
        {
            upgradeNameText.gameObject.SetActive(false);
            upgradeDescriptionText.gameObject.SetActive(false);
            upgradeCostText.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(false);
        }
    }

    public void BuyUpgrade()
    {
        if (buyButton.interactable)
        {
            SelectedUpgrade.GetComponent<UpgradeButton>().BuyUpgrade();
            UpdateButtons();
        }
    }

    public void CloseTownHall()
    {
        SelectedUpgrade = null;
        UpdateButtons();
        gameObject.SetActive(false);
    }
}
