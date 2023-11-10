using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<Card> deck;
    public Transform deckParent;
    public int playerRessourceCurrent;
    public int playerRessourceMax;
    public TextMeshProUGUI playerRessourceText;

    public List<Slot> activeCardSlots;
    private Transform activeCardSlot;
    public bool[] availableCardSlots;

    public List<Slot> playerHandSlots;
    private Transform playerHandSlot;
    public bool[] availablePlayerHandSlots;

    public List<Enemy> wagons;

    public List<Card> discardPile;
    public Transform discardPileParent;

    public List<Card> graveyardPile;

    TurnMaster TurnMaster;
    PlayerHealthManager Player;

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
        TurnMaster = FindObjectOfType<TurnMaster>();
        Player = FindObjectOfType<PlayerHealthManager>();
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
            if (deck.Count <= 0)
            {
                Shuffle();
            }
            if (!playerHandSlots[i].GetComponent<Slot>().hasCard)
            {
                Card randomCard = deck[Random.Range(0, deck.Count)];
                randomCard.gameObject.SetActive(true);
                randomCard.GetComponent<CardMovementHandler>().DrawCardSetup(i, playerHandSlots[i].transform);
                deck.Remove(randomCard);
                availablePlayerHandSlots[i] = false;
                playerHandSlots[i].GetComponent<Slot>().HasCard(true);
            }
        }
    }

    public void Shuffle()
    {
        if (discardPile.Count >= 1)
        {
            foreach (Card card in discardPile)
            {
                deck.Add(card);
                card.BackInPlay(deckParent);
            }
            discardPile.Clear();
        }
    }

    public void ResetEnergy()
    {
        playerRessourceCurrent = playerRessourceMax;
    }

    public bool PayCardCost(Card card)
    {
        if ((playerRessourceCurrent - card.cost) >= 0)
        {
            playerRessourceCurrent -= card.cost;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RefundCardCost(Card card)
    {
        if (playerRessourceMax >= (playerRessourceCurrent + card.cost))
        {
            playerRessourceCurrent += card.cost;
        }

        
    }

    public void PlayerDamage(int damageValue, string damageType)
    {
        Player.ApplyDamage(damageValue, damageType);
    }

    public void EndTurn()
    {
        TurnMaster.ResolveTurn(wagons.ToArray(), activeCardSlots.ToArray());
        Debug.Log("Resolving the turn!");
        ResetEnergy();
        DrawCards();


    }

}
