using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public bool hasCard;
    private void Start()
    {
        GameManager.OnAssignToSlot += HandleCardDropped;
    }

    private void OnDestroy()
    {
        GameManager.OnAssignToSlot -= HandleCardDropped;
    }

    private void HandleCardDropped(Card card, Slot slot)
    {
        
    }

    public void HasCard(bool hasCard)
    {
        this.hasCard = hasCard;
    }
}
