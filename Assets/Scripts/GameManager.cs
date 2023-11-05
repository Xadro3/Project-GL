using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<Card> deck;
    public int playerRessourceCurrent;
    public int playerRessourceMax;
    public TextMeshProUGUI playerRessourceText;

    public Transform[] activeCardSlots;
    private Transform activeCardSlot;
    public bool[] availableCardSlots;

    public Transform[] playerHandSlots;
    private Transform playerHandSlot;
    public bool[] availablePlayerHandSlots;

    public List<Card> discardPile;

    public List<Card> graveyardPile;

    public delegate void CardDroppedEventHandler(Card card, Slot activeCardSlot);
    public static event CardDroppedEventHandler OnAssignToSlot;

    public static void CardDropped(Card card, Slot activeCardSlot)
    {
        //AssignToSlot(card, activeCardSlot);
    }

    public static void AssignToActiveSlot(Card card, Slot activeCardSlot)
    {
        card.transform.SetParent(activeCardSlot.transform);
        activeCardSlot.HasCard(true);
        card.transform.position = activeCardSlot.transform.position;

    }

    public static void AssignToPlayerHand(Card card, Transform playerHandSlot, Slot activeCardSlot)
    {
        activeCardSlot.HasCard(false);
        card.transform.SetParent(playerHandSlot);
        card.transform.position = playerHandSlot.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        DrawCards();
    }

    // Update is called once per frame
    void Update()
    {
        playerRessourceText.text = playerRessourceCurrent.ToString();
        
    }

    public void DrawCards()
    {
        for (int i = 0; i < availablePlayerHandSlots.Length; i++)
        {
            Card randomCard = deck[Random.Range(0, deck.Count)];
            randomCard.gameObject.SetActive(true);
            randomCard.handIndex = i;
            randomCard.transform.position = playerHandSlots[i].position;
            randomCard.transform.SetParent(playerHandSlots[i].transform);
            randomCard.SetParent(playerHandSlots[i]);
            randomCard.initialDaddy = playerHandSlots[i].transform;
            randomCard.wasPlayed = false;
            deck.Remove(randomCard);
            availablePlayerHandSlots[i] = false;

        }
    }

    public void Shuffe()
    {
        if (discardPile.Count >= 1)
        {
            foreach (Card card in discardPile)
            {
                deck.Add(card);
            }
            discardPile.Clear();
        }
    }

    public void PayCardCost(int cardCost)
    {
        if ((playerRessourceCurrent - cardCost) >= 0)
        {
            playerRessourceCurrent -= cardCost;
        }
    }

    public void RefundCardCost(int cardCost)
    {
        if (playerRessourceMax >= (playerRessourceCurrent + cardCost))
        {
            playerRessourceCurrent += cardCost;
        }
    }

    public void EndTurn()
    {
        Debug.Log("Ending the turn!");
    }

}
