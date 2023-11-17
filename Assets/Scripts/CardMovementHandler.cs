using System;
using System.Collections;
using UnityEngine;

public class CardMovementHandler : MonoBehaviour
{

    private int handIndex;

    public bool wasPlayed = false;
    public bool isDragging = false;

    private Vector3 offset;
    private Vector3 mousePosition;
    public Transform initialHandSlot;

    public Transform currentSlot;
    public Slot activeCardSlot;

    GameManager gm;

    Card card;

    private SpriteRenderer[] spriteRenderers;
    private MeshRenderer[] textRenderers;


    // Use this for initialization
    void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        textRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        card = GetComponent<Card>();
    }

    // Update is called once per frame
    void Update()
    {

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
        Debug.Log(gameObject + " set my parent as: " + currentSlot);
    }

    private void SetPosition(Transform newPosition)
    {
        transform.position = newPosition.position;
    }

    private void SetHandIndex(int newHandIndex)
    {
        handIndex = newHandIndex;
    }


    //Mouse movement with card
    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition - GetMouseWorldPos();
        Debug.Log("MouseDown");
        if (!wasPlayed)
        {
            //offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
        }

        SetSortingOrder(99);
    }
    private void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
            //Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            //transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (wasPlayed && !card.wasPlayed)
            {
                activeCardSlot.HasCard(false);
                SetNewParent(initialHandSlot);
                SetPosition(initialHandSlot);
                gm.RefundCardCost(card);
                wasPlayed = false;
                initialHandSlot.GetComponent<Slot>().HasCard(true);
                Debug.Log(gameObject);
            }
            
        }
    }

    private void OnMouseUp()
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
                        initialHandSlot.GetComponent<Slot>().HasCard(false);
                    }
                    else
                    {
                        transform.position = initialHandSlot.position;
                    }
                    
                }
            }
        }
        //put card back to playerhand when not played on an active card slot
        if (!wasPlayed)
        {
            transform.position = initialHandSlot.position;
        }

        SetSortingOrder(-99);

    }

    private Vector3 GetMouseWorldPos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    void SetSortingOrder(int newSortingOrder)
    {
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.sortingOrder += newSortingOrder;
        }
        foreach (MeshRenderer renderer in textRenderers)
        {
            renderer.sortingOrder += newSortingOrder;
        }

        //GetComponent<SpriteRenderer>().sortingOrder += newSortingOrder;
    }

}