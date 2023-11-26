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

    private void OnDestroy()
    {
       
    }

    private void SetSortingOrder()
    {
        currentCard.GetComponent<CardMovementHandler>().SetSortingOrder(99);
    }

    private Card GetCardInSlotInfo()
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
    }
}
