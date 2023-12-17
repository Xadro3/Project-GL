using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyItem : MonoBehaviour
{
    Card card;

    private void OnCollisionEnter2D(Collision2D collision)
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
                    collision.transform.GetComponent<Drag>().startPos = Vector3.zero;
                    collision.transform.SetParent(GameObject.FindGameObjectWithTag("PlayerInventory").transform);
                    GameObject.FindGameObjectWithTag("Wallet").GetComponentInChildren<ShopCurrency>().RemoveMoney(card.currencyCost);
                    GameObject.FindGameObjectWithTag("Deck").GetComponent<Deck>().AddCardToDeck(card);
                }
              
            }
        }
    }
}
