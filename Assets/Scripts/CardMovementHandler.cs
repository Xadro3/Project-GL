using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class CardMovementHandler : MonoBehaviour
{

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
                                break;
                            }

                            if (!card.ability && !slot.hasCard && slot.CompareTag("ActiveCardSlot"))
                            {
                                activeCardSlot = slot;
                                activeCardSlot.HasCard(true);
                                SetNewParent(activeCardSlot.transform);
                                SetPosition(activeCardSlot.transform);
                                wasPlayed = true;
                                SetSortingOrder(transform.GetSiblingIndex());
                                //initialHandSlot.GetComponent<Slot>().HasCard(false);
                            }

                        }
                        else
                        {
                            //transform.position = initialHandSlot.position;
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
                break;

            case GameConstants.abilityTargets.AbilityPlayer when slot.CompareTag("Player"):
                Debug.Log("I want to play that on an Player");
                wasPlayed = true;
                card.SetWasPlayed(true);
                break;

            case GameConstants.abilityTargets.AbilityShield when slot.hasCard:
                Debug.Log("I want to play that on a Shield");
                wasPlayed = true;
                card.SetWasPlayed(true);
                slot.HandleShieldAbility(card);
                break;

            default:
                this.transform.SetParent(placeholder.transform.parent);
                this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
                gm.RefundCardCost(card);
                break;
        }

        if (wasPlayed)
        {
            foreach (var entry in card.cardEffects)
            {
                HandleEffect(entry.Key, entry.Value);
            }
            MoveToDiscardPile();
        }

        if (hasPlaceholder)
        {
            hasPlaceholder = false;
            Destroy(placeholder);
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

    private void HandleEffect(GameConstants.effectTypes effectType, int value)
    {
        switch (effectType)
        {
            case GameConstants.effectTypes.DamageReductionFlat:
                CardEffectEventHandler.TriggerDamageReductionFlat(value);
                Debug.Log("Trigger Effect: " + effectType);
                break;
            case GameConstants.effectTypes.DamageReductionPercent:
                CardEffectEventHandler.TriggerDamageReductionPercent(value);
                Debug.Log("Trigger Effect: " + effectType);
                break;
            case GameConstants.effectTypes.RadiationReductionFlat:
                CardEffectEventHandler.TriggerRadiationReductionFlat(value);
                Debug.Log("Trigger Effect: " + effectType);
                break;
            case GameConstants.effectTypes.RadiationReductionPercent:
                CardEffectEventHandler.TriggerRadiationReductionPercent(value);
                Debug.Log("Trigger Effect: " + effectType);
                break;
            case GameConstants.effectTypes.RadiationBlock:
                CardEffectEventHandler.TriggerRadiationBlock(value);
                Debug.Log("Trigger Effect: " + effectType);
                break;
            case GameConstants.effectTypes.RadiationImmunity:
                CardEffectEventHandler.TriggerRadiationImmunity(value);
                Debug.Log("Trigger Effect: " + effectType);
                break;
            case GameConstants.effectTypes.RadiationOrderChange:
                CardEffectEventHandler.TriggerRadiationOrderChange(value);
                Debug.Log("Trigger Effect: " + effectType);
                break;
            case GameConstants.effectTypes.ShieldRepair:
                CardEffectEventHandler.TriggerShieldRepair(value);
                Debug.Log("Trigger Effect: " + effectType);
                break;
            case GameConstants.effectTypes.ShieldBuff:
                CardEffectEventHandler.TriggerShieldBuff(value);
                Debug.Log("Trigger Effect: " + effectType);
                break;
            case GameConstants.effectTypes.ShieldDissolve:
                CardEffectEventHandler.TriggerShieldDissolve(value);
                Debug.Log("Trigger Effect: " + effectType);
                break;
            case GameConstants.effectTypes.ResistanceReductionFlat:
                CardEffectEventHandler.TriggerResistanceReductionFlat(value);
                Debug.Log("Trigger Effect: " + effectType);
                break;
            case GameConstants.effectTypes.ResistanceReductionPercent:
                CardEffectEventHandler.TriggerResistanceReductionPercent(value);
                Debug.Log("Trigger Effect: " + effectType);
                break;
            case GameConstants.effectTypes.ResistanceEffectReduction:
                CardEffectEventHandler.TriggerResistanceEffectReduction(value);
                Debug.Log("Trigger Effect: " + effectType);
                break;
            case GameConstants.effectTypes.PlayerHealFlat:
                CardEffectEventHandler.TriggerPlayerHealFlat(value);
                Debug.Log("Trigger Effect: " + effectType);
                break;
            case GameConstants.effectTypes.PlayerHealPercent:
                CardEffectEventHandler.TriggerPlayerHealPercent(value);
                Debug.Log("Trigger Effect: " + effectType);
                break;
            case GameConstants.effectTypes.TimerReductionFlat:
                CardEffectEventHandler.TriggerTimerReductionFlat(value);
                Debug.Log("Trigger Effect: " + effectType);
                break;
            case GameConstants.effectTypes.DrawCard:
                CardEffectEventHandler.TriggerDrawCard(value);
                Debug.Log("Trigger Effect: " + effectType);
                break;
            case GameConstants.effectTypes.Discard:
                CardEffectEventHandler.TriggerDiscard(value);
                Debug.Log("Trigger Effect: " + effectType);
                break;
            case GameConstants.effectTypes.EnergyGet:
                CardEffectEventHandler.TriggerEnergyGet(value);
                Debug.Log("Trigger Effect: " + effectType);
                break;
            case GameConstants.effectTypes.EnergyCostReduction:
                CardEffectEventHandler.TriggerEnergyCostReduction(value);
                Debug.Log("Trigger Effect: " + effectType);
                break;

        }
    }

}