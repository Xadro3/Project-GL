using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrader : MonoBehaviour
{
    Deck deck;
    bool used;
    private void Start()
    {
        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name);
        if (collision.transform.tag == "Card"&& !used)
        {
            collision.collider.GetComponent<Drag>().startPos = transform.position;
            collision.collider.GetComponent<Card>().UpgradeCard();
            used = true;
        }
    }
}