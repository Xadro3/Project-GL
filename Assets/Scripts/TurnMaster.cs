using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnMaster : MonoBehaviour
{
    GameManager gm;

    public int savedDamageValue;
    public List<string> savedDamageTypes;
    public Dictionary<GameConstants.radiationTypes, int> damageStats = new Dictionary<GameConstants.radiationTypes, int>();
    private List<Card> cardsInPlay = new List<Card>();

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void SetDamage(Dictionary<GameConstants.radiationTypes, int> damageStats)
    {
        this.damageStats = damageStats.ToDictionary(entry => entry.Key, entry => entry.Value);
    }


    public void ResolveTurn(List<Enemy> wagons, List<Slot> activeCardSlots)
    {
        cardsInPlay.Clear();
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

            Dictionary<GameConstants.radiationTypes, int> wagonDamageStats = new Dictionary<GameConstants.radiationTypes, int>(damageStats);

            foreach (Card card in cardsInPlay)
            {
                bool isAffected = false;
                // Check if wagon damage type affects card protection type
                foreach (GameConstants.radiationTypes radiationType in card.protectionTypes)
                {
                    if (wagonDamageStats.ContainsKey(radiationType))
                    {
                        isAffected = true;
                        int damageValue = wagonDamageStats[radiationType];
                        Debug.Log(wagon.name + " will deal: " + wagonDamageStats[radiationType] + " of " + radiationType);
                        wagonDamageStats[radiationType] = card.AdjustDurability(damageValue);
                        card.UpdateDisplay();

                        if (wagonDamageStats[radiationType] < 0)
                        {
                            Debug.Log("I killed a card with overkill damage. " + "Damage type: " + radiationType + " Damage left: " + wagonDamageStats[radiationType]);
                            wagonDamageStats[radiationType] = Mathf.Abs(wagonDamageStats[radiationType]);
                            break;
                        }
                        if (wagonDamageStats[radiationType] >= 0)
                        {
                            Debug.Log("No damage value left of damage type: " + radiationType);
                            wagonDamageStats[radiationType] = 0;
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
        if (gm.IsBetaDotActive())
        {
            gm.PlayerBetaDotDamage();
        }
    }



    private void HandleCardDurabilityZero(Card card)
    {
        // Handle the special effect when a cards durability reaches zero

        if (card.effect && card.onBruch)
        {
            foreach (var entry in card.cardEffects)
            {
                switch (entry.Key)
                {
                    case GameConstants.effectTypes.TimerReductionFlat:
                        break;

                    case GameConstants.effectTypes.Discard:
                        //BuffCardsBehind(card);
                        break;
                }
            }
        }
        if (card.entsorgen)
        {
            Destroy(card.gameObject);
        }
    }

    private void BuffCardsBehind(Card sourceCard)
    {

        // Identify the index of the sourceCard in the cardsInPlay list
        int sourceIndex = cardsInPlay.IndexOf(sourceCard);

        // Apply buff to cards behind the sourceCard
        for (int i = sourceIndex + 1; i <= Mathf.Min(sourceIndex + 3, cardsInPlay.Count - 1); i++)
        {
            cardsInPlay[i].BruchBuff();
        }
    }
}