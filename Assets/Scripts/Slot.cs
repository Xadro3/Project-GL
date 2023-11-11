using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public bool hasCard;

    private Card currentCard;

    private void Start()
    {
        
    }

    private void OnDestroy()
    {
       
    }

    private Card GetCardInSlotInfo()
    {
        return GetComponentInChildren<Card>();
    }

    private void HandleCardDropped(Card card, Slot slot)
    {
        
    }

    public void HasCard(bool hasCard)
    {
        this.hasCard = hasCard;
    }
}
