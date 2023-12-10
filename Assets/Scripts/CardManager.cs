using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardManager : MonoBehaviour
{
    public List<GameConstants.cardRarity> cardRarity;
    public List<GameConstants.cardType> cardType;
    public List<GameObject> prefabs;

    public GameObject cardSafe;
    GameManager gm;
    public Dictionary<GameConstants.cardType, Dictionary<GameConstants.cardRarity, GameObject>> prefabMapping;

    public SO_Card[] cardInfos;
    public Deck deck;

    private void Awake()
    {
        LoadCardsFromResources();
        gm = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        
        InitializePrefabMapping();
        AssignPrefabsToCards();
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
                        Debug.Log($"Assigning prefab {prefab.name} to card {card.name}");
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
                            cardComponent.SetActive(false);
                            deck.AddCardToDeck(cardComponent);
                            Debug.Log($"Instantiated card {cardComponent.cardInfo.name} with prefab {prefab.name}");
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
        deck.PopulatePlayerDeck();
        gm.DrawCards();
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

}
