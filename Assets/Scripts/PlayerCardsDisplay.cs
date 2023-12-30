using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCardsDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    Deck deck;
    List<GameObject> instantiatedCards;
    void Start()
    {
        deck = GameObject.FindGameObjectWithTag("Deck2").GetComponent<Deck>();


        //Debug.Log(deck.name);
            instantiatedCards = deck.GetPlayerDeck();
            //Debug.Log(instantiatedCard.name);
        foreach(GameObject instantiatedCard in instantiatedCards)
        {
            instantiatedCard.SetActive(true);
            instantiatedCard.transform.SetParent(transform);
            instantiatedCard.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            instantiatedCard.transform.localScale = new Vector3(30f, 30f, 30f);
            instantiatedCard.GetComponent<CardMovementHandler>().enabled = false;
            instantiatedCard.AddComponent<Drag>();
            instantiatedCard.layer = 0;
        }
            
        
    }

}
