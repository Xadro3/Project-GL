using System.Collections.Generic;
using System.Collections;
using System.Timers;
using UnityEngine;
using UnityEngine.Timeline;
using System;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action<int> UpdateDeckDisplay;
    public static event Action<int> UpdateDiscardDisplay;

    public int playerRessourceCurrent;
    public int playerRessourceMax;
    public int playerRessourceBuffMax;

    private ActiveCardSlots activeCardSlotsParent;
    public List<Slot> activeCardSlots;

    public PlayerEnergy playerEnergy;
    public PlayerHand playerHand;
    public int playerHandMax;

    public List<Enemy> wagons;

    public List<Card> discardPile;
    public Transform discardPileParent;

    public List<Card> graveyardPile;
    public Transform graveyardPileParent;

    public bool isGamePauseActive = false;
    public Transform interactionBlock;

    public TurnMaster turnMaster;
    public PlayerHealthManager player;
    MySceneManager mySceneManager;
    public Deck deck;
    public CardManager cardManager;

    public bool wait;
    private float elapsedTime;
    [Range(2,10)]
    public float waitTimer;

    private int costIncrease = 0;

    private List<Card> cardsToDiscard = new List<Card>();
    private int cardsToDiscardCount = 0;

    private void Awake()
    {
        mySceneManager = FindObjectOfType<MySceneManager>();
        turnMaster = GetComponentInChildren<TurnMaster>();
        player = GetComponentInChildren<PlayerHealthManager>();
        cardManager = GetComponentInChildren<CardManager>();
        deck = GetComponentInChildren<Deck>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Encounter")
        {
            playerHand = FindObjectOfType<PlayerHand>();
            playerEnergy = FindObjectOfType<PlayerEnergy>();
            discardPileParent = FindObjectOfType<DiscardPile>().transform;
            activeCardSlotsParent = FindObjectOfType<ActiveCardSlots>();
            activeCardSlots = activeCardSlotsParent.activeCardSlots;
            wagons[0].GenerateDamage();
            UpdatePlayerRessource();
            DrawCards();
            UpdateDiscard();
        }
    }

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (wait)
        {
            PauseGame(wait);
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= waitTimer)
            {
                wait = false;
                PauseGame(wait);
                mySceneManager.ChangeScene("Overworld");
            }
        }
    }

    public void PauseGame(bool b)
    {
        isGamePauseActive = b;
        BlockInteraction(b);
    }

    public void BlockInteraction(bool b)
    {
        interactionBlock.gameObject.SetActive(b);
    }

    public void DrawCards()
    {
        int cardsInHand = CountOccupiedHandSlots();
        if (cardsInHand < playerHandMax)
        {
            //Wenn Anzahl an Cards im Deck <= Cards im Discard + Cards auf der Hand + Cards aufm Graveyard
            if (deck.deck.Count <= (discardPile.Count + cardsInHand + graveyardPile.Count) || deck.playerDeck.Count == 0)
            {
                Shuffle();
            }

            // drawing cards up to playerHandMax
            for (int i = 0; i < (playerHandMax - cardsInHand); i++)
            {
                Card randomCard = deck.Draw();
                //Debug.Log("I just drew the card: " + randomCard);
                randomCard.GetComponent<CardMovementHandler>().DrawCardSetup(playerHand.transform);
                deck.RemoveCardFromPlayerDeck(randomCard);
                UpdateDeckDisplay?.Invoke(deck.playerDeck.Count);
            }
        }
        UpdateDeckDisplay?.Invoke(deck.playerDeck.Count);
    }

    public void Shuffle()
    {
        if (discardPile.Count >= 1)
        {
            foreach (Card card in discardPile)
            {
                deck.AddCardToPlayerDeck(card);
                card.BackInPlay(deck.transform);
            }
            discardPile.Clear();
        }
        UpdateDeckDisplay(deck.playerDeck.Count);
}

    public void AddEnergy(int value)
    {
        playerRessourceCurrent += value;
        if (playerRessourceCurrent > playerRessourceMax)
        {
            playerRessourceBuffMax = playerRessourceCurrent;
        }
        UpdatePlayerRessource();
    }
    public void ResetEnergy()
    {
        playerRessourceCurrent = playerRessourceMax;
        playerRessourceBuffMax = playerRessourceMax;
        UpdatePlayerRessource();
    }

    public bool PayCardCost(Card card)
    {
        if ((playerRessourceCurrent - card.cost) >= 0)
        {
            playerRessourceCurrent -= card.cost + costIncrease;
            UpdatePlayerRessource();
            return true;
        }
        else
        {
            UpdatePlayerRessource();
            return false;
        }
    }

    public void RefundCardCost(Card card)
    {
        if ((playerRessourceMax >= (playerRessourceCurrent + card.cost)) || (playerRessourceBuffMax >= (playerRessourceCurrent + card.cost)))
        {
            playerRessourceCurrent += card.cost;
            UpdatePlayerRessource();
        }
        else
        {
            Debug.Log("Cannot refund card cost.");
        }
    }

    public void UpdatePlayerRessource()
    {
        playerEnergy.UpdatePlayerEnergy(playerRessourceCurrent);
    }

    public void PlayerDamage(int damageValue, GameConstants.radiationTypes damageType)
    {
        player.ApplyDamage(damageValue, damageType);
    }

    public void PlayerBetaDotDamage()
    {
        player.ApplyDamage(player.betaDotDamage, GameConstants.radiationTypes.Pure);
    }

    public void EndTurn()
    {
        SetDamage(wagons[0].damageStats);
        turnMaster.ResolveTurn(wagons, activeCardSlots);
        Debug.Log("Resolving the turn!");
        //SetDamage(wagons[0].GenerateDamage());
        //ResetEnergy();
        //DrawCards();
        if (wagons[0].UpdateTimer(1))
        {
            wait = true;
        }
    }

    public void WaitTimer(float timerDuration, float elapsedTime)
    {

    }

    public void SetDamage(Dictionary<GameConstants.radiationTypes, int> damageStats)
    {
        turnMaster.SetDamage(damageStats);
    }

    int CountOccupiedHandSlots()
    {
        int currentCards = playerHand.transform.childCount;
        return currentCards;
    }
    public void UpdateDiscard()
    {
        UpdateDiscardDisplay?.Invoke(discardPile.Count);
    }
    public void ActivateDamageBuff(GameConstants.radiationTypes radiationType)
    {
        wagons[0].ActivateDamageBuff(radiationType);
    }

    public void ActivateShieldDebuff()
    {
        List<Card> cards = new List<Card>();

        cards.AddRange(FindObjectsOfType<Card>(true));
        foreach (Card card in cards)
        {
            card.ShieldDebuff();
        }
    }

    public void RemoveCardFromEncounter(Card card)
    {
        deck.RemoveCardFromPlayerDeck(card);
    }

    public void SetCardCostIncrease(int increase)
    {
        costIncrease += increase;
    }

    public bool IsBetaDotActive()
    {
        return player.betaDotActive;
    }
    
    public void HandleEffect(Card card)
    {
        foreach (var entry in card.cardEffects)
        {
            switch (entry.Key)
            {
                case GameConstants.effectTypes.Discard:
                    TriggerDiscardEffect(card, entry.Value);
                    break;

                case GameConstants.effectTypes.DrawCard:
                    Debug.Log("Effect: " + entry.Key);
                    TriggerCardDrawEffect(entry.Value);
                    break;

                case GameConstants.effectTypes.EnergyGet:
                    TriggerEnergyGetEffect(entry.Value);
                    Debug.Log("Effect: " + entry.Key);
                    break;
            }
        }
    }
    private void TriggerCardDrawEffect(int value)
    {
        int cardsInHand = CountOccupiedHandSlots();
        if (deck.deck.Count <= (discardPile.Count + cardsInHand + graveyardPile.Count + value) || deck.playerDeck.Count == 0)
        {
            Shuffle();
        }
        // drawing cards up to effect
        for (int i = 0; i < value; i++)
        {
            Card randomCard = deck.Draw();
            //Debug.Log("I just drew the card: " + randomCard);
            randomCard.GetComponent<CardMovementHandler>().DrawCardSetup(playerHand.transform);
            deck.RemoveCardFromPlayerDeck(randomCard);
        }
    }
    private void TriggerEnergyGetEffect(int value)
    {
        AddEnergy(value);
    }

    private void TriggerDiscardEffect(Card triggerCard, int value)
    {
        StartCoroutine(PlayerSelectCardsToDiscard(triggerCard, value));
    }

    private void HandleCardClicked(CardMovementHandler cardMovementHandler)
    {
        Debug.Log("Card Clicked: " + cardMovementHandler.gameObject.name);

        Card clickedCard = cardMovementHandler.GetComponent<Card>();

        cardsToDiscard.Add(clickedCard);
        cardsToDiscardCount++;
    }

    private IEnumerator PlayerSelectCardsToDiscard(Card triggerCard, int value)
    {
        List<Card> cardsInHand = new List<Card>();
        cardsInHand.AddRange(playerHand.GetComponentsInChildren<Card>());
        
        foreach (Card card in cardsInHand)
        {
            CardMovementHandler movementHandler = card.GetComponent<CardMovementHandler>();
            if (movementHandler != null)
            {
                movementHandler.OnCardClicked += HandleCardClicked;
            }
        }
        // Wait for the player to select cards
        while (cardsToDiscardCount < value)
        {
            // You can add some logic to wait for player input or other conditions
            yield return null;
        }

        foreach (Card card in cardsInHand)
        {
            CardMovementHandler movementHandler = card.GetComponent<CardMovementHandler>();
            if (movementHandler != null)
            {
                movementHandler.OnCardClicked -= HandleCardClicked;
            }
        }

        yield return new WaitForSeconds(2f);

        foreach (Card card in cardsToDiscard)
        {
            card.GetComponent<CardMovementHandler>().MoveToDiscardPile();
        }

        cardsToDiscard.Clear();
        cardsToDiscardCount = 0;
        yield break;
    }
}
