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


    //Events
    public event Action<Card> OnDurabilityZero;


    //CardInfo
    public List<GameConstants.cardTypes> cardArchetypes;
    public List<GameConstants.radiationTypes> protectionTypes;
    public int cost;
    public int durability;
    public int durabilityCurrent;
    public int repair;
    public int repairCurrent;
    public bool wasPlayed = false;


    //Abilities
    public bool ability;
    public int duration;
    public List<GameConstants.abilityTargets> abilityTypes;


    //Effects
    public bool effect;
    public bool bruch;
    public List<int> effectValues;
    public List<GameConstants.effectTypes> effectTypes;


    //Immunities
    public bool immunity;
    public List<GameConstants.radiationTypes> immunityTypes;


    //Specials
    public bool entsorgen;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        CardMovementHandler = GetComponentInParent<CardMovementHandler>();
        cardDisplay = GetComponentInParent<CardDisplay>();
        
        cardArchetypes = cardInfo.cardArchetypes;
        protectionTypes = cardInfo.protectionTypes;
        cost = cardInfo.cost;
        durability = cardInfo.durability;
        durabilityCurrent = durability;
        repair = cardInfo.repair;
        repairCurrent = repair;

        ability = cardInfo.ability;
        duration = cardInfo.duration;
        abilityTypes = cardInfo.abilityTypes;

        effect = cardInfo.effect;
        bruch = cardInfo.bruch;
        effectValues = cardInfo.effectValues;
        effectTypes = cardInfo.effectTypes;

        immunity = cardInfo.immunity;
        immunityTypes = cardInfo.immunityTypes;
        entsorgen = cardInfo.entsorgen;

    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        for (int i = 0; i < effectTypes.Count; i++)
        {
            switch (effectTypes[i])
            {
                case GameConstants.effectTypes.DamageReductionFlat:
                    CardEffectEventHandler.DamageReductionFlat += HandleDamageReductionFlat;
                    //Debug.Log(gameObject.name + "- I am subscribed to: " + effectTypes[i]);
                    break;

                case GameConstants.effectTypes.DamageReductionPercent:
                    CardEffectEventHandler.DamageReductionPercent += HandleDamageReductionPercent;
                    break;

                case GameConstants.effectTypes.EnergyCostReduction:
                    CardEffectEventHandler.EnergyCostReduction += HandleEnergyCostReduction;
                    break;

                case GameConstants.effectTypes.Discard:
                    CardEffectEventHandler.Discard += HandleDiscard;
                    break;
            }
        }
    }

    private void OnDisable()
    {
        CardEffectEventHandler.DamageReductionFlat -= HandleDamageReductionFlat;
        CardEffectEventHandler.DamageReductionPercent -= HandleDamageReductionPercent;
        CardEffectEventHandler.EnergyCostReduction -= HandleEnergyCostReduction;
        CardEffectEventHandler.Discard -= HandleDiscard;
    }

    public void UpdateDisplay()
    {
        cardDisplay.durabilityText.text = durabilityCurrent.ToString();
    }

    public int AdjustDurability(int damage)
    {
        durabilityCurrent -= damage;
        UpdateDisplay();
        
        if (durabilityCurrent <= 0)
        {
            OnDurabilityZero?.Invoke(this);
            CardMovementHandler.MoveToDiscardPile();
        }

        return durabilityCurrent;

    }

    public void BackInPlay(Transform newParent)
    {
        CardMovementHandler.SetNewParent(newParent);
        durabilityCurrent = durability;
        repairCurrent = repair;
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
        gm.discardPile.Remove(this);
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
    
    //Effect handling
    private void HandleDiscard(int value)
    {
        if (wasPlayed)
        {
            Debug.Log("Card: HandleDiscard triggered. Handle it!");
        }
    }

    private void HandleEnergyCostReduction(int value)
    {
        if (wasPlayed)
        {
            Debug.Log("Card:  HandleEnergyCostReduction triggered. Handle it!");
        }
    }

    private void HandleDamageReductionPercent(int value)
    {
        if (wasPlayed)
        {
            Debug.Log("Card: HandleDamageReductionPercent triggered. Handle it!");
        }
    }

    private void HandleDamageReductionFlat(int value)
    {
        if (wasPlayed)
        {
            Debug.Log("Card: HandleDamageReductionFlat triggered. Handle it!");
        }
    }

}
