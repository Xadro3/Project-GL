using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCardsDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    CardManager deck;
    GameObject instantiatedCard;
    void Start()
    {
        deck = GameObject.FindGameObjectWithTag("Deck").GetComponent<CardManager>();
        for (int i = 0; i <= deck.deck.playerDeck.Count;i++)
        {
            
            //Debug.Log(deck.name);
            instantiatedCard = deck.GetRandomCardFromCardSafe();
            //Debug.Log(instantiatedCard.name);

            instantiatedCard.SetActive(true);
            instantiatedCard.transform.SetParent(transform);
            instantiatedCard.GetComponent<CardMovementHandler>().enabled = false;
            instantiatedCard.AddComponent<Drag>();
            instantiatedCard.layer = 0;
        }
    }

}
