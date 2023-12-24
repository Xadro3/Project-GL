using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    
    
    public bool hasCard = false;
    GameManager gm;
    private Card currentCard = null;

    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        CardMovementHandler.OnEnemyEffect += HandleShieldEffect;
    }
    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        CardMovementHandler.OnEnemyEffect -= HandleShieldEffect;
    }

    private void HandleShieldEffect(Card card)
    {
        HandleShieldAbility(card);
    }

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
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
    }

    private void HandleShieldBuff(int value)
    {
        GetCardInSlotInfo().AdjustDurability(-value);
    }

    private void HandleShieldRepair(int value)
    {
        //currentCard.AdjustDurability(-(currentCard.durabilityCurrent/2));
        GetCardInSlotInfo().SetCurrentDurabilityToMax();
    }

    private void HandleShieldMaxBuff(int value)
    {
        GetCardInSlotInfo().AdjustDurability(-(GetCardInSlotInfo().durability));
        
    }

    public void HandleShieldAbility(Card card)
    {
        foreach (var entry in card.cardEffects)
        {
            switch (entry.Key)
            {
                case GameConstants.effectTypes.ShieldMaxBuff:
                    HandleShieldMaxBuff(entry.Value);
                    break;
                
                case GameConstants.effectTypes.ShieldBuff:
                    Debug.Log("Effect: " + entry.Key);
                    HandleShieldBuff(entry.Value);
                    break;

                case GameConstants.effectTypes.ShieldRepair:
                case GameConstants.effectTypes.ShieldRepairPapier:
                case GameConstants.effectTypes.ShieldRepairAlu:
                case GameConstants.effectTypes.ShieldRepairBlei:
                    Debug.Log("Effect: " + entry.Key);
                    HandleShieldRepair(entry.Value);
                    break;

                case GameConstants.effectTypes.ShieldDissolve:
                case GameConstants.effectTypes.ShieldDissolvePapier:
                case GameConstants.effectTypes.ShieldDissolveAlu:
                case GameConstants.effectTypes.ShieldDissolveBlei:
                    Debug.Log("Effect: " + entry.Key);
                    HandleShieldDissolve(entry.Value);
                    break;

                case GameConstants.effectTypes.DrawCard:
                case GameConstants.effectTypes.Discard:
                    Debug.Log("Effect: " + entry.Key);
                    gm.HandleEffect(card);
                    break;
            }
        }        
    }

    
}
