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

    public Transform currentSlot;
    public Slot activeCardSlot;

    GameManager gm;
    SortingGroup sortingGroup;
    Card card;

    private SpriteRenderer[] spriteRenderers;
    private MeshRenderer[] textRenderers;


    // Use this for initialization
    void Start()
    {
        SetSortingOrder(99);
    }

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        card = GetComponent<Card>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        textRenderers = GetComponentsInChildren<MeshRenderer>();
        sortingGroup = GetComponent<SortingGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sortingGroup.sortingOrder != transform.GetSiblingIndex() && !isDragging) 
        {
            SetSortingOrder(transform.GetSiblingIndex());
        }
        
    }

    public void DrawCardSetup(int handIndex, Transform parent)
    {
        SetHandIndex(handIndex);
        SetNewParent(parent);
        SetPosition(parent);
        SetInitialHandslot(parent);
        card.SetActive(true);
    }

    private void SetInitialHandslot(Transform newInitialHandslot)
    {
        initialHandSlot = newInitialHandslot;
    }

    public void MoveToDiscardPile()
    {
        gm.discardPile.Add(card);
        activeCardSlot.HasCard(false);
        SetNewParent(gm.discardPileParent);
        SetPosition(gm.discardPileParent);
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

    private void SetHandIndex(int newHandIndex)
    {
        handIndex = newHandIndex;
    }


    //Mouse movement with card
    private void OnMouseDown()
    {
        if (!gm.isPauseMenuActive)
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
        if (!gm.isPauseMenuActive)
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
        if (!gm.isPauseMenuActive)
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
        if (!gm.isPauseMenuActive)
        {
            if (isDragging)
            {
                isDragging = false;

                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);

                foreach (Collider2D collider in colliders)
                {
                    Slot slot = collider.GetComponent<Slot>();

                    if (slot != null && slot.CompareTag("ActiveCardSlot") && !slot.hasCard)
                    {
                        if (gm.PayCardCost(card))
                        {
                            activeCardSlot = slot;
                            activeCardSlot.HasCard(true);
                            SetNewParent(activeCardSlot.transform);
                            SetPosition(activeCardSlot.transform);
                            wasPlayed = true;
                            SetSortingOrder(transform.GetSiblingIndex());
                            //initialHandSlot.GetComponent<Slot>().HasCard(false);
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
            }

            SetSortingOrder(transform.GetSiblingIndex());
            if (hasPlaceholder)
            {
                hasPlaceholder = false;
                Destroy(placeholder);
            }
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

}