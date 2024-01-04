using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public SO_Card cardInfo;
    public GameManager gm;
    CardDisplay cardDisplay;
    public CardMovementHandler cardMovementHandler;

    private int tankedRadiation;

    //Events
    public event Action<Card> OnDurabilityZero;
    public static event Action FirstCardPlayedEvent;


    //CardInfo
    public string cardName;
    public List<GameConstants.cardRarity> cardRarity;
    public List<GameConstants.radiationTypes> protectionTypes;
    public int cost;
    public int durability;
    public int durabilityCurrent;
    public int durabilityPendantBuff = 0;
    public bool wasPlayed = false;
    public bool isBought;
    public string cardDescription;
    public bool energyCostAffected;
    public int energyCostIncrease;

    //Abilities
    public bool ability;
    public int duration;
    public List<GameConstants.abilityTargets> abilityTypes;


    //Effects
    public bool effect;
    public bool onBruch;
    public bool onPlay;
    public Dictionary<GameConstants.effectTypes, int> cardEffects = new Dictionary<GameConstants.effectTypes, int>();


    //Immunities
    public bool immunity;
    public List<GameConstants.radiationTypes> immunityTypes;


    //Specials
    public bool entsorgen;

    //Shop
    public int currencyCost;

    //Upgrade
    public bool upgraded;
    public Dictionary<GameConstants.cardUpgrades, int> cardUpgrade = new Dictionary<GameConstants.cardUpgrades, int>();
    public string upgradedCardDescription;
    private bool fml;

    //Additional Card Info
    public string sienceInfo;
    private void Awake()
    {

    }

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        cardMovementHandler = GetComponent<CardMovementHandler>();
        cardDisplay = GetComponent<CardDisplay>();

        cardName = cardInfo.name;
        gameObject.name = cardName;
        cardRarity = cardInfo.cardRarity;
        protectionTypes = cardInfo.protectionTypes;
        cost = cardInfo.cost + energyCostIncrease;
        energyCostAffected = cardInfo.energyCostAffected;
        energyCostIncrease = cardInfo.energyCostIncrease;


        durability = cardInfo.durability + durabilityPendantBuff;
        durabilityCurrent = durability;
        cardDescription = cardInfo.description;
        upgradedCardDescription = cardInfo.upgradedDescription;

        ability = cardInfo.ability;
        duration = cardInfo.duration;
        abilityTypes = cardInfo.abilityTypes;

        effect = cardInfo.effect;
        onBruch = cardInfo.onBruch;
        onPlay = cardInfo.onPlay;

        sienceInfo = cardInfo.cardInfo;
        currencyCost = cardInfo.currencyCost;

        immunity = cardInfo.immunity;
        immunityTypes = cardInfo.immunityTypes;
        entsorgen = cardInfo.entsorgen;

        upgraded = cardInfo.upgraded;

        //fill cardEffects Dictionary
        for (int i = 0; i < cardInfo.effectTypes.Count; i++)
        {
            cardEffects.Add(cardInfo.effectTypes[i], cardInfo.effectValues[i]);
        }

        //fill cardUpgrade Dictionary
        for (int i = 0; i < cardInfo.cardUpgrades.Count; i++)
        {
            cardUpgrade.Add(cardInfo.cardUpgrades[i], cardInfo.upgradeValues[i]);
        }
        if (upgraded)
        {
            UpgradeCard();
        }

        if (cardInfo.cardType.Contains(GameConstants.cardType.Schild))
        {
            cardDisplay.ActivateShieldIcon(true);
        }
        if (cardInfo.entsorgen)
        {
            cardDisplay.ActivateEntsorgenIcon(true);
        }
        if (energyCostAffected)
        {
            HandleCardEnergyCostEffect(energyCostIncrease);
        }
        SetPendantDurabilityBuff(1);
        cardDisplay.SetupDisplay();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnEnable()
    {
        GameManager.CardEnergyCostEffect += HandleCardEnergyCostEffect;
    }

    private void OnDisable()
    {
        GameManager.CardEnergyCostEffect -= HandleCardEnergyCostEffect;
    }

    public void HandleCardEnergyCostEffect(int value)
    {
        energyCostIncrease = value;
        UpdateEnergyCost();
    }

    private void UpdateEnergyCost()
    {
        cost = cardInfo.cost;
        energyCostIncrease = cardInfo.energyCostIncrease;
        if (cost + energyCostIncrease < 0)
        {
            cost = 0;
        }
        else
        {
            cost = cost + energyCostIncrease;
        }
        UpdateDisplay();
    }
    public void ShieldDebuff()
    {
        durability -= 2;
        if (durability < durabilityCurrent)
        {
            durabilityCurrent = durability;
            UpdateDisplay();
        }
    }
    
    public void UpdateDisplay()
    {
        cardDisplay.UpdateDisplay();
    }

    public int AdjustDurability(int value)
    {
        tankedRadiation = Math.Min(value, durabilityCurrent);
        
        durabilityCurrent -= value;
        UpdateDisplay();
        
        if (durabilityCurrent <= 0)
        {
            OnDurabilityZero?.Invoke(this);
            cardMovementHandler.MoveToDiscardPile();
        }
        
        return durabilityCurrent;

    }
    public int GammaReduction(int percentReduction)
    {
        int throughPut = Mathf.FloorToInt((tankedRadiation * percentReduction) / 100);
        Debug.Log("Gamma Reduction triggered. 25% of Gamma goes through: " + throughPut);
        return throughPut;
    }

    public void SetCurrentDurabilityToMax()
    {
        durabilityCurrent = durability;
        UpdateDisplay();
    }

    public void BackInPlay(Transform newParent)
    {
        cardMovementHandler.SetNewParent(newParent);
        SetCurrentDurabilityToMax();
        cardDisplay.UpdateDisplay();
        OnDurabilityZero = null;
    }

    public void SetWasPlayed(bool b)
    {
        wasPlayed = b;
        cardMovementHandler.wasPlayed = b;
    }

    public void SetActive(bool b)
    {
        gameObject.SetActive(b);
    }

    public void BruchBuff()
    {
        this.durabilityCurrent += 2;
        UpdateDisplay();
        Debug.Log(gameObject.name + " got buffed, new durability: " + durabilityCurrent);
    }

    public void SetImmunity(bool b, List<GameConstants.radiationTypes> radiationTypes)
    {
        immunity = b;
        if (radiationTypes != null)
        {
            for (int i = 0; i < radiationTypes.Count; i++)
            {
                immunityTypes.Add(radiationTypes[i]);
            }
        }
        else
        {
            immunityTypes.Clear();
            immunity = b;
        }
    }

    public void UpgradeCard()
    {
        upgraded = true;
        Debug.Log("Original Text: " + cardDescription);
        Debug.Log("Upgraded Text: " + upgradedCardDescription);
        cardDescription = upgradedCardDescription;
        foreach (var entry in cardUpgrade)
        {
            switch (entry.Key)
            {
                case GameConstants.cardUpgrades.EnergyCost:
                    cost -= entry.Value;
                    break;

                case GameConstants.cardUpgrades.Schild:
                    durability += entry.Value;
                    break;

                case GameConstants.cardUpgrades.Duration:
                    duration += entry.Value;
                    break;

                case GameConstants.cardUpgrades.Effect:
                    HandleCardEffectUpgrade(entry.Value);
                    break;

                default:
                    Debug.LogWarning(gameObject.name + " has an upgrade we do not handle");
                    break;

            }
            
        }
        SetCurrentDurabilityToMax();
        UpdateDisplay();
    }

    private void HandleCardEffectUpgrade(int value)
    {
        foreach (var entry in cardEffects)
        {
            switch (entry.Key)
            {
                case GameConstants.effectTypes.EnergyGet:
                    cardEffects[entry.Key] += value;
                    break;

                case GameConstants.effectTypes.DrawCard:
                    cardEffects[entry.Key] += value;
                    break;

                case GameConstants.effectTypes.HealthDamageReductionPercent:
                    cardEffects[entry.Key] += value;
                    break;

                case GameConstants.effectTypes.RadiationReductionFlat:
                    cardEffects[entry.Key] += value;
                    break;

                case GameConstants.effectTypes.TimerReductionFlat:
                    cardEffects[entry.Key] += value;
                    break;

                case GameConstants.effectTypes.ShieldBuff:
                    cardEffects[entry.Key] += value;
                    break;

                default:
                    Debug.LogWarning(gameObject.name + " has an effect we do not handle");
                    break;

            }
        }
        UpdateDisplay();
    }

    public void SetPendantDurabilityBuff(int value)
    {
        if (!ability)
        {
            if (gm.bleiBuffPendantActive && protectionTypes.Contains(GameConstants.radiationTypes.Gamma) && protectionTypes.Contains(GameConstants.radiationTypes.Beta) && protectionTypes.Contains(GameConstants.radiationTypes.Alpha))
            {
                durabilityPendantBuff = value;
                durability = durability + durabilityPendantBuff;
                durabilityCurrent = durability;
            }
            else if (gm.aluBuffPendantActive && !protectionTypes.Contains(GameConstants.radiationTypes.Gamma) && protectionTypes.Contains(GameConstants.radiationTypes.Beta) && protectionTypes.Contains(GameConstants.radiationTypes.Alpha))
            {
                durabilityPendantBuff = value;
                durability = durability + durabilityPendantBuff;
                durabilityCurrent = durability;
            }
            else if (gm.paperBuffPendantActive && !protectionTypes.Contains(GameConstants.radiationTypes.Gamma) && !protectionTypes.Contains(GameConstants.radiationTypes.Beta) && protectionTypes.Contains(GameConstants.radiationTypes.Alpha))
            {
                durabilityPendantBuff = value;
                durability = durability + durabilityPendantBuff;
                durabilityCurrent = durability;
            }
        }

    }

}
