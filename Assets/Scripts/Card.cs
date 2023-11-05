using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    // To-Do:
    // -Move card movement to CardDragHandler

    public bool wasPlayed = false;
    public int handIndex;
    public int cardCost = 1;

    public bool isDragging = false;
    
    private Vector3 offset;
    public Transform initialDaddy;

    private Transform playerHandSlot;
    private Slot activeCardSlot;

    GameManager gm;
    
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MoveToDiscardPile()
    {
        gm.discardPile.Add(this);
        gameObject.SetActive(false);
    }

    public void SetParent(Transform parent)
    {
        playerHandSlot = parent.transform;
    }

    //Logic 
    //if card is being 
    //dragged/dropped 
    //and stuff
    //will be moved to an extra script
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
            GameManager.AssignToPlayerHand(this, playerHandSlot, activeCardSlot);
            gm.RefundCardCost(cardCost);
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
                    GameManager.AssignToActiveSlot(this, slot);
                    gm.PayCardCost(cardCost);
                    activeCardSlot = slot;
                    wasPlayed = true;

                    return;
                }
            }
        }
        //put card back to playerhand when not played on an active card slot
        if (!wasPlayed)
        {
            transform.position = initialDaddy.position;
        }
    }



}
