using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public bool hasCard = false;
    GameManager gm;
    private Card currentCard = null;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {

    }

    private void OnDestroy()
    {
       
    }

    private void SetSortingOrder()
    {
        currentCard.GetComponent<CardMovementHandler>().SetSortingOrder(99);
    }

    public Card GetCardInSlotInfo()
    {
        currentCard = GetComponentInChildren<Card>();
        return currentCard;
    }

    private void HandleCardDropped(Card card, Slot slot)
    {
        
    }

    public void HasCard(bool hasCard)
    {
        this.hasCard = hasCard;
        if (!hasCard)
        {
            currentCard = null;
        }
    }

    private void HandleShieldDissolve(int value)
    {
        GetCardInSlotInfo().AdjustDurability(currentCard.durabilityCurrent);
        Debug.Log("Card: HandleShieldDissolve Event triggered. Handle it!");
    }

    private void HandleShieldBuff(int value)
    {
        GetCardInSlotInfo().AdjustDurability(-value);
        Debug.Log("Card: HandleShieldBuff triggered. Handle it!");
    }

    private void HandleShieldRepair(int value)
    {
        //currentCard.AdjustDurability(-(currentCard.durabilityCurrent/2));
        GetCardInSlotInfo().SetCurrentDurabilityToMax();
        Debug.Log("Card: HandleShieldRepair triggered. Handle it!");
    }
    private void HandleDrawCard(int value)
    {
        
    }

    public void HandleShieldAbility(Card abilityCard)
    {
        foreach (var entry in abilityCard.cardEffects)
        {
            switch (entry.Key)
            {
                case GameConstants.effectTypes.ShieldBuff:
                    HandleShieldBuff(entry.Value);
                    break;

                case GameConstants.effectTypes.ShieldRepair:
                    HandleShieldRepair(entry.Value);
                    break;

                case GameConstants.effectTypes.ShieldDissolve:
                    HandleShieldDissolve(entry.Value);
                    break;

                case GameConstants.effectTypes.DrawCard:
                    gm.HandleEffect(entry.Key,entry.Value);
                    break;

                case GameConstants.effectTypes.ShieldDissolvePapier:
                    HandleShieldDissolve(entry.Value);
                    break;

                case GameConstants.effectTypes.ShieldDissolveAlu:
                    HandleShieldDissolve(entry.Value);
                    break;

                case GameConstants.effectTypes.ShieldDissolveBlei:
                    HandleShieldDissolve(entry.Value);
                    break;
            }
        }        
    }

    
}
