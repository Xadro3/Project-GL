using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int playerRessourceCurrent;
    public int playerRessourceMax;
    public TextMeshProUGUI playerRessourceText;

    public List<Slot> activeCardSlots;
    private Transform activeCardSlot;

    public RectTransform playerHand;
    public List<Slot> playerHandSlots;
    public int playerHandMax;
    private Transform playerHandSlot;

    public List<Enemy> wagons;

    public List<Card> discardPile;
    public Transform discardPileParent;

    public List<Card> graveyardPile;

    public bool isPauseMenuActive = false;

    TurnMaster turnMaster;
    PlayerHealthManager player;
    MySceneManager mySceneManager;
    Deck deck;

    public bool wait;
    private float elapsedTime;
    [Range(2,10)]
    public float waitTimer;

    private void Awake()
    {
        mySceneManager = FindObjectOfType<MySceneManager>();
        turnMaster = FindObjectOfType<TurnMaster>();
        player = FindObjectOfType<PlayerHealthManager>();
        deck = FindObjectOfType<Deck>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        DrawCards();
        SetDamage(wagons[0].GenerateDamage());
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

    public void PauseGame(bool b)
    {
        isPauseMenuActive = b;
    }

    public void DrawCards()
    {
        int cardsInHand = CountOccupiedHandSlots();
        if (cardsInHand < playerHandMax)
        {
            // drawing cards up to playerHandMax
            for (int i = 0; i < (playerHandMax - cardsInHand); i++)
            {
                //Wenn Anzahl an Cards im Deck <= Cards im Discard + Cards auf der Hand + Cards aufm Graveyard
                if (deck.deck.Count <= (discardPile.Count + cardsInHand + graveyardPile.Count))
                {
                    Shuffle();
                }
                if (!playerHandSlots[i].hasCard)
                {
                    Card randomCard = deck.Draw();
                    randomCard.GetComponent<CardMovementHandler>().DrawCardSetup(i, playerHand.transform);
                    deck.RemoveCard(randomCard);
                    playerHandSlots[i].HasCard(true);
                }
            }
        }
        
        
    }

    public void Shuffle()
    {
        if (discardPile.Count >= 1)
        {
            foreach (Card card in discardPile)
            {
                deck.AddCard(card);
                card.BackInPlay(deck.transform);
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
        turnMaster.ResolveTurn(wagons, activeCardSlots);
        Debug.Log("Resolving the turn!");
        SetDamage(wagons[0].GenerateDamage());
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

    public void SetDamage(Dictionary<string, int> damageStats)
    {
        turnMaster.SetDamage(damageStats);
    }

    int CountOccupiedHandSlots()
    {
        int anzAvailableHandSlots = 0;
        int currentCards = 0;
        currentCards = playerHand.transform.childCount;

        for (int i = 0; i < playerHandSlots.Count; i++)
        {
            if (playerHandSlots[i].hasCard)
            {
                anzAvailableHandSlots++;
            }
        }

        return anzAvailableHandSlots;
    }

}
