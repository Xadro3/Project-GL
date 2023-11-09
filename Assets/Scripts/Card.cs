using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    // To-Do:
    // -Move card movement to CardDragHandler

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

    CardMovementHandler CardMovementHandler;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        CardMovementHandler = GetComponentInParent<CardMovementHandler>();
        //cardProtectionType = (ProtectionType)Random.Range(0, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
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
