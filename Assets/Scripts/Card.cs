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
    private Transform initialPosition;

    private Transform playerHandSlot;
    private Slot activeCardSlot;

     GameManager gm;
    
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        initialPosition = transform;

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
            
                if (slot != null && slot.GetComponent<Collider>().CompareTag("ActiveCardSlot") && !slot.hasCard)
                {
                    GameManager.AssignToActiveSlot(this, slot);
                    //GameManager.PayCardCost(cardCost);
                    activeCardSlot = slot;
                    wasPlayed = true;

                    return;
                }
            }
        }

        if (!wasPlayed)
        {
            transform.position = initialPosition.position;
            Debug.Log("I am getting triggered!");
        }
    }



}
