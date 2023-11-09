using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    // To-Do:
    // -Move card movement to CardDragHandler

    public int cost;
    public int durability;
    public int repair;

    public int cardCost = 1;
    public bool wasPlayed = false;

    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


 
}
