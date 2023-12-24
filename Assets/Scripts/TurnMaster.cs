using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnMaster : MonoBehaviour
{
    GameManager gm;

    public int savedDamageValue;
    public List<string> savedDamageTypes;
    public Dictionary<GameConstants.radiationTypes, int> damageStats = new Dictionary<GameConstants.radiationTypes, int>();
    private List<Card> cardsInPlay = new List<Card>();
    private ButtonRotate endTurnButton = null;

    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Encounter")
        {
            endTurnButton = FindObjectOfType<ButtonRotate>();
        }
    }

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

        StartCoroutine(ProcessCardsWithDelay(wagons));
    }

    private IEnumerator ProcessCardsWithDelay(List<Enemy> wagons)
    {
        Dictionary<GameConstants.radiationTypes, int> wagonDamageStats = new Dictionary<GameConstants.radiationTypes, int>(damageStats);
        
        foreach (Card card in cardsInPlay)
        {
            // Check if wagon damage type affects card protection type
            foreach (GameConstants.radiationTypes radiationType in card.protectionTypes)
            {
                if (card.durabilityCurrent > 0)
                {
                    if (card.immunityTypes.Contains(radiationType))
                    {
                        Debug.Log("Card is immun to " + radiationType + ". Set damage of " + wagonDamageStats[radiationType] + " to 0.");
                        wagonDamageStats[radiationType] = 0;
                    }
                    else if (wagonDamageStats.ContainsKey(radiationType))
                    {
                        int damageValue = wagonDamageStats[radiationType];
                        Debug.Log(wagons[0].name + " will deal: " + wagonDamageStats[radiationType] + " of " + radiationType);
                        wagonDamageStats[radiationType] = card.AdjustDurability(damageValue);
                        card.UpdateDisplay();
                        if (card.effect && radiationType == GameConstants.radiationTypes.Gamma)
                        {
                            wagonDamageStats[radiationType] += card.GammaReduction(card.cardEffects[GameConstants.effectTypes.DamageReductionPercent]);
                        }
                        if (wagonDamageStats[radiationType] < 0)
                        {
                            Debug.Log("I killed a card with overkill damage. " + "Damage type: " + radiationType + " Damage left: " + wagonDamageStats[radiationType]);
                            wagonDamageStats[radiationType] = Mathf.Abs(wagonDamageStats[radiationType]);
                            wagons[0].UpdateDamageDuringRound(radiationType, 0);
                        }
                        else if (wagonDamageStats[radiationType] >= 0)
                        {
                            Debug.Log("No damage value left of damage type: " + radiationType);
                            wagonDamageStats[radiationType] = 0;
                            wagons[0].UpdateDamageDuringRound(radiationType, 0);
                        }
                    }
                    wagons[0].UpdateDamageDuringRound(radiationType, wagonDamageStats[radiationType]);
                    yield return new WaitForSeconds(1f);
                }
            }
            card.OnDurabilityZero -= HandleCardDurabilityZero;
        }

        foreach (var entry in wagonDamageStats)
        {
            if (entry.Value != 0)
            {
                gm.PlayerDamage(entry.Value, entry.Key);
            }
        }

        if (gm.IsBetaDotActive())
        {
            gm.PlayerBetaDotDamage();
        }
        yield return new WaitForSeconds(3f);
        endTurnButton.RotateKnopfBack();
        gm.ResetEnergy();
        gm.DrawCards();
        wagons[0].GenerateDamage();
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

    IEnumerator ProcessCard()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
}