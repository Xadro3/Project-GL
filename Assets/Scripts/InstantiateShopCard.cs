using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateShopCard : MonoBehaviour
{
    CardManager deck;
    GameObject instantiatedCard;
    // Start is called before the first frame update
    void Start()
    {
        for(int i =0; i < 9; i++)
        {
            deck = GameObject.FindGameObjectWithTag("Deck").GetComponent<CardManager>();
            Debug.Log(deck.name);
            instantiatedCard = deck.GetRandomCardFromCardSafe();
            Debug.Log(instantiatedCard.name);

            instantiatedCard.SetActive(true);
            instantiatedCard.transform.SetParent(transform);
            instantiatedCard.GetComponent<CardMovementHandler>().enabled = false;
            instantiatedCard.AddComponent<Drag>();
            instantiatedCard.layer = 0;
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
