using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Deck : MonoBehaviour
{
    public List<Card> deck;
    public List<Card> playerDeck;

    void Awake()
    {
        PopulatePlayerDeck();
    }

    void Start()
    {
        
    }

    void PopulatePlayerDeck()
    {
        foreach (Card card in deck)
        {
            Card addedCard = Instantiate(card, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            addedCard.SetActive(false);
            playerDeck.Add(addedCard);
            Debug.Log("Added Card: " + addedCard);
        }
    }

    public Card Draw()
    {
        Debug.Log("Deck.Draw got called");
        return playerDeck[Random.Range(0, playerDeck.Count)];
    }

    public void RemoveCard(Card card)
    {
        playerDeck.Remove(card);
    }

    public void AddCard(Card card)
    {
        playerDeck.Add(card);
    }
}
