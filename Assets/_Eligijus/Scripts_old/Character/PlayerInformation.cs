using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerInformation : MonoBehaviour
{
    [SerializeField] private InformationType type = InformationType.Player;
    public PlayerInformationData playerInformationData;
    private PlayerInformationData _playerInformationData;
    public SavedCharacter savedCharacter;
    // private ActionManager actionManager;
    private DamageText damageTextas;
    private PlayerAttack playerAttack;
    private TextMeshPro textMeshPro;
    // private GridMovement gridMovement;
    private PlayerTeams playerTeams;
    // private CreateWhiteField createWhiteField;
    // private MindControl mindControl;
    private CharacterModel characterModel;
    private BoxCollider2D boxCollider2D;
    private VFXContainer vfxContainer;
    private TeamInformation teamInformation;
    private AIManager aiManager;

    [SerializeField] public SpriteRenderer spriteRenderer;

    [SerializeField] private Animator animator;
    private int _health = 100;
    
    public string CharactersTeam = "Default";
    //[HideInInspector] public GameObject characterPortrait; //portraitBoxFrame
    [HideInInspector] public GameObject TeamManager;
    [HideInInspector] public GameObject FlagInHand = null;
    public string role;
    // public Sprite CharacterSplashArt;//For character table
    [HideInInspector] public Debuffs Debuffs; // :'( uzsirasyti kazkur visus imanomus debuffus
    [HideInInspector] public bool Blocker = false; //is this character blocking another ally
    [HideInInspector] public GameObject BlockingAlly = null;
    [HideInInspector] public GameObject BarrierProvider = null;
    [HideInInspector] public GameObject Marker = null;
    [HideInInspector] public GameObject PinkWeakSpot = null;
    [HideInInspector] public bool Slow1 = false;
    [HideInInspector] public bool Slow2 = false;
    [HideInInspector] public bool Slow3 = false;
    [HideInInspector] public bool CantMove = false;
    [HideInInspector] public bool Silenced = false;
    [HideInInspector] public bool MindControlled = false;
    [HideInInspector] public GameObject Aflame = null; //DamageDealer
    [HideInInspector] public bool Danger = false;
    [HideInInspector] public bool IsCreatingWhiteField = false;
    [HideInInspector] public GameObject VisionGameObject;
    [HideInInspector] public bool CantAttackCondition = false;
    [HideInInspector] public bool Protected = false;
    [HideInInspector] public bool Stasis = false;
    [HideInInspector] public List<string> enabledAbilitiesEnemy;

    [HideInInspector] public int XPToGain = 0;
    [HideInInspector] public bool isThisObject = false;

    private int _currentCharacterTeam = -1;
   
    private int turnCounter = 1;
    void Awake()
    {
        _playerInformationData = new PlayerInformationData();
        _playerInformationData.CopyData(playerInformationData);
    }
    void Start()
    {
        LoadPlayerProgression();
        PlayerSetup();
        _health = _playerInformationData.MaxHealth;
    }

    public float GetHealthPercentage()
    {
        float maxHealthDouble = _playerInformationData.MaxHealth;
        float healthDouble = _health;
        return healthDouble / maxHealthDouble * 100;
    }

    public InformationType GetInformationType()
    {
        return type;
    }
    
    public void SetInformationType(InformationType informationType)
    {
        type = informationType;
    }

    public void SetPlayerTeam(int currentCharacterTeam)
    {
        _currentCharacterTeam = currentCharacterTeam;
    }

    public int GetPlayerTeam()
    {
        return _currentCharacterTeam;
    }

    public enum SpecialColor
    {
        Poison = 0,
        PinkWeakSpot = 1,
        Protected = 2,
        None = 3
    };

    public void DealDamage(int damage, bool crit, GameObject damageDealer, string specialInformation = "")
    {
       // List<TextMeshProUGUI> damageTextTest = new List<TextMeshProUGUI>();
        if (damage != -1) 
        { 
            if (Protected || Stasis)
            { 
                damage /= 2; 
            } 
            _health -= damage;
        }
        if (_health <= 0) // DEATH
        {
            DeathStart();
        }
        else
        {
            
        }
           
    }

    public int GetHealth()
    {
        return _health;
    }

    public PlayerInformationData GetPlayerInformationData()
    {
        return _playerInformationData;
    }

    public void DeathStart()
    {
       
    }

    public void Die()
    { 
        spriteRenderer.enabled = false;
        animator.enabled = false;
        vfxContainer?.gameObject.SetActive(false);
        if (isThisObject)
        {
            Destroy(gameObject);
        }
        //gameObject.SetActive(false);
    }

   public void AddKillXP()
    {
        // if (!isThisObject)
        // {
        //     XPToGain = 0;
        //     foreach (GameObject x in KillList)
        //     {
        //         _data.statistics.killCountByClass[Statistics.GetClassIndex(_playerInformationData.ClassName)]++; // vietoj playerInformationData.ClassName buvo ClassName
        //         _data.globalStatistics.killCountByClass[Statistics.GetClassIndex(_playerInformationData.ClassName)]++; // vietoj playerInformationData.ClassName buvo ClassName
        //         XPToGain += 50;
        //     }
        // }
    }
    public void Heal(int healAmount, bool crit)
    {
        if (_health + healAmount >= _playerInformationData.MaxHealth)
        {
            _health = _playerInformationData.MaxHealth;
        }
        else
        {
            _health += healAmount;
        }
    }
    public void ApplyDebuff(string debuff, GameObject DebuffApplier = null)
    {
       
    }
    public void ToggleSelectionBorder(bool state)
    {
       
    }
    public void PlayerSetup()
    {
        Color TeamUIColor = ColorStorage.TeamColor(CharactersTeam);
        if (CharactersTeam == "Default")
        {
            Debug.LogError("Fix Comment");
        }
        else if (vfxContainer != null)
        {
            spriteRenderer.color = TeamUIColor;
        }
    }
    public void LoadPlayerProgression()
    {
       
    } 
   
    public virtual void OnTurnStart()
    {
        
    }
    public void OnTurnEnd()
    {
       
    }
    
    public int TotalPoisonDamage()
    {
        int totalDamage = 0;
        //foreach (Poison x in Poisons)
       // {
        //    totalDamage += x.poisonValue;
        //}
        return totalDamage;
    }
    IEnumerator ExecuteAfterTime(float time, Action task)
    {
        yield return new WaitForSeconds(time);
        task();
    }

}
