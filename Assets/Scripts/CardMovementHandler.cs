using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class CardMovementHandler : MonoBehaviour
{
    public static event System.Action CardDropped;
    public event Action<CardMovementHandler> OnCardClicked;
    public static event Action<Card> OnPlayerEffect;
    public static event Action<Card> OnEnemyEffect;
    public static event Action<Card> OnShieldEffect;

    private int handIndex;

    public bool wasPlayed = false;
    public bool isDragging = false;

    private Vector3 offset;
    private Vector3 mousePosition;
    public Transform initialHandSlot;

    private GameObject placeholder = null;
    private bool hasPlaceholder = false;

    public Transform currentSlot = null;
    public Slot activeCardSlot = null;

    GameManager gm;
    SortingGroup sortingGroup;
    public Card card;

    private SpriteRenderer[] spriteRenderers;
    private MeshRenderer[] textRenderers;


    // Use this for initialization
    void Start()
    {
        //SetSortingOrder(99);
        gm = FindObjectOfType<GameManager>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        textRenderers = GetComponentsInChildren<MeshRenderer>();
        sortingGroup = GetComponent<SortingGroup>();
    }

    private void Awake()
    {
        
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
        if (!gm.isGamePauseActive)
        {
            mousePosition = Input.mousePosition - GetMouseWorldPos();
            if (!wasPlayed)
            {
                //offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                isDragging = true;
            }
            OnCardClicked?.Invoke(this);
            SetSortingOrder(transform.GetSiblingIndex());
        }
        
    }
    private void OnMouseDrag()
    {
        if (!gm.isGamePauseActive)
        {
            if (!hasPlaceholder)
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
        if (!gm.isGamePauseActive)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (wasPlayed && !card.wasPlayed)
                {
                    activeCardSlot.HasCard(false);
                    SetNewParent(initialHandSlot);
                    //SetPosition(initialHandSlot);
                    gm.RefundCardCost(card);
                    wasPlayed = false;
                    //initialHandSlot.GetComponent<Slot>().HasCard(true);
                    Debug.Log(gameObject);
                }
            }
        }
    }

    private void OnMouseUp()
    {
        if (!gm.isGamePauseActive)
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
                        if (gm.PayCardCost(card))
                        {
                            if (card.ability)
                            {
                                HandleAbilityCard(slot);
                                if (wasPlayed)
                                {
                                    MoveToDiscardPile();
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
                                CardDropped();
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
                //gm.RefundCardCost(card);
            }

            SetSortingOrder(transform.GetSiblingIndex());
            if (hasPlaceholder)
            {
                hasPlaceholder = false;
                Destroy(placeholder);
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
                wasPlayed = true;
                card.SetWasPlayed(true);
                OnPlayerEffect?.Invoke(card);
                break;

            case GameConstants.abilityTargets.AbilityShield when slot.hasCard:
                Debug.Log("I want to play that on a Shield");
                if (CanPlayCardOnShield(card, slot))
                {
                    wasPlayed = true;
                    card.SetWasPlayed(true);
                    OnShieldEffect?.Invoke(card);
                }
                break;

            default:
                this.transform.SetParent(placeholder.transform.parent);
                this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
                gm.RefundCardCost(card);
                Debug.LogWarning("Default Ability Handling");
                break;
        }
        SetSortingOrder(transform.GetSiblingIndex());
        if (hasPlaceholder)
        {
            hasPlaceholder = false;
            Destroy(placeholder);
        }
        CardDropped();
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

            case "Aluschweißgerät":
            case "Bromlösung":
                return slotCardName == "Aluminiumfolie" || slotCardName == "Aluminiumblech" || slotCardName == "Aluminiumplatte";

            case "Bleischweißgerät":
            case "Salpetersäure":
                return slotCardName == "Dünne Bleiplatte" || slotCardName == "Mittlere Bleiplatte" || slotCardName == "Dicke Bleiplatte";

            case "Recycle":
            case "Upcycling":
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