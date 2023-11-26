using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

    public string cardName;
    public int cost;
    public int durability;
    public int durabilityCurrent;
    public int repair;
    public int repairCurrent;

    public List<GameConstants.radiationTypes> protectionTypes;

    public bool wasPlayed = false;

    GameManager gm;
    CardDisplay cardDisplay;
    CardMovementHandler CardMovementHandler;

    // Constructor
    public Card(string cardName, int cardCost, int cardDurability, int cardRepair)
    {
        this.cardName = cardName;
        this.cost = cardCost;
        this.durability = cardDurability;
        this.repair = cardRepair;
    }


    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        CardMovementHandler = GetComponentInParent<CardMovementHandler>();
        //cardProtectionType = (ProtectionType)Random.Range(0, 3);
        cardDisplay = GetComponentInParent<CardDisplay>();
        cost = cardDisplay.card.cost;
        durability = cardDisplay.card.durability;
        durabilityCurrent = durability;
        repair = cardDisplay.card.repair;
        protectionTypes = cardDisplay.card.protectionTypes;
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
            CardMovementHandler.MoveToDiscardPile();
            return Mathf.Abs(durabilityCurrent);
        }
        else
        {
            return 0;
        }
    }

    public void BackInPlay(Transform newParent)
    {
        CardMovementHandler.SetNewParent(newParent);
        durabilityCurrent = durability;
        repairCurrent = repair;
        cardDisplay.UpdateDisplay();
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

}
