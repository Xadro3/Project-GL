using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class CardManager : MonoBehaviour
{
    public List<GameConstants.cardRarity> cardRarity;
    public List<GameConstants.cardType> cardType;
    public List<GameObject> prefabs;

    public List<SO_Card> baseCards;

    public GameObject cardSafe;
    public Dictionary<GameConstants.cardType, Dictionary<GameConstants.cardRarity, GameObject>> prefabMapping;

    public List<GameObject> cardSafeCards;

    public SO_Card[] cardInfos;
    public Deck deck;
    public GameManager gameManager;

    private void Awake()
    {
        LoadCardsFromResources();
        InitializePrefabMapping();
        AssignPrefabsToCards();
        AddBaseCardsToDeck();
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

        }
    }

    private void Start()
    {
        foreach (SO_Card cardInfo in baseCards)
        {
            cardInfo.energyCostAffected = false;
            cardInfo.energyCostIncrease = 0;
        }
        //GetRandomCardFromCardSafe();
    }

    public void CardEnergyCostEffect(int value)
    {
        foreach (SO_Card cardInfo in baseCards)
        {
            cardInfo.energyCostAffected = true;
            cardInfo.energyCostIncrease = 0 + value;
        }
    }

    public void AddCardToBaseDeck(Card card)
    {
        baseCards.Add(card.cardInfo);
    }

    public void RemoveCardFromBaseDeck(Card card)
    {
        baseCards.Remove(card.cardInfo);
    }

    public void BuildDeck()
    {
        AddBaseCardsToDeck();
        deck.PopulatePlayerDeck();
    }

    private void AddBaseCardsToDeck()
    {
        deck.deck.Clear();
        foreach (SO_Card baseCard in baseCards)
        {
            // Ensure that cardType and cardRarity arrays are not empty
            if (baseCard.cardType.Count > 0 && baseCard.cardRarity.Count > 0)
            {
                // Check if cardType[0] exists in prefabMapping
                if (prefabMapping.TryGetValue(baseCard.cardType[0], out var rarityMapping))
                {
                    if (rarityMapping.TryGetValue(baseCard.cardRarity[0], out var prefab))
                    {
                        // Assign prefab to card
                        // Debug.Log($"Assigning prefab {prefab.name} to card {baseCard.name}");
                        // Instantiate the prefab
                        GameObject instantiatedCard = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                        instantiatedCard.transform.SetParent(cardSafe.transform);
                        instantiatedCard.transform.position = cardSafe.transform.position;
                        // Access the Card component of the instantiated card
                        Card cardComponent = instantiatedCard.GetComponent<Card>();

                        // Assign the SO_Card to the Card component
                        if (cardComponent != null)
                        {
                            cardComponent.cardInfo = baseCard;
                            instantiatedCard.name = baseCard.name;
                            cardComponent.SetActive(false);
                            deck.AddCardToDeck(cardComponent);
                            //Debug.Log($"Instantiated card {cardComponent.cardInfo.name} with prefab {prefab.name}");
                        }
                        else
                        {
                            Debug.LogError($"Card component not found on instantiated card {instantiatedCard.name}");
                        }

                    }
                    else
                    {
                        Debug.LogWarning($"Prefab not found for rarity {baseCard.cardRarity[0]} and type {baseCard.cardType[0]} in rarityMapping");
                    }
                }
                else
                {
                    Debug.LogWarning($"Prefab mapping not found for type {baseCard.cardType[0]} in prefabMapping");
                }
            }
            else
            {
                Debug.LogWarning($"cardType or cardRarity array is empty for card {baseCard.name}");
            }
        }
    }

    void InitializePrefabMapping()
    {
        // Initialize the mapping
        prefabMapping = new Dictionary<GameConstants.cardType, Dictionary<GameConstants.cardRarity, GameObject>>();

        // Example: Add prefabs to the mapping
        AddPrefabToMapping(GameConstants.cardType.FähigkeitGegner, GameConstants.cardRarity.Common, prefabs[0]);
        AddPrefabToMapping(GameConstants.cardType.FähigkeitGegner, GameConstants.cardRarity.Uncommon, prefabs[1]);
        AddPrefabToMapping(GameConstants.cardType.FähigkeitGegner, GameConstants.cardRarity.Rare, prefabs[2]);
        AddPrefabToMapping(GameConstants.cardType.Fähigkeit, GameConstants.cardRarity.Common, prefabs[3]);
        AddPrefabToMapping(GameConstants.cardType.Fähigkeit, GameConstants.cardRarity.Uncommon, prefabs[4]);
        AddPrefabToMapping(GameConstants.cardType.Fähigkeit, GameConstants.cardRarity.Rare, prefabs[5]);
        AddPrefabToMapping(GameConstants.cardType.SchildAuflösung, GameConstants.cardRarity.Common, prefabs[6]);
        AddPrefabToMapping(GameConstants.cardType.SchildAuflösung, GameConstants.cardRarity.Uncommon, prefabs[7]);
        AddPrefabToMapping(GameConstants.cardType.SchildAuflösung, GameConstants.cardRarity.Rare, prefabs[8]);
        AddPrefabToMapping(GameConstants.cardType.SchildBuff, GameConstants.cardRarity.Common, prefabs[9]);
        AddPrefabToMapping(GameConstants.cardType.SchildBuff, GameConstants.cardRarity.Uncommon, prefabs[10]);
        AddPrefabToMapping(GameConstants.cardType.SchildBuff, GameConstants.cardRarity.Rare, prefabs[11]);
        AddPrefabToMapping(GameConstants.cardType.Schild, GameConstants.cardRarity.Common, prefabs[12]);
        AddPrefabToMapping(GameConstants.cardType.Schild, GameConstants.cardRarity.Uncommon, prefabs[13]);
        AddPrefabToMapping(GameConstants.cardType.Schild, GameConstants.cardRarity.Rare, prefabs[14]);
        AddPrefabToMapping(GameConstants.cardType.SchildReparatur, GameConstants.cardRarity.Common, prefabs[15]);
        AddPrefabToMapping(GameConstants.cardType.SchildReparatur, GameConstants.cardRarity.Uncommon, prefabs[16]);
        AddPrefabToMapping(GameConstants.cardType.SchildReparatur, GameConstants.cardRarity.Rare, prefabs[17]);

    }
    void AssignPrefabsToCards()
    {
        foreach (SO_Card card in cardInfos)
        {
            // Ensure that cardType and cardRarity arrays are not empty
            if (card.cardType.Count > 0 && card.cardRarity.Count > 0)
            {
                // Check if cardType[0] exists in prefabMapping
                if (prefabMapping.TryGetValue(card.cardType[0], out var rarityMapping))
                {
                    if (rarityMapping.TryGetValue(card.cardRarity[0], out var prefab))
                    {
                        // Assign prefab to card
                        //Debug.Log($"Assigning prefab {prefab.name} to card {card.name}");
                        // Instantiate the prefab
                        GameObject instantiatedCard = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                        instantiatedCard.transform.SetParent(cardSafe.transform);
                        instantiatedCard.transform.position = cardSafe.transform.position;
                        // Access the Card component of the instantiated card
                        Card cardComponent = instantiatedCard.GetComponent<Card>();


                        // Assign the SO_Card to the Card component
                        if (cardComponent != null)
                        {
                            cardComponent.cardInfo = card;
                            instantiatedCard.name = card.name;
                            cardSafeCards.Add(instantiatedCard);
                            instantiatedCard.SetActive(false);

                            // Remove comment if you want to add all cards to the deck
                            //deck.AddCardToDeck(cardComponent);
                            //Debug.Log($"Instantiated card {cardComponent.cardInfo.name} with prefab {prefab.name}");
                        }
                        else
                        {
                            Debug.LogError($"Card component not found on instantiated card {instantiatedCard.name}");
                        }

                    }
                    else
                    {
                        Debug.LogWarning($"Prefab not found for rarity {card.cardRarity[0]} and type {card.cardType[0]} in rarityMapping");
                    }
                }
                else
                {
                    Debug.LogWarning($"Prefab mapping not found for type {card.cardType[0]} in prefabMapping");
                }
            }
            else
            {
                Debug.LogWarning($"cardType or cardRarity array is empty for card {card.name}");
            }
        }
    }

    void AddPrefabToMapping(GameConstants.cardType cardType, GameConstants.cardRarity cardRarity, GameObject prefab)
    {
        if (!prefabMapping.ContainsKey(cardType))
        {
            prefabMapping[cardType] = new Dictionary<GameConstants.cardRarity, GameObject>();
        }

        prefabMapping[cardType][cardRarity] = prefab;
    }

    private void LoadCardsFromResources()
    {
        cardInfos = Resources.LoadAll("Cardinfos", typeof(SO_Card)).Cast<SO_Card>().ToArray();
    }

    public GameObject GetRandomCardFromCardSafe()
    {
        int randomIndex = UnityEngine.Random.Range(0, cardSafeCards.Count());
        // Return a random card from playerDeck
        GameObject randomCard = Instantiate(cardSafeCards[randomIndex], transform.position, Quaternion.identity);
        Debug.Log("Random Card From Card Safe: "+ randomCard.name);
        return randomCard;

    }

}
