using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
                    
                    //collision.transform.GetComponent<Drag>().startPos = GameObject.FindGameObjectWithTag("PlayerInventory").transform.position;
                    collision.transform.SetParent(GameObject.FindGameObjectWithTag("PlayerInventory").transform);
                    collision.transform.SetAsFirstSibling();
                    card.GetComponent<Drag>().bought = true;
                    GameObject.FindGameObjectWithTag("Wallet").GetComponentInChildren<ShopCurrency>().RemoveMoney(card.currencyCost);
                    GameObject.FindGameObjectWithTag("Deck2").GetComponent<Deck>().AddCardToBaseDeck(card);
                    LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
                    Buycard?.Invoke();
                }
              
            }
        }
        Canvas.ForceUpdateCanvases();
    }

    public static System.Action Buycard;

}
