using System;
using System.Collections.Generic;
using UnityEngine;

public class TurnMaster : MonoBehaviour
{
    GameManager gm;

    public int savedDamageValue;
    public List<string> savedDamageTypes;
    

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }


    public void ResolveTurn(Enemy[] wagons, Slot[] activeCardSlots)
    {
        //go through each wagon one by one
        foreach (Enemy wagon in wagons)
        {
            int typePosition = 0;
            //save damage of wagon and damageType
            savedDamageValue = wagon.damageValue;
            //save all damage types the wagon does.
            foreach (GameConstants.radiationTypes radiationType in wagon.damageTypes)
            {
                savedDamageTypes.Add(radiationType.ToString());
            }
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
                    card.SetWasPlayed(true);
                    //see if wagon damage type affects card protection type
                    foreach (GameConstants.radiationTypes radiationType in card.protectionTypes)
                    {
                        for (int i = 0; i < savedDamageTypes.Count; i++)
                        {
                            if(radiationType.ToString() == savedDamageTypes[i])
                            {
                                isAffected = true;
                                typePosition = i;
                                break;
                            }
                        }
                        break;
                    }
                    //if card is affected by wagon damage type, calculate damage
                    if (isAffected)
                    {
                        savedDamageValue = card.AdjustDurability(savedDamageValue);
                        card.UpdateDisplay();

                        if (savedDamageValue == 0)
                        {
                            break;
                        }
                        //if remaining damage after all cards is > 0 then do damage to player
                        else
                        {
                            gm.PlayerDamage(savedDamageValue, savedDamageTypes[typePosition].ToString());
                        }
                    }
                }
            }
            //if no card is found in any slot damage gets directly to the player
            if (!foundCard)
            {
                gm.PlayerDamage(savedDamageValue, savedDamageTypes[typePosition].ToString());
            }
        }
    }
}
