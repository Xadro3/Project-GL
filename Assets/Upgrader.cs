using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrader : MonoBehaviour
{

    Deck deck;
    bool used;
    private void Start()
    {
        WorkshopManager.FoundUpgradedCardEvent += HandleCardSwap;

    }

    private void HandleCardSwap(GameObject upgradedCard)
    {
        upgradedCard.GetComponent<Drag>().startPos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name);
        if (collision.transform.tag == "Card"&& !used)
        {
            used = true;
            collision.collider.GetComponent<Drag>().startPos = transform.position;
            collision.collider.GetComponent<Card>().UpgradeCard();            
        }
    }
}