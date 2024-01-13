using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopManager : MonoBehaviour
{
    public static event System.Action<GameObject> FoundUpgradedCardEvent;

    public PlayerHealthManager playerHealth;
    public Deck deck;
    List<GameObject> instantiatedCards;
    public int amount;
    public GameObject menu;
    public GameObject upgrades;
    public GameObject sell;
    ShopCurrency shop;
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Wallet").GetComponentInChildren<PlayerHealthManager>();
        deck = GameObject.FindGameObjectWithTag("Wallet").GetComponentInChildren<Deck>();
        shop = GameObject.FindGameObjectWithTag("Wallet").GetComponentInChildren<ShopCurrency>();
    }
    private void OnEnable()
    {
        Deck.CardUpgradedEvent += RefreshCardDisplay;
    }
    private void OnDisable()
    {
        Deck.CardUpgradedEvent -= RefreshCardDisplay;
    }

    public void UpgradeCards()
    {
        if (shop.RemoveMoney(35))
        {
            menu.SetActive(false);
            upgrades.SetActive(true);
            instantiatedCards = deck.GetPlayerDeck();
            //Debug.Log(instantiatedCard.name);
            foreach (GameObject instantiatedCard in instantiatedCards)
            {

                instantiatedCard.SetActive(true);
                instantiatedCard.transform.SetParent(transform);
                instantiatedCard.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                instantiatedCard.transform.localScale = new Vector3(4f, 4f, 4f);
                instantiatedCard.GetComponent<CardMovementHandler>().enabled = false;
                instantiatedCard.AddComponent<Drag>();
                instantiatedCard.layer = 0;
                instantiatedCard.GetComponent<CardDisplay>().ActivateCurrencyCostField(true);
            }
        }
    }

    public void RemoveCards()
    {
        if (shop.RemoveMoney(20))
        {
            menu.SetActive(false);
            sell.SetActive(true);
            instantiatedCards = deck.GetPlayerDeck();
            //Debug.Log(instantiatedCard.name);
            foreach (GameObject instantiatedCard in instantiatedCards)
            {
                instantiatedCard.SetActive(true);
                instantiatedCard.transform.SetParent(transform);
                instantiatedCard.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                instantiatedCard.transform.localScale = new Vector3(4f, 4f, 4f);
                instantiatedCard.GetComponent<CardMovementHandler>().enabled = false;
                instantiatedCard.AddComponent<Drag>();
                instantiatedCard.layer = 0;
                instantiatedCard.GetComponent<CardDisplay>().ActivateCurrencyCostField(true);
            }
        }
        
    }

    public void RefillHealth()
    {

        if (shop.RemoveMoney(30))
        {
            menu.SetActive(false);
            int remainingSpace = playerHealth.healthMax - playerHealth.health;

            // Check if the increase would overcap healthMax
            if (amount > remainingSpace)
            {
                // If so, set health to healthMax
                playerHealth.health = playerHealth.healthMax;
            }
            else
            {
                // Otherwise, increase health by the specified amount
                playerHealth.health += amount;
            }
        }
        

    }

    private void RefreshCardDisplay(string removedCardName)
    {
        Debug.Log("Refreshing Card Display");
        instantiatedCards.Clear();
        instantiatedCards = deck.GetPlayerDeck();
        List<GameObject> refreshedCards = new List<GameObject>();
        //Debug.Log(instantiatedCard.name);
        foreach (GameObject instantiatedCard in instantiatedCards)
        {
            instantiatedCard.SetActive(true);
            instantiatedCard.transform.SetParent(transform);
            instantiatedCard.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            instantiatedCard.transform.localScale = new Vector3(4f, 4f, 4f);
            instantiatedCard.GetComponent<CardMovementHandler>().enabled = false;
            instantiatedCard.AddComponent<Drag>();
            instantiatedCard.layer = 0;
            instantiatedCard.GetComponent<CardDisplay>().ActivateCurrencyCostField(true);
            refreshedCards.Add(instantiatedCard);
            Debug.Log("Added: " + instantiatedCard.GetComponent<Card>().cardInfo.name);
            Debug.Log(refreshedCards.Count);
        }
        foreach (GameObject refreshedCard in refreshedCards)
        {
            if (refreshedCard.GetComponent<Card>().cardInfo.name == removedCardName && refreshedCard.GetComponent<Card>().cardInfo.upgraded)
            {
                Debug.Log("Found Upgraded Card Fired!");
                FoundUpgradedCardEvent?.Invoke(refreshedCard);
                break;
            }
        }
    }

    
}
