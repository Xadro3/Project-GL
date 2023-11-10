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
        //go through each wagon one by one
        foreach (Enemy wagon in wagons)
        {
            //save damage of wagon and damageType
            savedDamageValue = wagon.damageValue;
            savedDamageType = wagon.damageType.ToString();
            bool foundCard = false;
            //go through all slots one by one with the saved damage/type
            foreach (Slot activeCardSlot in activeCardSlots)
            {
                //if slot has card do stuff
                if (activeCardSlot.hasCard)
                {
                    foundCard = true;
                    Card card = activeCardSlot.GetComponentInChildren<Card>();
                    bool isAffected = false;
                    //see if wagon damage type affects card protection type
                    foreach (Card.ProtectionType protectionType in card.protectionTypes)
                    {
                        if (protectionType.ToString() == savedDamageType)
                        {
                            isAffected = true;
                            break;
                        }
                    }
                    //if card is affected by wagon damage type, calculate damage
                    if (isAffected)
                    {
                        savedDamageValue = card.AdjustDurability(savedDamageValue);

                        if (savedDamageValue == 0)
                        {
                            break;
                        }
                        //if remaining damage after all cards is > 0 then do damage to player
                        else
                        {
                            gm.PlayerDamage(savedDamageValue, savedDamageType);
                        }
                    }
                }             
            }
            if (!foundCard)
            {
                gm.PlayerDamage(savedDamageValue, savedDamageType);
            }
        }
    }
}
