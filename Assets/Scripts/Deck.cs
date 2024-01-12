using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Deck : MonoBehaviour
{
    public static event System.Action<string> CardUpgradedEvent;

    public List<Card> deck;
    public List<Card> playerDeck;
    public List<GameObject> playerDeckObjects;

    private string cardNameRemove;
    private Card cardAdd;
    private Card cardUpgrade;

    public CardManager cardManager;

    public GameManager gameManager;

    void Awake()
    {
        CardMovementHandler.CardRewardChosenEvent += HandleCardRewardChosen;
    }


    private void HandleCardRewardChosen(Card chosenCard)
    {
        AddCardToBaseDeck(chosenCard);
    }

    void Start()
    {

    }

    

    private void OnEnable()
    {

    }

    public bool PopulatePlayerDeck()
    {
        playerDeck.Clear();
        foreach (Card card in deck)
        {
            Card addedCard = Instantiate(card, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            addedCard.SetActive(false);
            playerDeck.Add(addedCard);
            //Debug.Log("Added Card: " + addedCard);
        }
        return true;
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

    public void AddCardToBaseDeck(Card card)
    {
        cardManager.AddCardToBaseDeck(card.cardInfo);
    }

    public void RemoveCardFromBaseDeck(Card card)
    {
        cardManager.RemoveCardFromBaseDeck(card.cardInfo);
        Destroy(card.gameObject);
    }

    public void UpgradeCard(Card card)
    {
        cardNameRemove = card.cardName;
        cardManager.RemoveCardFromBaseDeck(card.cardInfo);
        deck.Remove(card);
        playerDeck.Remove(card);
        Destroy(card.gameObject);
        //Debug.Log("Destroyed!");
        cardManager.AddCardToBaseDeck(card.cardInfo.upgradedCardInfo);
        cardManager.AddBaseCardsToDeck();
        PopulatePlayerDeck();
        Debug.Log("UpgradedCard Fired!");
        CardUpgradedEvent?.Invoke(cardNameRemove);
    }

    public List<GameObject> GetPlayerDeck()
    {
        StartCoroutine(SetupPlayerDeck());
        return playerDeckObjects;
    }

    private IEnumerator SetupPlayerDeck()
    {
        playerDeckObjects.Clear();
        while (!cardManager.AddBaseCardsToDeck())
        {
            yield return null;
        }
        while (!PopulatePlayerDeck())
        {
            yield return null;
        }
        foreach (Card card in deck)
        {
            playerDeckObjects.Add(card.gameObject);
        }
        yield break;
    }
}
