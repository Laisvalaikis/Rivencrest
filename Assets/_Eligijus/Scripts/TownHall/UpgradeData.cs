using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "ScriptableObjects/MerchantUpgrade", order = 1)]
public class UpgradeData : ScriptableObject
{
    public int upgradeIndex;
    public int upgradeValue;
    public int upgradeCost;
    public string upgradeName;
    public string upgradeDescription;
}
