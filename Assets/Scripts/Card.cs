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

    public void Buff()
    {
        this.durabilityCurrent += 2;
        UpdateDisplay();
        Debug.Log(gameObject.name + " got buffed, new durability: " + durabilityCurrent);
    }

    private void OnDestroy()
    {
        gm.discardPile.Remove(this);
    }

}
