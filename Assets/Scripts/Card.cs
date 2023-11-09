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

    public enum ProtectionType
    {
        Alpha,
        Beta,
        Gamma
    }

    public ProtectionType cardProtectionType;

    public bool wasPlayed = false;

    GameManager gm;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        //cardProtectionType = (ProtectionType)Random.Range(0, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }




 
}
