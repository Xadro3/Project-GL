using UnityEngine;
using System;

public class TurnMaster : MonoBehaviour
{
    GameManager gm;

    public int savedDamageValue;
    public string savedDamageType = "";
    

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }


    public void ResolveTurn(Enemy[] wagons, Slot[] activeCardSlots)
    {
        foreach (Enemy wagon in wagons)
        {
            savedDamageValue = wagon.damageValue;
            savedDamageType = wagon.damageType.ToString();
            
            foreach (Slot activeCardSlot in activeCardSlots)
            {
                if (activeCardSlot.hasCard)
                {
                    Card card = activeCardSlot.GetComponentInChildren<Card>();
                    bool isAffected = false;
                    foreach (Card.ProtectionType protectionType in card.protectionTypes)
                    {
                        if (protectionType.ToString() == savedDamageType)
                        {
                            isAffected = true;
                            break;
                        }
                    }
                    if (isAffected)
                    {
                        savedDamageValue = card.AdjustDurability(savedDamageValue);

                        if (savedDamageValue == 0)
                        {
                            break;
                        }
                        else
                        {
                            gm.PlayerDamage(savedDamageValue, savedDamageType);
                        }
                    }
                }
                
            }
        }
    }
}
