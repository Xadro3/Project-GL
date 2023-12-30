using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Deck : MonoBehaviour
{
    public List<Card> deck;
    public List<Card> playerDeck;
    public List<GameObject> playerDeckObjects;

    void Awake()
    {
        
    }

    void Start()
    {
        GetPlayerDeck();
    }

    private void OnEnable()
    {

    }

    public void PopulatePlayerDeck()
    {
        foreach (Card card in deck)
        {
            Card addedCard = Instantiate(card, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            addedCard.SetActive(false);
            playerDeck.Add(addedCard);
            //Debug.Log("Added Card: " + addedCard);
        }
    }

    public Card Draw()
    {
        //Debug.Log("Deck.Draw got called");
        //return playerDeck[Random.Range(0, playerDeck.Count)];

        // Check if the playerDeck is not null and not empty
        if (playerDeck != null && playerDeck.Count > 0)
        {
            // Ensure the random index is within valid bounds
            int randomIndex = Random.Range(0, playerDeck.Count);
            if (randomIndex >= 0 && randomIndex < playerDeck.Count)
            {
                // Return a random card from playerDeck
                Card randomCard = playerDeck[randomIndex];
                //Debug.Log("Deck gave: " + randomCard);
                return randomCard;
            }
            else
            {
                // Handle the case when the random index is out of bounds
                Debug.LogWarning("Random index is out of bounds!");
                return null; // or throw new InvalidOperationException("Random index is out of bounds!");
            }
        }
        else
        {
            // Handle the case when the playerDeck is null or empty
            Debug.LogWarning("Player deck is null or empty!");
            return null; // or throw new InvalidOperationException("Player deck is null or empty!");
        }
    }

    public void RemoveCardFromPlayerDeck(Card card)
    {
        playerDeck.Remove(card);
    }

    public void RemoveCardFromDeck(Card card)
    {
        deck.Remove(card);
    }

    public void AddCardToPlayerDeck(Card card)
    {
        playerDeck.Add(card);
    }

    public void AddCardToDeck(Card card)
    {
        deck.Add(card);
    }

    public List<GameObject> GetPlayerDeck()
    {
        playerDeckObjects.Clear();
        foreach (Card card in deck)
        {
            playerDeckObjects.Add(card.gameObject);
        }

        return playerDeckObjects;
    }
}
