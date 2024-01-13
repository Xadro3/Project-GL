using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seller : MonoBehaviour
{
    Deck deck;
    bool used;
    ShopCurrency shop;
    // Start is called before the first frame update
    void Start()
    {
        deck = GameObject.FindGameObjectWithTag("Deck").GetComponent<Deck>();
        shop = GameObject.FindGameObjectWithTag("Wallet").GetComponentInChildren<ShopCurrency>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name);
        if (collision.transform.tag == "Card"&& !used)
        {
            shop.AddMoney(collision.gameObject.GetComponent<Card>().currencyCost);
            collision.collider.GetComponent<Drag>().startPos = transform.position;
            deck.RemoveCardFromBaseDeck(collision.gameObject.GetComponent<Card>());
            this.gameObject.SetActive(false);
            used = true;
            SellCard?.Invoke();
        }
    }

    public static System.Action SellCard;
}
