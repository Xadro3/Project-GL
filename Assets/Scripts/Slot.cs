using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Slot : MonoBehaviour
{
    public static event System.Action ShieldRepairEvent;
    public static event System.Action ShieldBuffEvent;


    public bool hasCard = false;
    GameManager gm;
    public Card currentCard = null;
    public SpriteRenderer cardSlotSprite;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnEnable()
    {
        CardMovementHandler.OnShieldEffect += HandleShieldEffect;
    }
    private void OnDisable()
    {
        CardMovementHandler.OnShieldEffect -= HandleShieldEffect;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene arg0)
    {

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

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
        if (hasCard)
        {
            currentCard = GetCardInSlotInfo();
            cardSlotSprite.enabled = false;
        }
        else
        {
            currentCard = null;
            cardSlotSprite.enabled = true;
        }
    }

    private void HandleShieldDissolve(int value)
    {
        GetCardInSlotInfo().AdjustDurability(currentCard.durabilityCurrent);
    }

    private void HandleShieldBuff(int value)
    {
        ShieldBuffEvent?.Invoke();
        GetCardInSlotInfo().AdjustDurability(-value);
    }

    private void HandleShieldRepair()
    {
        //currentCard.AdjustDurability(-(currentCard.durabilityCurrent/2));
        ShieldRepairEvent?.Invoke();
        Card cardToRepair = GetCardInSlotInfo();
        if (cardToRepair != null)
        {
            Debug.Log(cardToRepair.gameObject + " will get healed");
            cardToRepair.SetCurrentDurabilityToMax();
        }
        
    }

    public void HandleShieldMaxBuff()
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
                        if (card.cardName == "Upcycling")
                        {
                            Debug.Log("Upcycling played");
                            gm.HandleEffect(entry.Key, entry.Value);
                        }
                        else
                        {
                            HandleShieldMaxBuff();
                        }
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
                        gm.HandleEffect(entry.Key, entry.Value);
                        break;
                }
            }
        }
        
    }

    
}
