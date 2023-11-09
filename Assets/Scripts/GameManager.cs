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
        //not in use anymore/yet
    }

    public static void AssignToActiveSlot(Card card, Slot activeCardSlot)
    {
        //not in use anymore/yet
    }

    public static void AssignToPlayerHand(Card card, Transform playerHandSlot, Slot activeCardSlot)
    {
        //not in use anymore/yet
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
            if (!playerHandSlots[i].GetComponent<Slot>().hasCard)
            {
                Card randomCard = deck[Random.Range(0, deck.Count)];
                randomCard.gameObject.SetActive(true);
                randomCard.GetComponent<CardMovementHandler>().DrawCardSetup(i, playerHandSlots[i].transform);
                deck.Remove(randomCard);
                availablePlayerHandSlots[i] = false;
            }
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

    public void PayCardCost(Card card)
    {
        if ((playerRessourceCurrent - card.cost) >= 0)
        {
            playerRessourceCurrent -= card.cost;
        }
    }

    public void RefundCardCost(Card card)
    {
        if (playerRessourceMax >= (playerRessourceCurrent + card.cost))
        {
            playerRessourceCurrent += card.cost;
        }
    }

    public void EndTurn()
    {
        Debug.Log("Ending the turn!");
    }

}
