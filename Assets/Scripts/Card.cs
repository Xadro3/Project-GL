using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public event Action<Card> OnDurabilityZero;
    private SO_Card cardInfo;

    public string cardName;
    public int cost;
    public int durability;
    public int durabilityCurrent;
    public int repair;
    public int repairCurrent;
    public bool ability;
    public int duration;
    public bool effect;
    public bool entsorgen;
    public List<GameConstants.effectTypes> effectTypes;
    public List<GameConstants.radiationTypes> protectionTypes;
    public List<GameConstants.cardTypes> cardTypes;

    public bool wasPlayed = false;

    GameManager gm;
    CardDisplay cardDisplay;
    CardMovementHandler CardMovementHandler;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        CardMovementHandler = GetComponentInParent<CardMovementHandler>();
        cardDisplay = GetComponentInParent<CardDisplay>();
        cardInfo = cardDisplay.card;
        effectTypes = cardInfo.effectTypes;
        effect = cardInfo.effect;
        duration = cardInfo.duration;
        cost = cardInfo.cost;
        durability = cardInfo.durability;
        durabilityCurrent = durability;
        repair = cardInfo.repair;
        protectionTypes = cardInfo.protectionTypes;
        entsorgen = cardInfo.entsorgen;
        cardTypes = cardInfo.cardArchetypes;

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
