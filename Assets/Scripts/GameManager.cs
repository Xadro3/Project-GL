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

    TurnMaster turnMaster;
    PlayerHealthManager player;
    MySceneManager mySceneManager;

    public bool wait;
    private float elapsedTime;
    [Range(2,10)]
    public float waitTimer;

    // Start is called before the first frame update
    void Start()
    {
        mySceneManager = FindObjectOfType<MySceneManager>();
        turnMaster = FindObjectOfType<TurnMaster>();
        player = FindObjectOfType<PlayerHealthManager>();
        DrawCards();
    }

    // Update is called once per frame
    void Update()
    {
        if (wait)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= waitTimer)
            {
                wait = false;
                mySceneManager.ChangeScene("Overworld");
            }
        }
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
        UpdatePlayerRessource();
    }

    public bool PayCardCost(Card card)
    {
        if ((playerRessourceCurrent - card.cost) >= 0)
        {
            playerRessourceCurrent -= card.cost;
            UpdatePlayerRessource();
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
            UpdatePlayerRessource();
        }

        
    }

    public void UpdatePlayerRessource()
    {
        playerRessourceText.text = playerRessourceCurrent.ToString();
    }

    public void PlayerDamage(int damageValue, string damageType)
    {
        player.ApplyDamage(damageValue, damageType);
    }

    public void EndTurn()
    {
        turnMaster.ResolveTurn(wagons.ToArray(), activeCardSlots.ToArray());
        Debug.Log("Resolving the turn!");
        wagons[0].GenerateRandomDamage();
        ResetEnergy();
        DrawCards();
        if (wagons[0].UpdateTimer(1))
        {
            wait = true;
        }
    }

    public void WaitTimer(float timerDuration, float elapsedTime)
    {

    }

}
