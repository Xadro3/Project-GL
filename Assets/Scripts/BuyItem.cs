using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyItem : MonoBehaviour
{
    Card card;
    GridLayout grid;
    private void Start()
    {
        grid = GetComponent<GridLayout>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        if (collision.transform.tag == "Card") 
        {
            card = collision.transform.gameObject.GetComponent<Card>();
            if (!card.isBought)
            {
                if(GameObject.FindGameObjectWithTag("Wallet").GetComponentInChildren<ShopCurrency>().money-card.currencyCost > 0)
                {
                    card.isBought = true;
                    
                    collision.transform.GetComponent<Drag>().startPos = GameObject.FindGameObjectWithTag("PlayerInventory").transform.position;
                    collision.transform.SetParent(GameObject.FindGameObjectWithTag("PlayerInventory").transform);
                    GameObject.FindGameObjectWithTag("Wallet").GetComponentInChildren<ShopCurrency>().RemoveMoney(card.currencyCost);
                    GameObject.FindGameObjectWithTag("Deck").GetComponent<Deck>().AddCardToPlayerDeck(card);
                    
                }
              
            }
        }
        Canvas.ForceUpdateCanvases();
    }
}
