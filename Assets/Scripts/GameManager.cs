using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public static event Action<int> UpdateDeckDisplay;
    public static event Action<int> UpdateDiscardDisplay;
    public static event Action UpdateUI;
    //public static event Action<int> CurrencyUpdateEvent;
    public static event Action<int> CardEnergyCostEffect;
    public static event Action CardRewardChosenSoundEvent;
    public static event Action NotEnoughEnergyEvent;
    public static event Action FirstCardPlayedEvent;
    public static event Action<int> SetEncounterBackgroundEvent;

    public int playerRessourceCurrent;
    public int playerRessourceMax;
    public int playerRessourceBuffMax;
    public int playerRessourceLoss;

    private ActiveCardSlots activeCardSlotsParent;
    public List<Slot> activeCardSlots;
    public List<Card> playedCards;

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
    public ShopCurrency shopCurrency;
    public PendantManager pendantManager;

    private List<Card> cardsToDiscard = new List<Card>();
    private int cardsToDiscardCount = 0;

    public bool encounterEndScreenActive = false;
    
    public bool cardRewardScreenActive;
    public int cardRewardAmount;
    public bool encounterWon = false;
    public bool encounterEnd = false;

    public GameObject endScreenPrefab;
    public GameObject cardRewardScreenPrefab;
    public PauseMenu pauseMenu;
    public NodeLoader nodeLoader;

    private bool discardActive = false;
    private bool haveToShuffle = false;

    public int encounterCompleted = 0;
    public int tokenRewardAmount;
    public int tokenRewardPendantBuffAmount = 0;
    public int tokenRewardChapterOne = 30;
    public int tokenRewardChapterTwo = 50;
    public int tokenRewardChapterThree = 70;

    public bool isFirstTurn = true;
    public bool isFirstCardPlayed = false;
    public bool firstCardPendantActive = false;

    public bool aluBuffPendantActive;
    public bool bleiBuffPendantActive;
    public bool paperBuffPendantActive;
    public int pendantBuffValue = 0;

    public bool isLastEncounterOnMap = false;

    private void Awake()
    {
        mySceneManager = FindObjectOfType<MySceneManager>();
        nodeLoader = FindObjectOfType<NodeLoader>();
        playerRessourceBuffMax = playerRessourceMax;
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        EndTurnButtonEventScript.EndTurnEvent += EndTurnEvent;
        CardPopup.PauseGame += PauseGame;
        Enemy.EncounterEnd += HandleEncounterEnd;
        PlayerHealthManager.EncounterEnd += HandleEncounterEnd;
        EncounterEndScript.CardRewardScreenEvent += HandleCardRewardEvent;
        CardMovementHandler.CardRewardChosenEvent += HandleCardRewardChosenEvent;
        CardMovementHandler.CardDropped += HandleCardDroppedEvent;
        Node.EnteringNodeEvent += HandleNodeEnterEvent;
        Node.EnteringNodeEvent += IncreaseCompletedEncounterCount;
    }

    private void IncreaseCompletedEncounterCount(bool arg1, string arg2)
    {
        encounterCompleted++;
    }

    private void HandleCardDroppedEvent()
    {
        if (isFirstTurn)
        {
            playedCards.Clear();
            int cardsPlayed = 0;
            foreach (Slot slot in activeCardSlots)
            {
                if (slot.GetCardInSlotInfo() != null)
                {
                    playedCards.Add(slot.GetCardInSlotInfo());
                    cardsPlayed++;
                }
            }
            if (cardsPlayed == 1)
            {
                FirstCardPlayedEvent?.Invoke();
            }
            if (cardsPlayed > 0)
            {
                isFirstCardPlayed = true;
            }
            else
            {
                isFirstCardPlayed = false;
            }
        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        isFirstCardPlayed = false;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Loaded Scene: " + scene.name);
        interactionBlock = FindObjectOfType<InteractionBlock>(true).transform;
        if (scene.name == "Encounter")
        {
            SetEncounterBackgroundEvent?.Invoke(encounterCompleted);
            playerHand = FindObjectOfType<PlayerHand>(true);
            playerEnergy = FindObjectOfType<PlayerEnergy>(true);
            discardPileParent = FindObjectOfType<DiscardPile>(true).transform;
            activeCardSlotsParent = FindObjectOfType<ActiveCardSlots>(true);
            activeCardSlots = activeCardSlotsParent.activeCardSlots;
            interactionBlock = FindObjectOfType<InteractionBlock>(true).transform;
            encounterEnd = false;
            cardManager.BuildDeck();
            wagons[0].StartEncounter();
            DrawCards();
            UpdateDiscard();
            wagons[0].GenerateDamage();
            SetTokenReward();
            pauseMenu = FindObjectOfType<PauseMenu>(true);
            pendantManager.TriggerPendantEffects();
            UpdatePlayerRessource();

        }
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    player.TriggerRandomDebuff();
        //    pendantManager.AwardRandomPendant();
        //}
    }

    private void HandleNodeEnterEvent(bool isLastNode, string nextMap)
    {
        if (isLastNode)
        {
            nodeLoader.originScene = nextMap;
            isLastEncounterOnMap = true;
        }
    }

    private void SetTokenReward()
    {
        if (encounterCompleted < 10)
        {
            tokenRewardAmount = tokenRewardChapterOne;
        }
        else if (encounterCompleted < 20)
        {
            tokenRewardAmount = tokenRewardChapterTwo;
        }
        else if (encounterCompleted >= 20)
        {
            tokenRewardAmount = tokenRewardChapterThree;
        }

    }
    public int GetCompletedEncounter()
    {
        encounterCompleted = 0;
        foreach (GameObject node in nodeLoader.nodes)
        {
            if (node.GetComponent<Node>().isCompleted)
            {
                encounterCompleted++;
            }
        }
        return encounterCompleted;
    }
    public bool IsCurrentEncounterBoss()
    {
        if (nodeLoader.activeNode.GetComponent<Node>().isLastNode)
        {
            return true;
        }
        else
        {
            return false;
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
            if (deck.deck.Count <= (discardPile.Count + playerHandMax + graveyardPile.Count) || deck.playerDeck.Count <= 0)
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
        haveToShuffle = false;
    }
    public void MaxEnergyDebuff(int value)
    {
        playerRessourceMax -= value;
        playerRessourceBuffMax = playerRessourceMax;
        if (playerRessourceCurrent > playerRessourceMax)
        {
            playerRessourceCurrent = playerRessourceMax;
        }
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
        if (playerRessourceLoss > 0)
        {
            playerRessourceCurrent -= playerRessourceLoss;
            playerRessourceLoss = 0;
        }
        UpdatePlayerRessource();
    }
    public bool EnoughEnergy(Card card)
    {
        if ((playerRessourceCurrent - card.cost) >= 0)
        {
            return true;
        }
        else
        {
            NotEnoughEnergyEvent?.Invoke();
            return false;
        }
    }
    public void PayCardCost(Card card)
    {
        if (EnoughEnergy(card))
        {
            playerRessourceCurrent -= card.cost;
            UpdatePlayerRessource();
            
        }
        else
        {
            UpdatePlayerRessource();
        }
    }
    public void RefundCardCost(Card card)
    {
        if ((playerRessourceMax + playerRessourceBuffMax >= (playerRessourceCurrent + card.cost)) || (playerRessourceBuffMax >= (playerRessourceCurrent + card.cost)))
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
        if (player.betaDotTimer > 0)
        {
            player.ApplyDamage(player.betaDotDamageSum, GameConstants.radiationTypes.Pure);
            player.betaDotTimer--;
            player.playerModel.betaDotTimer.text = player.betaDotTimer.ToString();
        }
        else
        {
            player.playerModel.betaDotDisplay.SetActive(false);
            player.betaDotActive = false;
        }
        
    }
    public void EndTurn()
    {
        PauseGame(true);
        Debug.Log("Resolving the turn!");
        Debug.Log("Enemies: " + wagons.Count);
        for (int i = 0; i < wagons.Count; i++)
        {
            SetDamage(wagons[i].damageStats);
        }
        turnMaster.ResolveTurn(wagons, activeCardSlots);
    }
    public void SetDamage(Dictionary<GameConstants.radiationTypes, int> damageStats)
    {
        turnMaster.SetDamage(damageStats);
    }
    private int CountOccupiedHandSlots()
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
    public void ActivateShieldDebuff(int value)
    {
        cardManager.ShieldDebuffEffect(value);

        if (SceneManager.GetActiveScene().name == "Encounter")
        {
            List<Card> cards = new List<Card>();

            cards.AddRange(FindObjectsOfType<Card>(true));
            if (cards != null && cards.Count > 0)
            {
                foreach (Card card in cards)
                {
                    card.ShieldDebuff(value);
                }
            }
        }
    }
    public void RemoveCardFromEncounter(Card card)
    {
        deck.RemoveCardFromPlayerDeck(card);
    }
    public void SetCardCostIncrease(int increase)
    {
        //costIncrease += increase;
        cardManager.CardEnergyCostEffect(increase);
        CardEnergyCostEffect?.Invoke(increase);
    }
    public bool IsBetaDotActive()
    {
        return player.betaDotActive;
    }    
    public void HandleEffect(GameConstants.effectTypes effectType, int effectValue)
    {
        switch (effectType)
        {
            case GameConstants.effectTypes.Discard:
                Debug.Log("Effect: " + effectType.ToString());
                TriggerDiscardEffect(effectValue);
                break;

            case GameConstants.effectTypes.DrawCard:
                Debug.Log("Effect: " + effectType.ToString());
                TriggerCardDrawEffect(effectValue);
                break;

            case GameConstants.effectTypes.EnergyGet:
                TriggerEnergyGetEffect(effectValue);
                Debug.Log("Effect: " + effectType.ToString());
                break;

            case GameConstants.effectTypes.ShieldMaxBuff:
                TriggerRandomShieldMaxBuff();
                break;

            case GameConstants.effectTypes.ShieldRepair:
                TriggerRandomShieldRepair();
                break;

            case GameConstants.effectTypes.EnergyLose:
                TriggerEnergyLoseEffect(effectValue);
                break;
        }
    }

    private void TriggerEnergyLoseEffect(int value)
    {
        playerRessourceLoss += value;
    }

    private void TriggerRandomShieldRepair()
    {
        List<Slot> activeSlotsWithCard = new List<Slot>();
        foreach (Slot slot in activeCardSlots)
        {
            if (slot.hasCard)
            {
                activeSlotsWithCard.Add(slot);
            }
        }
        if (activeSlotsWithCard.Count > 0)
        {
            int randomIndex = Random.Range(0, activeSlotsWithCard.Count);
            Slot randomSlot = activeSlotsWithCard[randomIndex];
            Debug.Log("Repaired card in slot: " + randomSlot);
            randomSlot.HandleShieldRepair();
        }
    }

    private void TriggerRandomShieldMaxBuff()
    {
        List<Slot> activeSlotsWithCard = new List<Slot>();
        foreach (Slot slot in activeCardSlots)
        {
            if (slot.hasCard)
            {
                activeSlotsWithCard.Add(slot);
            }
        }
        if (activeSlotsWithCard.Count - 1 > 0)
        {
            int randomIndex = Random.Range(0, activeSlotsWithCard.Count);
            Slot randomSlot = activeSlotsWithCard[randomIndex];
            Debug.Log("Buffed card in slot: " + randomSlot);
            randomSlot.HandleShieldMaxBuff();
        } 
    }

    private void TriggerCardDrawEffect(int value)
    {
        StartCoroutine(CardDrawEffect(value));
    }
    private IEnumerator CardDrawEffect(int value)
    {
        while (discardActive)
        {
            yield return new WaitForEndOfFrame();
        }

        int cardsInHand = CountOccupiedHandSlots();
        if (deck.deck.Count <= (discardPile.Count + cardsInHand + graveyardPile.Count + value) || deck.playerDeck.Count == 0)
        {
            haveToShuffle = true;
            Shuffle();
        }

        while (haveToShuffle)
        {
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < value; i++)
        {
            Card randomCard = deck.Draw();
            //Debug.Log("I just drew the card: " + randomCard);
            randomCard.GetComponent<CardMovementHandler>().DrawCardSetup(playerHand.transform);
            deck.RemoveCardFromPlayerDeck(randomCard);
        }
        UpdateDeckDisplay?.Invoke(deck.playerDeck.Count);
        yield break;
    }
    private void TriggerEnergyGetEffect(int value)
    {
        AddEnergy(value);
    }
    private void TriggerDiscardEffect(int value)
    {
        discardActive = true;
        StartCoroutine(DiscardRandomCardFromHand(value));
        //Coroutine to have players select a card to discard - not in use
        //StartCoroutine(PlayerSelectCardsToDiscard(triggerCard, value));
    }
    private void HandleCardClicked(CardMovementHandler cardMovementHandler)
    {
        Debug.Log("Card Clicked: " + cardMovementHandler.gameObject.name);

        Card clickedCard = cardMovementHandler.GetComponent<Card>();

        cardsToDiscard.Add(clickedCard);
        cardsToDiscardCount++;
    }
    private IEnumerator DiscardRandomCardFromHand(int value)
    {
        List<Card> cardsInHand = new List<Card>();
        cardsInHand.AddRange(playerHand.GetComponentsInChildren<Card>());

        // Ensure that the value is within a valid range
        value = Mathf.Clamp(value, 0, cardsInHand.Count);

        Debug.Log("Removing " + value + " cards from hand.");
        // Randomly select cards and perform the discard effect
        for (int i = 0; i < value; i++)
        {
            Debug.Log("Removed card nr. " + i);
            // Ensure there are still cards in the list
            if (cardsInHand.Count > 0)
            {
                int randomIndex = Random.Range(0, cardsInHand.Count);

                Card randomCard = cardsInHand[randomIndex];

                randomCard.cardMovementHandler.MoveToDiscardPile();

                cardsInHand.RemoveAt(randomIndex);
            }
            yield return new WaitForSeconds(1f);
        }
        discardActive = false;
        yield break;
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
    private void EndTurnEvent()
    {
        EndTurn();
    }
    private void HandleEncounterEnd()
    {
        wagons[0].TriggerEncounterEndAnimation();
        player.TriggerEncounterEndAnimation();
        if (wagons[0].roundTimer <= 0 && player.health > 0)
        {
            encounterWon = true;
        }
        else
        {
            encounterWon = false;
        }
        foreach (Slot activeCardSlot in activeCardSlots)
        {
            activeCardSlot.gameObject.SetActive(false);
        }
        
        StartCoroutine(EndingEncounter());
    }
    private IEnumerator EndingEncounter()
    {
        encounterEnd = true;
        encounterEndScreenActive = true;
        PauseGame(true);
        UpdateUI?.Invoke();

        yield return new WaitForSeconds(1f);
       
        GameObject endingScreen = Instantiate(endScreenPrefab, pauseMenu.offscreenPosition.transform.position, Quaternion.identity);
        endingScreen.GetComponent<EncounterEndScript>().SetupScreen(encounterWon, tokenRewardAmount, pauseMenu.offscreenPosition);
        if (encounterWon)
        {
            shopCurrency.AddMoney(tokenRewardAmount + tokenRewardPendantBuffAmount);
        }
        //Falls wir zeit brauchen um animationen abzuspielen o.ä.
        while (encounterEndScreenActive)
        {
            yield return null;
        }
        encounterEndScreenActive = false;
        yield break;
    }
    private void HandleCardRewardEvent()
    {
        StartCoroutine(CardReward());
    }
    private IEnumerator CardReward()
    {
        cardRewardScreenActive = true;
        List<GameObject> cardRewards = new List<GameObject>();
        GameObject cardRewardScreen = Instantiate(cardRewardScreenPrefab, pauseMenu.targetPosition.transform.position, Quaternion.identity);
        for (int i = 0; i < cardRewardAmount; i++)
        {
            cardRewards.Add(cardManager.GetRandomCardFromCardSafe());
        }
        foreach (GameObject randomCard in cardRewards)
        {
            randomCard.transform.SetParent(cardRewardScreen.GetComponent<CardRewardScript>().rewardArea.transform);
            randomCard.SetActive(true);
            randomCard.GetComponent<SortingGroup>().sortingLayerName = "Menu";
            randomCard.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            randomCard.transform.localPosition = new Vector3(randomCard.transform.localPosition.x, randomCard.transform.localPosition.y, 0f);
            randomCard.GetComponent<CardMovementHandler>().inRewardScreen = true;
            randomCard.GetComponent<SortingGroup>().sortingOrder = 1;
        }
        while (cardRewardScreenActive)
        {
            yield return null;
        }
        CardRewardChosenSoundEvent?.Invoke();
        cardRewardScreen.GetComponent<CardRewardScript>().ToggleButton(true);
        foreach (GameObject randomCard in cardRewards)
        {
            randomCard.GetComponent<CardMovementHandler>().inRewardScreen = false;
            randomCard.GetComponent<SortingGroup>().sortingLayerName = "Card";
            randomCard.SetActive(false);
            randomCard.transform.SetParent(cardManager.cardSafe.transform);
        }
        Shuffle();
        PauseGame(false);
        if (isLastEncounterOnMap)
        {
            ResetEnergy();
            player.ResetResistances();
        }
        yield break;
    }
    private void HandleCardRewardChosenEvent(Card obj)
    {
        cardRewardScreenActive = false;
    }
    public void HandlePendantBuffActiavtion(GameConstants.pendantEffect effect, int effectValue)
    {
        switch (effect)
        {
            case GameConstants.pendantEffect.encounterEndMoreToken:
                tokenRewardPendantBuffAmount = effectValue;
                break;

            case GameConstants.pendantEffect.firstCardLessCost:
                firstCardPendantActive = true;
                //if (!isFirstCardPlayed)
                //{
                //    costIncrease = -effectValue;
                //}
                break;

            case GameConstants.pendantEffect.buffAlu:
                aluBuffPendantActive = true;
                break;

            case GameConstants.pendantEffect.buffPaper:
                paperBuffPendantActive = true;
                break;

            case GameConstants.pendantEffect.buffBlei:
                bleiBuffPendantActive = true;
                break;

            case GameConstants.pendantEffect.firstTurnMoreEnergy:
                if (isFirstTurn)
                {
                    Debug.Log("First Turn more Energy: " + playerRessourceBuffMax + " will get + " + effectValue);
                    playerRessourceCurrent += effectValue;
                    playerRessourceBuffMax += effectValue;
                    playerEnergy.UpdatePlayerEnergy(playerRessourceCurrent);
                }
                break;

            case GameConstants.pendantEffect.firstTurnMoreCards:
                if (isFirstTurn)
                {
                    playerHandMax += effectValue;
                    DrawCards();
                    playerHandMax -= effectValue;
                }
                break;
        }
    }

    public void UpgradeCard(Card card)
    {
        deck.UpgradeCard(card);
    }

    public void TriggerEventAward()
    {
        pendantManager.AwardRandomPendant();
    }
}