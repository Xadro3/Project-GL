using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public bool hasCard = false;

    private Card currentCard = null;

    private void Start()
    {
        
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
        currentCard.AdjustDurability(currentCard.durabilityCurrent);
        Debug.Log("Card: HandleShieldDissolve Event triggered. Handle it!");
    }

    private void HandleShieldBuff(int value)
    {
        currentCard.AdjustDurability(-value);
        Debug.Log("Card: HandleShieldBuff triggered. Handle it!");
    }

    private void HandleShieldRepair(int value)
    {
        currentCard.AdjustDurability(-(currentCard.durabilityCurrent/2));
        Debug.Log("Card: HandleShieldRepair triggered. Handle it!");
    }

    private void HandleRadiationImmunity(bool b, List<GameConstants.radiationTypes> radiationTypes)
    {
        currentCard.SetImmunity(b, radiationTypes);
        Debug.Log("Card: HandleRadiationImmunity triggered. Handle it!");
    }

    public void HandleShieldAbility(Card abilityCard)
    {
        GetCardInSlotInfo();
        for (int i = 0; i < abilityCard.effectTypes.Count; i++)
        {
            switch (abilityCard.effectTypes[i])
            {
                case GameConstants.effectTypes.ShieldBuff:
                    HandleShieldBuff(abilityCard.effectValues[i]);
                    break;

                case GameConstants.effectTypes.ShieldRepair:
                    HandleShieldRepair(abilityCard.effectValues[i]);
                    break;

                case GameConstants.effectTypes.ShieldDissolve:
                    HandleShieldDissolve(abilityCard.effectValues[i]);
                    break;

                case GameConstants.effectTypes.RadiationImmunity:
                    HandleRadiationImmunity(abilityCard.immunity, abilityCard.immunityTypes);
                    break;
            }
        }
        
    }
}
