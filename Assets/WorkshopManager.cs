using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopManager : MonoBehaviour
{
    
    public PlayerHealthManager playerHealth;
    public Deck deck;
    List<GameObject> instantiatedCards;
    public int amount;
    public GameObject menu;
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Wallet").GetComponentInChildren<PlayerHealthManager>();
        deck = GameObject.FindGameObjectWithTag("Wallet").GetComponentInChildren<Deck>();

    }

    public void UpgradeCards()
    {
        menu.SetActive(false);
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
        }
    }

    public void RemoveCards()
    {
        menu.SetActive(false);
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
        }
    }

    public void RefillHealth()
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
