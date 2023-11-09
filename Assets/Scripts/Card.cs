using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    // To-Do:
    // -Move card movement to CardDragHandler

    public int cost;
    public int durability;
    public int repair;

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
        durability -= damage;
        if (durability < 0)
        {
            CardMovementHandler.MoveToDiscardPile();
            return Mathf.Abs(durability);
        }
        else
        {
            return 0;
        }
    }




 
}
