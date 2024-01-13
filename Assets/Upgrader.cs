using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrader : MonoBehaviour
{

    Deck deck;
    bool used;
    private void Start()
    {
        
    }
    private void OnEnable()
    {
        WorkshopManager.FoundUpgradedCardEvent += HandleCardSwap;
    }
    private void OnDisable()
    {
        WorkshopManager.FoundUpgradedCardEvent -= HandleCardSwap;
    }

    private void HandleCardSwap(GameObject upgradedCard)
    {
        Debug.Log("Swap Handle Fired!");
        upgradedCard.GetComponent<Drag>().startPos = transform.position;
        upgradedCard.GetComponent<LayoutElement>().ignoreLayout = true;
        upgradedCard.transform.position = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name);
        if (collision.transform.tag == "Card"&& !used)
        {
            used = true;
            collision.collider.GetComponent<Drag>().startPos = transform.position;
            collision.collider.GetComponent<Card>().UpgradeCard();
            UpgradeCard?.Invoke();
        }
    }

    public static System.Action UpgradeCard;
    
}