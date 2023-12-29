using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    
    public bool hasCard = false;
    GameManager gm;
    public Card currentCard = null;

    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        CardMovementHandler.OnShieldEffect += HandleShieldEffect;
    }
    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        CardMovementHandler.OnShieldEffect -= HandleShieldEffect;
    }

    private void HandleShieldEffect(Card card, Slot slot)
    {
        HandleShieldAbility(card, slot);
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

    public bool IsCurrentCardAffected(Card card)
    {
        Card cardInSlot = GetCardInSlotInfo();
        return false;
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

    private void HandleShieldRepair()
    {
        //currentCard.AdjustDurability(-(currentCard.durabilityCurrent/2));
        Card cardToRepair = GetCardInSlotInfo();
        if (cardToRepair != null)
        {
            Debug.Log(cardToRepair.gameObject + " will get healed");
            cardToRepair.SetCurrentDurabilityToMax();
        }
        
    }

    private void HandleShieldMaxBuff(int value)
    {
        GetCardInSlotInfo().AdjustDurability(-(GetCardInSlotInfo().durability));
        
    }

    public void HandleShieldAbility(Card card, Slot slot)
    {
        if (slot == GetComponent<Slot>())
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
                        HandleShieldRepair();
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

    
}
