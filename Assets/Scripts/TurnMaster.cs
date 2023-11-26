using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnMaster : MonoBehaviour
{
    GameManager gm;

    public int savedDamageValue;
    public List<string> savedDamageTypes;
    public Dictionary<string, int> damageStats = new Dictionary<string, int>();
    

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void SetDamage(Dictionary<string, int> damageStats)
    {
        this.damageStats = damageStats.ToDictionary(entry => entry.Key, entry => entry.Value);
    }


    public void ResolveTurn(List<Enemy> wagons, List<Slot> activeCardSlots)
    {
        foreach (Enemy wagon in wagons)
        {
            
            Dictionary<string, int> wagonDamageStats = new Dictionary<string, int>(damageStats);

            bool foundCard = false;

            foreach (Slot activeCardSlot in activeCardSlots)
            {
                if (activeCardSlot.hasCard)
                {
                    foundCard = true;
                    Card card = activeCardSlot.GetComponentInChildren<Card>();
                    bool isAffected = false;
                    Debug.Log(card.gameObject.name + " wurde gefunden");
                    card.SetWasPlayed(true);

                    // Check if wagon damage type affects card protection type
                    foreach (GameConstants.radiationTypes radiationType in card.protectionTypes)
                    {
                        if (wagonDamageStats.ContainsKey(radiationType.ToString()))
                        {
                            isAffected = true;
                            int damageValue = wagonDamageStats[radiationType.ToString()];
                            wagonDamageStats[radiationType.ToString()] = card.AdjustDurability(damageValue);
                            card.UpdateDisplay();

                            if (wagonDamageStats[radiationType.ToString()] == 0)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            // If no card is found in any slot, damage goes directly to the player
            foreach (var entry in wagonDamageStats)
            {
                if (entry.Value != 0)
                {
                    gm.PlayerDamage(entry.Value, entry.Key);
                }
            }
        }
    }


    /**
    public void ResolveTurn(List<Enemy> wagons, List<Slot> activeCardSlots)
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
                    }
                }
            }
            //if no card is found in any slot damage gets directly to the player
            if (savedDamageValue != 0)
            {
                gm.PlayerDamage(savedDamageValue, savedDamageTypes[typePosition].ToString());
            }
        }
    }
    **/
}
