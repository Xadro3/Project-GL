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
    private List<Card> cardsInPlay = new List<Card>();

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
        foreach (Slot activeCardSlot in activeCardSlots)
        {
            if (activeCardSlot.hasCard)
            {
                Card card = activeCardSlot.GetComponentInChildren<Card>();
                cardsInPlay.Add(card);
                card.SetWasPlayed(true);
                card.OnDurabilityZero += HandleCardDurabilityZero;
            }
        }

        foreach (Enemy wagon in wagons)
        {

            Dictionary<string, int> wagonDamageStats = new Dictionary<string, int>(damageStats);

            foreach (Card card in cardsInPlay)
            {
                bool isAffected = false;
                // Check if wagon damage type affects card protection type
                foreach (GameConstants.radiationTypes radiationType in card.protectionTypes)
                {
                    if (wagonDamageStats.ContainsKey(radiationType.ToString()))
                    {
                        isAffected = true;
                        int damageValue = wagonDamageStats[radiationType.ToString()];
                        Debug.Log(wagon.name + " will deal: " + wagonDamageStats[radiationType.ToString()] + " of " + radiationType.ToString());
                        wagonDamageStats[radiationType.ToString()] = card.AdjustDurability(damageValue);
                        card.UpdateDisplay();

                        if (wagonDamageStats[radiationType.ToString()] < 0)
                        {
                            Debug.Log("I killed a card with overkill damage. " + "Damage type: " + radiationType.ToString() + " Damage left: " + wagonDamageStats[radiationType.ToString()]);
                            wagonDamageStats[radiationType.ToString()] = Mathf.Abs(wagonDamageStats[radiationType.ToString()]);
                            break;
                        }
                        if (wagonDamageStats[radiationType.ToString()] >= 0)
                        {
                            Debug.Log("No damage value left of damage type: " + radiationType.ToString());
                            wagonDamageStats[radiationType.ToString()] = 0;
                        }
                    }
                }
                card.OnDurabilityZero -= HandleCardDurabilityZero;
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



    private void HandleCardDurabilityZero(Card card)
    {
        // Handle the special effect when a cards durability reaches zero

        if (card.effect)
        {
            for (int i = 0; i < card.effectTypes.Count; i++)
            {
                switch (card.effectTypes[i])
                {
                    case GameConstants.effectTypes.BruchTimer:
                        break;

                    case GameConstants.effectTypes.BruchBuff:
                        BuffCardsBehind(card);
                        break;

                    case GameConstants.effectTypes.AbwerfenPlayer:
                        break;

                    case GameConstants.effectTypes.AbwerfenEnemy:
                        break;

                    case GameConstants.effectTypes.Schichtung:
                        break;

                    case GameConstants.effectTypes.Robust:
                        break;

                    case GameConstants.effectTypes.AbilityPlayer:
                        break;

                    case GameConstants.effectTypes.AbilityEnemy:
                        break;

                    case GameConstants.effectTypes.Immunity:
                        break;
                }
            }
        }
    }

    private void BuffCardsBehind(Card sourceCard)
    {

        // Identify the index of the sourceCard in the cardsInPlay list
        int sourceIndex = cardsInPlay.IndexOf(sourceCard);

        // Apply buff to cards behind the sourceCard
        for (int i = sourceIndex + 1; i <= Mathf.Min(sourceIndex + 3, cardsInPlay.Count - 1); i++)
        {
            cardsInPlay[i].Buff();
        }
    }
}