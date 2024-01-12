using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CardMovementHandler : MonoBehaviour
{
    public static event System.Action CardDropped;
    public event Action<CardMovementHandler> OnCardClicked;
    public static event Action<Card> OnPlayerEffect;
    public static event Action<Card> OnEnemyEffect;
    public static event Action<Card, Slot> OnShieldEffect;
    public static event Action<Card> CardRewardChosenEvent;
    public static event System.Action CardMoveToDiscardPileEvent;
    public static event System.Action ShowCardPopupEvent;

    public GameObject cardPopupPrefab;

    private int handIndex;

    public bool wasPlayed = false;
    public bool isDragging = false;
    public bool inRewardScreen = false;

    private Vector3 offset;
    private Vector3 mousePosition;
    public Transform initialHandSlot;

    private GameObject placeholder = null;
    private bool hasPlaceholder = false;

    public Transform currentSlot = null;
    public Slot activeCardSlot = null;

    public GameManager gm;
    SortingGroup sortingGroup;
    public Card card;

    private SpriteRenderer[] spriteRenderers;
    private MeshRenderer[] textRenderers;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        textRenderers = GetComponentsInChildren<MeshRenderer>();
        sortingGroup = GetComponent<SortingGroup>();
        enabled = true;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene arg0)
    {
        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        if (scene.name == "Shops" && this != null)
        {
            this.enabled = false;
        }
    }

    void Start()
    {
        //SetSortingOrder(99);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (sortingGroup.sortingOrder != transform.GetSiblingIndex() && !isDragging) 
        {
            SetSortingOrder(transform.GetSiblingIndex());
        }
        
    }

    public void DrawCardSetup(Transform parent)
    {
        SetNewParent(parent);
        SetPosition(parent);
        SetInitialHandslot(parent);
        card.SetActive(true);
        Debug.Log(gameObject.name + " was drawn.");
    }

    private void SetInitialHandslot(Transform newInitialHandslot)
    {
        initialHandSlot = newInitialHandslot;
    }

    public void MoveToDiscardPile()
    {
        CardMoveToDiscardPileEvent?.Invoke();
        if (activeCardSlot != null)
        {
            activeCardSlot.HasCard(false);
        }
        
        gm.discardPile.Add(card);
        gm.UpdateDiscard();
        SetNewParent(gm.discardPileParent);
        SetPosition(gm.discardPileParent);
        card.SetWasPlayed(false);
        card.SetActive(false);
        wasPlayed = false;
    }

    public void MoveToGraveyardPile()
    {
        if (activeCardSlot != null)
        {
            activeCardSlot.HasCard(false);
        }

        gm.graveyardPile.Add(card);
        SetNewParent(gm.graveyardPileParent);
        SetPosition(gm.graveyardPileParent);
        card.SetWasPlayed(false);
        card.SetActive(false);
        wasPlayed = false;
    }

    public void SetNewParent(Transform parent)
    {
        currentSlot = parent.transform;
        transform.SetParent(currentSlot);
    }

    private void SetPosition(Transform newPosition)
    {
        transform.position = newPosition.position;
        transform.rotation = newPosition.rotation;
    }

    //Mouse movement with card
    private void OnMouseDown()
    {
        if (!gm.isGamePauseActive && enabled)
        {
            mousePosition = Input.mousePosition - GetMouseWorldPos();
            if (!wasPlayed)
            {
                //offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                isDragging = true;
                if (!gm.isFirstCardPlayed && gm.firstCardPendantActive)
                {
                    card.cost -= 1;
                    card.UpdateDisplay();
                }
            }
            OnCardClicked?.Invoke(this);
            SetSortingOrder(transform.GetSiblingIndex());
        }
    }
    private void OnMouseDrag()
    {
        if (!gm.isGamePauseActive && enabled)
        {
            if (!hasPlaceholder && !wasPlayed)
            {
                hasPlaceholder = true;
                GeneratePlaceholder();
                this.transform.SetParent(this.transform.parent.parent);
            }
            if (isDragging)
            {
                transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
                //Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
                //transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);
                foreach (Collider2D collider in colliders)
                {
                    Transform handCard = collider.transform;
                    if (handCard.CompareTag("Card"))
                    {
                        if (this.transform.position.x < handCard.position.x)
                        {
                            placeholder.transform.SetSiblingIndex(handCard.GetSiblingIndex());
                            break;
                        }
                    }
                }
            }
        }
    }

    private void OnMouseOver()
    {
        if (enabled)
        {
            
            if (inRewardScreen)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    ShowCardPopup();
                }
                if (Input.GetMouseButtonDown(0))
                {
                    CardRewardChosenEvent?.Invoke(card);
                }
            }
            if (!gm.isGamePauseActive)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    if (!wasPlayed && !card.wasPlayed)
                    {
                        ShowCardPopup();
                    }
                    if (wasPlayed && !card.wasPlayed)
                    {
                        activeCardSlot.HasCard(false);
                        SetNewParent(initialHandSlot);
                        //SetPosition(initialHandSlot);
                        gm.RefundCardCost(card);
                        card.UpdateEnergyCost();
                        card.UpdateDisplay();
                        wasPlayed = false;
                        //initialHandSlot.GetComponent<Slot>().HasCard(true);
                        Debug.Log(gameObject);
                        CardDropped?.Invoke();
                    }
                }
            }
        }
        
    }

    private void ShowCardPopup()
    {
        ShowCardPopupEvent?.Invoke();
        // Instantiate the card popup prefab
        GameObject cardPopup = Instantiate(cardPopupPrefab, transform.position, Quaternion.identity);

        // Set the card popup content (you can customize this based on your card's data)
        CardPopup popupScript = cardPopup.GetComponent<CardPopup>();
        if (popupScript != null)
        {
            popupScript.SetCardInfo(card.sienceInfo);
        }
    }

    private void OnMouseUp()
    {
        if (!gm.isGamePauseActive && enabled)
        {
            if (isDragging)
            {
                isDragging = false;

                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);

                foreach (Collider2D collider in colliders)
                {
                    Slot slot = collider.GetComponent<Slot>();

                    if (slot != null)
                    {                        
                        if (gm.EnoughEnergy(card))
                        {
                            if (card.ability)
                            {
                                HandleAbilityCard(slot);
                                if (wasPlayed)
                                {
                                    MoveToDiscardPile();
                                    gm.PayCardCost(card);
                                    return;
                                }
                                
                            }

                            if (!card.ability && !slot.hasCard && slot.CompareTag("ActiveCardSlot"))
                            {
                                activeCardSlot = slot;
                                activeCardSlot.HasCard(true);
                                SetNewParent(activeCardSlot.transform);
                                SetPosition(activeCardSlot.transform);
                                wasPlayed = true;
                                SetSortingOrder(transform.GetSiblingIndex());
                                gm.PayCardCost(card);
                                //initialHandSlot.GetComponent<Slot>().HasCard(false);
                            }

                        }
                        else
                        {
                            //transform.position = initialHandSlot.position;
                            Debug.Log("Slot Else is firing");
                            this.transform.SetParent(placeholder.transform.parent);
                            this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
                            SetSortingOrder(0);
                        }
                        CardDropped?.Invoke();
                    }
                }
            }
            
            //put card back to playerhand when not played on an active card slot
            if (!wasPlayed)
            {
                Debug.Log("Card was not played, back to playerhand");
                //transform.position = initialHandSlot.position;
                this.transform.SetParent(placeholder.transform.parent);
                this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
                //card.cost = card.cardInfo.cost;
                card.UpdateEnergyCost();
                card.UpdateDisplay();
                //gm.RefundCardCost(card);
            }

            SetSortingOrder(transform.GetSiblingIndex());
            if (hasPlaceholder)
            {
                hasPlaceholder = false;
                Destroy(placeholder);
            }
            if (wasPlayed)
            {
                gm.isFirstCardPlayed = true;
            }
        }

    }

    private void HandleAbilityCard(Slot slot)
    {
        switch (card.abilityTypes[0])
        {
            case GameConstants.abilityTargets.AbilityEnemy when slot.CompareTag("Enemy"):
                Debug.Log("I want to play that on an Enemy");
                wasPlayed = true;
                card.SetWasPlayed(true);
                OnEnemyEffect?.Invoke(card);
                break;

            case GameConstants.abilityTargets.AbilityPlayer when slot.CompareTag("Player"):
                Debug.Log("I want to play that on an Player");

                //Check if enough cards are in hand for Discard Effects
                
                
                if (card.cardEffects.ContainsKey(GameConstants.effectTypes.Discard))
                {
                    if (CheckCardsInHandForEffect() >= card.cardEffects.GetValueOrDefault(GameConstants.effectTypes.Discard, 0))
                    {
                        wasPlayed = true;
                        card.SetWasPlayed(true);
                        OnPlayerEffect?.Invoke(card);
                    }
                    else
                    {
                        Debug.Log("Not enough cards in hand to play this card");
                    }
                }
                else
                {
                    wasPlayed = true;
                    card.SetWasPlayed(true);
                    OnPlayerEffect?.Invoke(card);
                }
                break;


            case GameConstants.abilityTargets.AbilityShield when slot.hasCard:
                Debug.Log("I want to play that on a Shield");
                if (card.cardName == "Wiederverwertung")
                {
                    wasPlayed = true;
                    card.SetWasPlayed(true);
                    OnShieldEffect?.Invoke(card, slot);
                }else if (CanPlayCardOnShield(card, slot))
                {
                    if (card.cardEffects.ContainsKey(GameConstants.effectTypes.ShieldRepair) || card.cardEffects.ContainsKey(GameConstants.effectTypes.ShieldRepairPapier) || card.cardEffects.ContainsKey(GameConstants.effectTypes.ShieldRepairBlei) || card.cardEffects.ContainsKey(GameConstants.effectTypes.ShieldRepairAlu))
                    {
                        if(CanShieldRepair(card, slot))
                        {
                            wasPlayed = true;
                            card.SetWasPlayed(true);
                            OnShieldEffect?.Invoke(card, slot);
                        }
                        else
                        {
                            Debug.Log("Shield is not repairable!");
                            break;
                        }
                        
                    }
                    wasPlayed = true;
                    card.SetWasPlayed(true);
                    OnShieldEffect?.Invoke(card, slot);
                }
                break;

            default:
                this.transform.SetParent(placeholder.transform.parent);
                this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
                Debug.LogWarning("Default Ability Handling");
                break;
        }
        SetSortingOrder(transform.GetSiblingIndex());
        CardDropped?.Invoke();
        if (hasPlaceholder)
        {
            hasPlaceholder = false;
            Destroy(placeholder);
        }
    }
    private bool CanShieldRepair(Card card, Slot slot)
    {
        if (slot.GetCardInSlotInfo().durabilityCurrent < slot.GetCardInSlotInfo().durability)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private int CheckCardsInHandForEffect()
    {
        List<Card> cardsInHand = new List<Card>();
        cardsInHand.AddRange(gm.playerHand.GetComponentsInChildren<Card>());
        return cardsInHand.Count;
    }

    private bool CanPlayCardOnShield(Card card, Slot slot)
    {
        string slotCardName = slot.GetCardInSlotInfo().cardName;
        Debug.Log("Shield Card name: " + slotCardName);

        switch (card.cardName)
        {
            case "Klebestreifen":
            case "Feuerzeug":
                return slotCardName == "Papier" || slotCardName == "Dickes Papier" || slotCardName == "Verbundpapier";

            case "Schweißgerät":
            case "Bromlösung":
                return slotCardName == "Aluminiumfolie" || slotCardName == "Aluminiumblech" || slotCardName == "Aluminiumplatte";

            case "Lötlampe":
            case "Salpetersäure":
                return slotCardName == "Dünne Bleiplatte" || slotCardName == "Mittlere Bleiplatte" || slotCardName == "Dicke Bleiplatte";

            case "Recycle":
            case "Upcycling":
            case "Wiederverwertung":
            case "Bleibeschichtung":
            case "Alubeschichtung":
            case "Papierbeschichtung":
                return true;

            default:
                return false;
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    public void GeneratePlaceholder()
    {
        placeholder = new GameObject();
        placeholder.transform.SetParent(this.transform.parent);
        placeholder.transform.localScale = new Vector3(30.89397f, 30.89397f, 30.89397f);
        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;

        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
    }

    public void SetSortingOrder(int newSortingOrder)
    {
        
        if (isDragging)
        {
            sortingGroup.sortingOrder = 99;
        }
        if (!isDragging)
        {
            sortingGroup.sortingOrder = newSortingOrder;
        }

    }

    public void HandleEffect(Card card)
    {
        foreach (var entry in card.cardEffects)
        {
            switch (entry.Key)
            {
                case GameConstants.effectTypes.Discard:
                    MoveToDiscardPile();
                    Debug.Log("CardMovement Effect: " + entry.Key);
                    break;
            }
        }

    }

}