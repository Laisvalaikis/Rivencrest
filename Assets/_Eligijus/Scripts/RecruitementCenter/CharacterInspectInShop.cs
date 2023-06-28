using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInspectInShop : MonoBehaviour
{
    [SerializeField] private Image characterArt;
    [SerializeField] private Image tableBorder;
    [SerializeField] private TextMeshProUGUI className;
    [SerializeField] private TextMeshProUGUI role;
    [SerializeField] private TextMeshProUGUI maxHP;
    [SerializeField] private TextMeshProUGUI range;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private GameObject damage;
    [SerializeField] private GameObject tank;
    [SerializeField] private GameObject support;
    [SerializeField] private GameObject meleeType;
    [SerializeField] private GameObject rangeType;
    [SerializeField] private List<CharacterAbilityRecruit> _characterAbilityRecruits;
    [SerializeField] private Recruitment _recruitment;

    public void UpdateView(int index)
    {
        SavedCharacter savedCharacter = _recruitment.GetInShopCharacterByIndex(index);
        PlayerInformation character = savedCharacter.prefab.GetComponent<PlayerInformation>();
        Debug.LogError("Redo PlayerInformation data saving");
        
        gameObject.SetActive(true);
        characterArt.sprite = character.CroppedSplashArt;
        tableBorder.color = character.ClassColor2;
        className.text = character.ClassName;
        className.color = character.ClassColor;
        role.text = character.role;
        role.color = character.ClassColor;
        damage.SetActive(character.role == "DAMAGE");
        tank.SetActive(character.role == "TANK");
        support.SetActive(character.role == "SUPPORT");
        maxHP.text = character.MaxHealth + 4 + " HP";
        maxHP.color = character.ClassColor;
        bool melee = character.GetComponent<PlayerAttack>().AttackRange == 1;
        range.text = melee ? "MELEE" : "RANGED";
        range.color = character.ClassColor;
        meleeType.gameObject.SetActive(melee);
        rangeType.SetActive(!melee);
        level.text = "LEVEL " + savedCharacter.level;
        level.color = character.ClassColor;
        for (int i = 0; i < _characterAbilityRecruits.Count; i++)
        {
            _characterAbilityRecruits[i].UpdateCharacterAbility(i, character, savedCharacter);
        }
        
    }
}
