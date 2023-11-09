using System;
using System.Collections;
using UnityEngine;

public class CardMovementHandler : MonoBehaviour
{

    private int handIndex;

    private bool wasPlayed = false;
    private bool isDragging = false;

    private Vector3 offset;
    private Transform initialHandSlot;

    private Transform playerHandSlot;
    private Slot activeCardSlot;

    GameManager gm;

    Card card;


    // Use this for initialization
    void Start()
    {
        
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
        SetParent(parent);
        SetPosition(parent);
        SetInitialHandslot(parent);
    }

    private void SetInitialHandslot(Transform newInitialHandslot)
    {
        initialHandSlot = newInitialHandslot;
    }

    void MoveToDiscardPile()
    {
        gm.discardPile.Add(card);
        gameObject.SetActive(false);
    }

    private void SetParent(Transform parent)
    {
        playerHandSlot = parent.transform;
        transform.parent = playerHandSlot;
        Debug.Log(card.name + "New parent: "+ parent);
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
        if (!wasPlayed)
        {
            offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
        }
    }
    private void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            activeCardSlot.HasCard(false);
            SetParent(initialHandSlot);
            SetPosition(initialHandSlot);
            gm.RefundCardCost(card);
            wasPlayed = false;
            Debug.Log(gameObject);
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
                    activeCardSlot = slot;
                    activeCardSlot.HasCard(true);
                    SetParent(activeCardSlot.transform);
                    SetPosition(activeCardSlot.transform);
                    wasPlayed = true;
                    gm.PayCardCost(card);

                    return;
                }
            }
        }
        //put card back to playerhand when not played on an active card slot
        if (!wasPlayed)
        {
            transform.position = initialHandSlot.position;
        }
    }

}