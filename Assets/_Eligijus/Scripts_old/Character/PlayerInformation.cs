using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerInformation : MonoBehaviour
{
    private PlayerInformationData playerInformationData;
    public PlayerInformationData _playerInformationData;
    public SavedCharacter savedCharacter;
    private ActionManager actionManager;
    private DamageText damageTextas;
    private PlayerAttack playerAttack;
    private TextMeshPro textMeshPro;
    private GridMovement gridMovement;
    private Animator animator;
    private PlayerTeams playerTeams;
    private CreateWhiteField createWhiteField;
    private MindControl mindControl;
    //public int MaxHealth = 100;
    [HideInInspector] public int health = 100;
    //public int critChance = 5;
    //public int accuracy = 100;
    //public int dodgeChance = 20;
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
    //
   // public Sprite CharacterPortraitSprite;
   // public Sprite CharacterSplashArt;//For character table
   // public Sprite CroppedSplashArt;
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
    public GameInformation gameInformation;

    public bool Respawn;
    //
    [HideInInspector] public int XPToGain = 0;
    //
    [HideInInspector] public bool isThisObject = false;
    //
   // public string ClassName;
  //  public Color ClassColor;
   // public Color ClassColor2;
    //public List<Blessing> BlessingsAndCurses = new List<Blessing>();
    //
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
        gameInformation = GameInformation.instance;
    }
    void Update()
    {
        if (Input.GetKeyDown("e") && CharactersTeam == "Default")
        {
            GameObject.Find("GameInformation").gameObject.GetComponent<PlayerTeams>().AddCharacterToCurrentTeam(gameObject);
        }
        if (currentState != AnimationState && StateAnimations)
        {
            if (currentState == "Attack" && GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().SelectedCharacter == transform.gameObject)
            {
                transform.Find("CharacterModel").GetComponent<Animator>().SetBool("idleAttack", true);
            }
            else
            {
                transform.Find("CharacterModel").GetComponent<Animator>().SetBool("idleAttack", false);
                //GetComponent<Animator>().SetTrigger("backToIdle");
            }
            AnimationState = currentState;
        }
    }
    public float GetHealthPercentage()
    {
        float maxHealthDouble = _playerInformationData.MaxHealth;
        float healthDouble = health;
        return healthDouble / maxHealthDouble * 100;
    }
    public void DealDamage(int damage, bool crit, GameObject damageDealer, string specialInformation = "")
    {
        if (BarrierProvider == null)
        {
            if (BlockingAlly == null || (BlockingAlly != null && BlockingAlly.GetComponent<PlayerInformation>().health <= 0))
            {
                string SpecialColor = "";
                if (specialInformation == "Poison")
                {
                    SpecialColor = "Poison";
                }
                if (specialInformation == "PinkWeakSpot")
                {
                    transform.Find("VFX").Find("VFXImpact").GetComponent<Animator>().SetTrigger("pink1");
                    transform.Find("VFX").Find("PinkWeakSpot").GetComponent<Animator>().SetTrigger("hit");
                    SpecialColor = "PinkWeakSpot";
                }
                if (specialInformation == "Mark")
                {
                    SpecialColor = "Protected";
                }
                if (damage != -1)
                {
                    if (Protected || Stasis)
                    {
                        damage /= 2;
                        SpecialColor = "Protected";
                    }
                    health -= damage;//Deals damage
                    if (KillerList.Find(x => x == damageDealer) == null)
                    {
                        KillerList.Add(damageDealer);
                    }
                    if (Marker != null && damageDealer != Marker && specialInformation != "Mark")
                    {
                        StartCoroutine(ExecuteAfterTime(0.1f, () =>
                        {
                            PlayerInformationData playerInformationDataMarker =
                                Marker.GetComponent<PlayerInformation>()._playerInformationData;
                            DealDamage(5, false, Marker, "Mark");
                            if (Marker != null && playerInformationDataMarker.BlessingsAndCurses.Find(x => x.blessingName == "Astonishing") != null)//Blessing
                            {
                                ApplyDebuff("Stun", damageDealer);
                            }
                            else
                            {
                                ApplyDebuff("CantMove", damageDealer);
                            }
                            Marker = null;
                            transform.Find("VFX").Find("Mark").GetComponent<Animator>().SetTrigger("explode");
                            gameInformation.FakeUpdate();
                        }));
                    }
                    if (PinkWeakSpot != null && specialInformation != "PinkWeakSpot")
                    {
                        StartCoroutine(ExecuteAfterTime(0.05f, () =>
                        {
                            int weakSpotDamage = 1;
                            PlayerInformationData playerInformationDataPink =
                                PinkWeakSpot.GetComponent<PlayerInformation>()._playerInformationData;
                            if (playerInformationData.BlessingsAndCurses.Find(x => x.blessingName == "Painful spot") != null)//Blessing
                            {
                                weakSpotDamage = 2;
                            }
                            DealDamage(weakSpotDamage, false, PinkWeakSpot, "PinkWeakSpot");
                            gameInformation.FakeUpdate();
                        }));
                    }
                    if (IsCreatingWhiteField)
                    {
                       // GetComponent<CreateWhiteField>().OnTurnStart();//sunaikina white field
                       createWhiteField.OnTurnStart();
                    }
                    if (MindControlTarget != null)
                    {
                        //GetComponent<MindControl>().OnTurnStart();//sustabdo
                        mindControl.OnTurnStart();//sustabdo
                    }
                }
                if (transform.CompareTag("Player"))
                {
                    if (transform.Find("DamageTexts").transform.Find("DamageText1").gameObject.activeSelf)
                    {
                        if (transform.Find("DamageTexts").transform.Find("DamageText2").gameObject.activeSelf)
                        {
                            EnableDamageText(3, damage, crit, false, SpecialColor);
                        }
                        else EnableDamageText(2, damage, crit, false, SpecialColor);
                    }
                    else EnableDamageText(1, damage, crit, false, SpecialColor);
                }
                if (health <= 0) // DEATH
                {
                    transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("death");
                    DeathStart();
                }
                else
                {
                    transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerHit");
                }
            }
            else
            {
                BlockingAlly.GetComponent<PlayerInformation>().DealDamage(damage, crit, damageDealer);
            }
        }
        else
        {
            BarrierProvider = null;
            transform.Find("VFX").Find("VFXBool").GetComponent<Animator>().SetTrigger("shieldEnd");
        }
    }

    public void DeathStart()
    {
        if (wasThisCharacterSpawned)
        {
            GameObject.Find("GameInformation").GetComponent<PlayerTeams>().RemoveCharacterFromTeam(gameObject, CharactersTeam);
        }
        transform.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        if (!CompareTag("Wall"))
        {
            SendKillMessage();
            transform.Find("VFX").gameObject.SetActive(false);
            transform.Find("VFX").Find("TeamUI").gameObject.SetActive(false);
        }
        //GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        //When character dies
        /*if (characterPortrait != null)
        {
            characterPortrait.transform.GetChild(0).GetComponent<Image>().color = Color.grey;
        }*/
        if(_playerInformationData.ClassName == "MERCHANT")  // vietoj playerInformationData.ClassName buvo ClassName
        {
            gameInformation.Events.Add("MerchantDied");
        }
        if (TeamManager != null)
        {
            TeamManager.GetComponent<TeamInformation>().ModifyList();
        }
        if (GameObject.Find("GameInformation").GetComponent<PlayerTeams>().IsGameOver())
        {
            if (Respawn && GameObject.Find("GameInformation").GetComponent<AIManager>().RespawnEnemyWaves && GameObject.Find("GameInformation").GetComponent<AIManager>().RespawnCount > 0)
            {
                gameInformation.respawnEnemiesDuringThisTurn = true;
            }
            else
                gameInformation.GameOver();
        }
        //gameObject.SetActive(false);
    }

    public void Die()
    {
        transform.Find("CharacterModel").GetComponent<SpriteRenderer>().enabled = false;
        transform.Find("CharacterModel").GetComponent<Animator>().enabled = false;

        transform.Find("VFX")?.gameObject.SetActive(false);
        if (isThisObject)
        {
            Destroy(gameObject);
        }
        //gameObject.SetActive(false);
    }
    private void SendKillMessage()
    {
        string killMessage = gameObject.name + " was killed by:";
        foreach (GameObject x in KillerList)
        {
            if (x!= null && x.GetComponent<PlayerInformation>() != null)//??
            {
                killMessage += x.name + ", ";
                if (!x.GetComponent<PlayerInformation>().KillList.Contains(gameObject))
                {
                    x.GetComponent<PlayerInformation>().KillList.Add(gameObject);
                }
            }
        }
        //Debug.Log(killMessage);
    }
    public void AddKillXP()
    {
        if (!isThisObject)
        {
            XPToGain = 0;
            foreach (GameObject x in KillList)
            {
                _data.statistics.killCountByClass[Statistics.GetClassIndex(_playerInformationData.ClassName)]++; // vietoj playerInformationData.ClassName buvo ClassName
                _data.globalStatistics.killCountByClass[Statistics.GetClassIndex(_playerInformationData.ClassName)]++; // vietoj playerInformationData.ClassName buvo ClassName
                XPToGain += 50;
            }
        }
    }
    public void Heal(int healAmount, bool crit)
    {
        //if (health + healAmount >= MaxHealth)
        if (health + healAmount >= _playerInformationData.MaxHealth)
        {
            if (transform.Find("DamageTexts").transform.Find("DamageText1").gameObject.activeSelf)
            {
                if (transform.Find("DamageTexts").transform.Find("DamageText2").gameObject.activeSelf)
                {
                    //EnableDamageText(3, MaxHealth - health, crit, true);
                    EnableDamageText(3, _playerInformationData.MaxHealth - health, crit, true);
                }
                //else EnableDamageText(2, MaxHealth - health, crit, true);
                else EnableDamageText(2, _playerInformationData.MaxHealth - health, crit, true);
            }
            //else EnableDamageText(1, MaxHealth - health, crit, true);
            else EnableDamageText(1, _playerInformationData.MaxHealth - health, crit, true);
            //health = MaxHealth;
            health = _playerInformationData.MaxHealth;
        }
        else
        {
            if (transform.Find("DamageTexts").transform.Find("DamageText1").gameObject.activeSelf)
            {
                if (transform.Find("DamageTexts").transform.Find("DamageText2").gameObject.activeSelf)
                {
                    EnableDamageText(3, healAmount, crit, true);
                }
                else EnableDamageText(2, healAmount, crit, true);
            }
            else EnableDamageText(1, healAmount, crit, true);
            health += healAmount;
        }
        // health = MaxHealth;
        transform.Find("VFX").Find("VFX1x1").GetComponent<Animator>().SetTrigger("heal");
    }
    public void ApplyDebuff(string debuff, GameObject DebuffApplier = null)
    {
        if (gameObject.transform.CompareTag("Player"))
        {
            if (BlockingAlly != null)
            {
                BlockingAlly.GetComponent<PlayerInformation>().ApplyDebuff(debuff, DebuffApplier);
            }
            else
            {
                GameObject target = gameObject;
                if (debuff == "IceSlow" || debuff == "OilSlow" || debuff == "Stun")
                {
                    //GetComponent<GridMovement>().ApplyDebuff(debuff, DebuffApplier);
                    gridMovement.ApplyDebuff(debuff,DebuffApplier);
                    
                }
                else if (debuff == "CantMove")
                {
                    CantMove = true;
                }
                else if (debuff == "Disarmed")
                {
                    Disarmed = true;
                }
                else if ((debuff == "MindControl" || debuff == "Silenced") && !Silenced)
                {
                    if (debuff == "MindControl")
                    {
                        MindControlled = true;
                       // DebuffApplier.GetComponent<ActionManager>().FindActionByName("MindControl").SpecificAbilityAction(gameObject);
                    }
                    Silenced = true;
                }
            }

        }
    }
    public void ToggleSelectionBorder(bool state)
    {
        if (transform.Find("SelectedUI").gameObject != null)
        {
            transform.Find("SelectedUI").gameObject.SetActive(state);
        }
        if (state == false)
        {
            currentState = "Movement";
        }
    }
    public void PlayerSetup()
    {
        GameObject GameInformation;
        GameInformation = GameObject.Find("GameInformation");
        Color TeamUIColor = ColorStorage.TeamColor(CharactersTeam);
        if (CharactersTeam == "Default")
        {
            GameInformation.gameObject.GetComponent<PlayerTeams>().SpawnDefaultCharacter(gameObject);
        }
        else if (transform.Find("VFX") != null)
        {
            transform.Find("VFX").Find("TeamUI").GetComponent<SpriteRenderer>().color = TeamUIColor;
        }
    }
    public void LoadPlayerProgression()
    {
        if (savedCharacter != null && savedCharacter.prefab != null)
        {
            /*foreach(string x in savedCharacter.blessings)
            {
                BlessingsAndCurses.Add(x);
            }*/
           // BlessingsAndCurses = savedCharacter.blessings;
           _playerInformationData.BlessingsAndCurses = savedCharacter.blessings;
            //max hp
            //MaxHealth += (savedCharacter.level - 1) * 2;
            _playerInformationData.MaxHealth += (savedCharacter.level - 1) * 2;
            //Healthy
            //if (BlessingsAndCurses.Find(x => x.blessingName == "Healthy") != null)
            if (_playerInformationData.BlessingsAndCurses.Find(x => x.blessingName == "Healthy") != null)
            {
                //MaxHealth += 3;
                _playerInformationData.MaxHealth += 3;
            }
            //Head start
            /* if (BlessingsAndCurses.Contains("Head start"))
             {
                 GetComponent<GridMovement>().MovementPoints+=2;
             }*/
            //Sharp blade/Far reach
            //GetComponent<ActionManager>().ActivateBlessingBuffs();
            actionManager.ActivateBlessingBuffs();
        }
    }
   /*   public void EnableDamageText(int textIndex, int damageOrHealAmount, bool crit, bool heal, string specialColor = "")
    {
        GameObject damageText = null;
        char sign = '-';
        if (heal) sign = '+';
        switch (textIndex)
        {
            case 1:
                //damageText = transform.Find("DamageTexts").transform.Find("DamageText1").gameObject;
                break;
            case 2:
                damageText = transform.Find("DamageTexts").transform.Find("DamageText2").gameObject;
                break;
            case 3:
                damageText = transform.Find("DamageTexts").transform.Find("DamageText3").gameObject;
                break;
        }
        string textToDisplay;
        if (damageOrHealAmount == -1)
        {
            damageText.GetComponent<TextMeshPro>().color = Color.white;
            //textMeshPro.color=Color.white;
        }
        else if (!heal)
        {
            //damageText.GetComponent<TextMeshPro>().color = Color.red;
            if (specialColor == "Protected")
            {
                damageText.GetComponent<TextMeshPro>().color = new Color(221 / 255f, 193 / 255f, 193 / 255f);
            }
            if (specialColor == "Poison")
            {
                damageText.GetComponent<TextMeshPro>().color = new Color(0 / 255f, 255 / 255f, 74 / 255f);
            }
            if (specialColor == "PinkWeakSpot")
            {
                damageText.GetComponent<TextMeshPro>().color = new Color(255 / 255f, 145 / 255f, 191 / 255f);
            }
        }
        else damageText.GetComponent<TextMeshPro>().color = Color.green;
        //
        if (crit && damageOrHealAmount != -1)
        {
            textToDisplay = " Crit!\n" + sign + damageOrHealAmount;
        }
        else
        {
            textToDisplay = sign + damageOrHealAmount.ToString();
        }
        //
        if (damageOrHealAmount == -1)
        {
            textToDisplay = "Dodge!";
        }
        else
        {
            if (crit && damageOrHealAmount != -1)
            {
                textToDisplay = " Crit!\n" + sign + damageOrHealAmount.ToString();
            }
            else
            {
                textToDisplay = sign + damageOrHealAmount.ToString();
            }
        }
        damageText.GetComponent<DamageText>().damageBeingDealt = damageOrHealAmount;
        damageText.SetActive(true);
        damageText.GetComponent<TextMeshPro>().text = textToDisplay;
        damageText.GetComponent<DamageText>().time = 1;
      }
   */
   public void EnableDamageText(int textIndex, int damageOrHealAmount, bool crit, bool heal, string specialColor = "")
   {
       char sign = '-';
       if (heal) sign = '+';
       {
           TextMeshProUGUI damageText = damageTextTest[textIndex];
       }
       string textToDisplay;
       if (damageOrHealAmount == -1)
       {
           damageTextTest[textIndex].color=Color.white;
       }
       else if (!heal)
       {
           //damageText.GetComponent<TextMeshPro>().color = Color.red;
           if (specialColor == "Protected")
           {
               damageTextTest[textIndex].color = new Color(221 / 255f, 193 / 255f, 193 / 255f);
           }
           if (specialColor == "Poison")
           {
               damageTextTest[textIndex].color = new Color(0 / 255f, 255 / 255f, 74 / 255f);
           }
           if (specialColor == "PinkWeakSpot")
           {
               damageTextTest[textIndex].color = new Color(255 / 255f, 145 / 255f, 191 / 255f);
           }
       }
       else
           damageTextTest[textIndex].color= Color.green;
       //
       if (crit && damageOrHealAmount != -1)
       {
           textToDisplay = " Crit!\n" + sign + damageOrHealAmount;
       }
       else
       {
           textToDisplay = sign + damageOrHealAmount.ToString();
       }
       //
       if (damageOrHealAmount == -1)
       {
           textToDisplay = "Dodge!";
       }
       else
       {
           if (crit && damageOrHealAmount != -1)
           {
               textToDisplay = " Crit!\n" + sign + damageOrHealAmount.ToString();
           }
           else
           {
               textToDisplay = sign + damageOrHealAmount.ToString();
           }
       }
       damageTextas.damageBeingDealt = damageOrHealAmount; 
       //damageTextTest[textIndex].SetActive(true);
       damageTextTest[textIndex].text = textToDisplay;
       damageTextas.time = 1;
       
   }
    public void OnTurnStart()
    {
        turnCounter++;
        //if (BlessingsAndCurses.Find(x => x.blessingName == "Swiftness") != null && turnCounter % 2 == 0)
        if (_playerInformationData.BlessingsAndCurses.Find(x => x.blessingName == "Swiftness") != null && turnCounter % 2 == 0)
        {
            //GetComponent<GridMovement>().AvailableMovementPoints++;
            gridMovement.AvailableMovementPoints++;
        }

        //Poison
        int poisonDamage = 0;

        foreach (Poison x in Poisons)
        {
            x.turnsLeft--;
        }
        poisonDamage = TotalPoisonDamage();
        Poisons.RemoveAll(x => x.turnsLeft <= 0);
        if (poisonDamage > 0 && health > 0)
        {
            //if (BlessingsAndCurses.Find(x => x.blessingName == "Antitoxic") != null)
            if (_playerInformationData.BlessingsAndCurses.Find(x => x.blessingName == "Antitoxic") != null)
            {
                poisonDamage = 0;
            }
            DealDamage(poisonDamage, false, gameObject, "Poison");
        }
        //Barrier
        StartCoroutine(ExecuteAfterTime(0.5f, () =>
        {
            if (BarrierProvider != null)
            {
                BarrierProvider = null;
                transform.Find("VFX").Find("VFXBool").GetComponent<Animator>().SetTrigger("shieldEnd");
            }
        }));
    }
    public void OnTurnEnd()
    {
        /*
        if (BlessingsAndCurses.Contains("Head start") && turnCounter == 1)//buggy?
        {
            GetComponent<GridMovement>().MovementPoints -= 2;
            GetComponent<GridMovement>().AvailableMovementPoints -= 2;
        }
        */
        Silenced = false;
        MindControlled = false;
        if (Aflame != null)
        {
            Aflame.GetComponent<ActionManager>().FindActionByName("Attack").TriggerAflame(gameObject);
            
        }
        if (Debuffs.Contains("EnvHazardDamageBoost"))
        {
           // GetComponent<PlayerAttack>().minAttackDamage -= 3;
           // GetComponent<PlayerAttack>().maxAttackDamage -= 3;
           playerAttack.minAttackDamage -= 3;
           playerAttack.maxAttackDamage -= 3;
            Debuffs.Remove("EnvHazardDamageBoost");
        }
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
