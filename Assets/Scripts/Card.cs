using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

    public int cost;
    public int durability;
    public int durabilityCurrent;
    public int repair;
    public int repairCurrent;

    


    public enum ProtectionType
    {
        Alpha,
        Beta,
        Gamma
    }

    public ProtectionType[] protectionTypes;

    public bool wasPlayed = false;

    GameManager gm;
    CardDisplay cardDisplay;
    CardMovementHandler CardMovementHandler;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        CardMovementHandler = GetComponentInParent<CardMovementHandler>();
        //cardProtectionType = (ProtectionType)Random.Range(0, 3);
        cardDisplay = GetComponentInParent<CardDisplay>();
        Debug.Log("Ich koste: " + cardDisplay.card.cost + " Energy.");
        cost = cardDisplay.card.cost;
        durability = cardDisplay.card.durability;
        durabilityCurrent = durability;
        repair = cardDisplay.card.repair;
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
        if (durabilityCurrent < 0)
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
    }




 
}
