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
    [SerializeField] private Recruitment recruitment;
    [SerializeField] private HelpTable helpTable;
    [SerializeField] private View view;
    private int _currentCharacterIndex = 0;
    private int _currentCharacterInShop;

    public void DisableCharacterOnBuy(int index)
    {
        if (index == _currentCharacterInShop)
        {
            view.ExitView();
        }
    }

    public void EnableDisableView(int index)
    {
        if (gameObject.activeInHierarchy && index == _currentCharacterInShop)
        {
            view.ExitView();
        }
        else
        {
            UpdateView(index);
        }
    }

    private void UpdateView(int index)
    {
        SavedCharacter savedCharacter = recruitment.GetInShopCharacterByIndex(index);
        _currentCharacterInShop = index;
        _currentCharacterIndex = recruitment.GetRealCharacterIndex(index);
        PlayerInformationData character = savedCharacter.playerInformation;
        view.OpenView();
        characterArt.sprite = character.CroppedSplashArt;
        tableBorder.color = character.secondClassColor;
        className.text = character.ClassName;
        className.color = character.classColor;
        role.text = character.role;
        role.color = character.classColor;
        damage.SetActive(character.role == "DAMAGE");
        tank.SetActive(character.role == "TANK");
        support.SetActive(character.role == "SUPPORT");
        maxHP.text = character.MaxHealth + 4 + " HP";
        maxHP.color = character.classColor;
        Debug.LogError("NEED TO FIX THIS AFTER ABILITIES ARE REMADE");
        bool melee = savedCharacter.prefab.GetComponent<PlayerAttack>().AttackRange == 1;
        range.text = melee ? "MELEE" : "RANGED";
        range.color = character.classColor;
        meleeType.gameObject.SetActive(melee);
        rangeType.SetActive(!melee);
        level.text = "LEVEL " + savedCharacter.level;
        level.color = character.classColor;
        for (int i = 0; i < _characterAbilityRecruits.Count; i++)
        {
            _characterAbilityRecruits[i].gameObject.SetActive(true);
            _characterAbilityRecruits[i].backgroundImage.color = character.backgroundColor;
            _characterAbilityRecruits[i].abilityIcon.color = character.classColor;
            _characterAbilityRecruits[i].abilityIcon.sprite = character.abilities[i].sprite;
        }
        
    }
    
    public void EnableDisableHelpTable(int abilityIndex)
    {
        Transform helpTableTransform = helpTable.transform;
        Vector3 currentPosition = helpTableTransform.position;
        Vector3 position = new Vector3(currentPosition.x, _characterAbilityRecruits[abilityIndex].gameObject.transform.position.y,
            currentPosition.z);
        helpTableTransform.SetPositionAndRotation(position, helpTableTransform.rotation);
        // helpTable.EnableTableForCharacters(abilityIndex, _currentCharacterIndex);
        Debug.LogError("FIX ABILITY INFORMATION");
    }
}
