using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public SO_Card cardInfo;
    GameManager gm;
    CardDisplay cardDisplay;
    CardMovementHandler CardMovementHandler;

    private int tankedRadiation;

    //Events
    public event Action<Card> OnDurabilityZero;


    //CardInfo
    public string cardName;
    public List<GameConstants.cardRarity> cardRarity;
    public List<GameConstants.radiationTypes> protectionTypes;
    public int cost;
    public int durability;
    public int durabilityCurrent;
    public bool wasPlayed = false;
    public bool isBought;

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

    //Additional Card Info
    public string sienceInfo;
    private void Awake()
    {
        

    }

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        CardMovementHandler = GetComponentInParent<CardMovementHandler>();
        cardDisplay = GetComponentInParent<CardDisplay>();

        cardName = cardInfo.name;
        gameObject.name = cardName;
        cardRarity = cardInfo.cardRarity;
        protectionTypes = cardInfo.protectionTypes;
        cost = cardInfo.cost;
        durability = cardInfo.durability;
        durabilityCurrent = durability;

        ability = cardInfo.ability;
        duration = cardInfo.duration;
        abilityTypes = cardInfo.abilityTypes;

        effect = cardInfo.effect;
        onBruch = cardInfo.onBruch;
        onPlay = cardInfo.onPlay;
        
        //fill cardEffects Dictionary
        for (int i = 0; i < cardInfo.effectTypes.Count; i++)
        {
            cardEffects.Add(cardInfo.effectTypes[i], cardInfo.effectValues[i]);
        }

        immunity = cardInfo.immunity;
        immunityTypes = cardInfo.immunityTypes;
        entsorgen = cardInfo.entsorgen;

        //fill cardUpgrade Dictionary
        upgraded = cardInfo.upgraded;
        for (int i = 0; i < cardInfo.cardUpgrades.Count; i++)
        {
            cardUpgrade.Add(cardInfo.cardUpgrades[i], cardInfo.upgradeValues[i]);
        }
        if (upgraded)
        {
            UpgradeCard();
        }

        currencyCost = cardInfo.currencyCost;

        if (cardInfo.cardType.Contains(GameConstants.cardType.Schild))
        {
            cardDisplay.ActivateShieldIcon(true);
        }
        if (cardInfo.entsorgen)
        {
            cardDisplay.ActivateEntsorgenIcon(true);
        }


        sienceInfo = cardInfo.cardInfo;

        cardDisplay.UpdateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        cardDisplay.durabilityText.text = durabilityCurrent.ToString();
    }

    public int AdjustDurability(int value)
    {
        tankedRadiation = Math.Min(value, durabilityCurrent);
        
        durabilityCurrent -= value;
        UpdateDisplay();
        
        if (durabilityCurrent <= 0)
        {
            OnDurabilityZero?.Invoke(this);
            CardMovementHandler.MoveToDiscardPile();
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
        CardMovementHandler.SetNewParent(newParent);
        SetCurrentDurabilityToMax();
        cardDisplay.UpdateDisplay();
        OnDurabilityZero = null;
    }

    public void SetWasPlayed(bool b)
    {
        wasPlayed = b;
        CardMovementHandler.wasPlayed = b;
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

    private void OnDestroy()
    {
        //gm.discardPile.Remove(this);
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
            UpdateDisplay();
        }
        
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

}
