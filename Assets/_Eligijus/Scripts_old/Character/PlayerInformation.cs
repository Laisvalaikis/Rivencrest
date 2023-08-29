using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerInformation : MonoBehaviour
{
    public PlayerInformationData playerInformationData;
    private PlayerInformationData _playerInformationData;
    public SavedCharacter savedCharacter;
    // private ActionManager actionManager;
    private DamageText damageTextas;
    private PlayerAttack playerAttack;
    private TextMeshPro textMeshPro;
    private GridMovement gridMovement;
    private PlayerTeams playerTeams;
    // private CreateWhiteField createWhiteField;
    // private MindControl mindControl;
    private CharacterModel characterModel;
    private BoxCollider2D boxCollider2D;
    private VFXContainer vfxContainer;
    public GameInformation gameInformation;
    private TeamInformation teamInformation;
    private AIManager aiManager;
    
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Animator animator;
    
    [HideInInspector] public int health = 100;

    private int _health = 100;
    
    public string CharactersTeam = "Default";
    //[HideInInspector] public GameObject characterPortrait; //portraitBoxFrame
    [HideInInspector] public GameObject TeamManager;
    [HideInInspector] public GameObject FlagInHand = null;
    public string currentState = "Movement";
    public string AnimationState = "Movement";
    [HideInInspector] public bool isCurrentStateAvailable;//nereikalinga?
    public bool StateAnimations = false;
    private List<GameObject> KillerList = new List<GameObject>();
    private List<GameObject> KillList = new List<GameObject>();
    private List<TextMeshProUGUI> damageTextTest = new List<TextMeshProUGUI>();
    public string role;
    // public Sprite CharacterSplashArt;//For character table
    public Data _data;
    //
    [HideInInspector] public Debuffs Debuffs; // :'( uzsirasyti kazkur visus imanomus debuffus
    [HideInInspector] public bool Blocker = false; //is this character blocking another ally
    [HideInInspector] public GameObject BlockingAlly = null;
    [HideInInspector] public GameObject BarrierProvider = null;
    [HideInInspector] public GameObject Marker = null;
    [HideInInspector] public GameObject PinkWeakSpot = null;
    [HideInInspector] public bool Stun = false;//
    [HideInInspector] public bool Slow1 = false;
    [HideInInspector] public bool Slow2 = false;
    [HideInInspector] public bool Slow3 = false;
    [HideInInspector] public bool CantMove = false;
    [HideInInspector] public bool Silenced = false;
    [HideInInspector] public bool MindControlled = false;
    [HideInInspector] public GameObject Aflame = null; //DamageDealer
    [HideInInspector] public bool Danger = false;
    [HideInInspector] public bool IsCreatingWhiteField = false;
    [HideInInspector] public GameObject MindControlTarget = null;
    [HideInInspector] public GameObject VisionGameObject;
    [HideInInspector] public bool CantAttackCondition = false;
    [HideInInspector] public bool Disarmed = false;
    [HideInInspector] public GameObject cornerPortraitBoxInGame; //CornerUI
    [HideInInspector] public bool Caged = false;
    [HideInInspector] public bool Protected = false;
    [HideInInspector] public bool MistShield = false; //UndeadKnightSpell
    [HideInInspector] public bool Stasis = false;
    [HideInInspector] public List<Poison> Poisons = new List<Poison>();
    [HideInInspector] public bool wasThisCharacterSpawned = false;
    [HideInInspector] public List<string> enabledAbilitiesEnemy;
    

    public bool Respawn;
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
        //health = MaxHealth;
        health = _playerInformationData.MaxHealth;
        _health = _playerInformationData.MaxHealth;
    }

    public float GetHealthPercentage()
    {
        float maxHealthDouble = _playerInformationData.MaxHealth;
        float healthDouble = health;
        return healthDouble / maxHealthDouble * 100;
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
        if (health + healAmount >= _playerInformationData.MaxHealth)
        {
            health = _playerInformationData.MaxHealth;
        }
        else
        {
            health += healAmount;
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
   
    public void OnTurnStart()
    {
        
    }
    public void OnTurnEnd()
    {
       
    }
    public class Poison
    {
        public GameObject Poisoner;
        public int turnsLeft;
        public int poisonValue;
        public Poison(GameObject poisoner, int turnsleft, int poisonvalue)
        {
            Poisoner = poisoner;
            turnsLeft = turnsleft;
            poisonValue = poisonvalue;
        }
    }
    public int TotalPoisonDamage()
    {
        int totalDamage = 0;
        foreach (Poison x in Poisons)
        {
            totalDamage += x.poisonValue;
        }
        return totalDamage;
    }
    IEnumerator ExecuteAfterTime(float time, Action task)
    {
        yield return new WaitForSeconds(time);
        task();
    }

}
