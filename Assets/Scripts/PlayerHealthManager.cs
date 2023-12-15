using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerHealthManager : MonoBehaviour
{
    public int health = 1;
    [Range(0, 100)]
    public int healthMax = 100;
    public int alphaResistance;
    [Range(0, 50)]
    public int alphaResistanceMax = 20;
    public int betaResistance;
    [Range(0, 50)]
    public int betaResistanceMax = 20;
    public int gammaResistance;
    [Range(0, 50)]
    public int gammaResistanceMax = 30;

    public HealthBar alphaBar;
    public HealthBar betaBar;
    public HealthBar gammaBar;
    public HealthBar healthBar;

    private bool alphaDamageReductionFlat = false;
    private bool alphaDamageReductionPercent = false;
    private bool betaDamageReductionFlat = false;
    private bool betaDamageReductionPercent = false;
    private bool gammaDamageReductionFlat = false;
    private bool gammaDamageReductionPercent = false;

    private bool healthDamageReductionPercent = false;


    public bool betaDotActive = false;
    public int betaDotDamage = 3;
    public int betaDotDamageSum = 0;

    private int healthDamageReductionFlatValue = 0;
    private int resistanceDamageReductionPercentValue = 0;
    private int resistanceDamageReductionFlatValue = 0;
    

    GameManager gm;

    private void Awake()
    {
        alphaResistance = alphaResistanceMax;
        betaResistance = betaResistanceMax;
        gammaResistance = gammaResistanceMax;
    }
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        alphaBar.SetMaxHealth(alphaResistanceMax);
        alphaBar.SetHealth(alphaResistanceMax);
        betaBar.SetMaxHealth(betaResistanceMax);
        betaBar.SetHealth(betaResistanceMax);
        gammaBar.SetMaxHealth(gammaResistanceMax);
        gammaBar.SetHealth(gammaResistanceMax);
        healthBar.SetMaxHealth(healthMax);
        healthBar.SetHealth(healthMax);

}

    // function to apply damage -> currently only total damage no debuffs here
    public void ApplyDamage(int damageValue, GameConstants.radiationTypes damageType)
    {
        //switch > if :)
        switch (damageType)
        {
            case GameConstants.radiationTypes.Alpha:
                HandleAlphaDamage(damageValue);
                break;

            case GameConstants.radiationTypes.Beta:
                HandleBetaDamage(damageValue);
                break;

            case GameConstants.radiationTypes.Gamma:
                HandleGammaDamage(damageValue);
                break;

            case GameConstants.radiationTypes.Pure:
                HandlePuredamage(damageValue);
                break;
        }
        // check if player survived damage
        CheckResistances();
        if (CheckHealth() <= 0)
        {
            // trigger Game Over
            Debug.Log("Game Over!");
        }
    }

    private void HandlePuredamage(int damageValue)
    {
        if (healthDamageReductionPercent)
        {
            damageValue = damageValue * (1 - (healthDamageReductionFlatValue/100));
        }
        health -= damageValue;
        healthBar.SetHealth(health);
    }

    private void HandleGammaDamage(int damageValue)
    {
        if (gammaDamageReductionFlat)
        {
            damageValue -= resistanceDamageReductionFlatValue;
        }
        if (gammaDamageReductionPercent)
        {
            damageValue = damageValue * (1 - (resistanceDamageReductionPercentValue/100));
        }
        gammaResistance -= damageValue;
        gammaBar.SetHealth(gammaResistance);
        Debug.Log("I just took: " + damageValue + " gamma damage. My resistance is at: " + gammaResistance);
        CheckResistances();
    }

    private void HandleBetaDamage(int damageValue)
    {
        if (betaDamageReductionFlat)
        {
            damageValue -= resistanceDamageReductionFlatValue;
        }
        if (betaDamageReductionPercent)
        {
            damageValue = damageValue * (1 - (resistanceDamageReductionPercentValue / 100));
        }
        betaResistance -= damageValue;
        betaBar.SetHealth(betaResistance);
        Debug.Log("I just took: " + damageValue + " beta damage. My resistance is at: " + betaResistance);
        CheckResistances();
    }

    private void HandleAlphaDamage(int damageValue)
    {
        if (alphaDamageReductionFlat)
        {
            damageValue -= resistanceDamageReductionFlatValue;
        }
        if (alphaDamageReductionPercent)
        {
            damageValue = damageValue * (1 - (resistanceDamageReductionPercentValue / 100));
        }
        alphaResistance -= damageValue;
        alphaBar.SetHealth(alphaResistance);
        Debug.Log("I just took: " + damageValue + " alpha damage. My resistance is at: " + alphaResistance);
        CheckResistances();
    }

    private void CheckResistances()
    {
        if (alphaResistance <= 0)
        {
            healthBar.SetHealth(health -= Mathf.RoundToInt(healthMax * 0.75f));
            alphaResistance = alphaResistanceMax;
            Debug.Log("Aua! Ich habe schaden bekommen!");
        }
        if (betaResistance <= 0)
        {
            betaDotActive = true;
            betaDotDamageSum += betaDotDamage;
            betaResistance = betaResistanceMax;
            ApplyDamage(betaDotDamageSum, GameConstants.radiationTypes.Pure);
        }
        if (gammaResistance <= 0)
        {
            gammaResistance = gammaResistanceMax;
            TriggerRandomDebuff();
        }
    }

    private void TriggerRandomDebuff()
    {
        // Array of actions
        System.Action[] actions = new System.Action[]
        {
            () => gm.ActivateDamageBuff(GameConstants.radiationTypes.Alpha),
            () => gm.ActivateDamageBuff(GameConstants.radiationTypes.Beta),
            () => gm.ActivateDamageBuff(GameConstants.radiationTypes.Gamma),
            () => gm.SetCardCostIncrease(1),
            () => gm.playerRessourceMax -= 1,
            () => gm.playerHandMax -= 1,
            () => gm.ActivateShieldDebuff()
            
        };

        // Randomly select and invoke an action
        int randomIndex = UnityEngine.Random.Range(0, actions.Length);
        actions[randomIndex].Invoke();
    }
    
    public int CheckHealth()
    {
        return health;
    }

    public void HandleEffect(Card card)
    {
        foreach (var entry in card.cardEffects)
        {
            switch (entry.Key)
            {
                case GameConstants.effectTypes.ResistanceReductionFlat:
                    TriggerResistanceReductionFlat(card.protectionTypes, entry.Value);
                    Debug.Log("Effect: " + entry);
                    break;

                case GameConstants.effectTypes.ResistanceReductionPercent:
                    TriggerResistanceDamageReductionPercent(card.protectionTypes, entry.Value);
                    Debug.Log("Effect: " + entry);
                    break;

                case GameConstants.effectTypes.ResistanceEffectReduction:
                    Debug.Log("Effect: " + entry);
                    break;

                case GameConstants.effectTypes.PlayerHealFlat:
                    Debug.Log("Effect: " + entry);
                    //health += value;
                    break;

                case GameConstants.effectTypes.PlayerHealPercent:
                    Debug.Log("Effect: " + entry);
                    //health += (1 + (value / 100));
                    break;

                case GameConstants.effectTypes.DrawCard:
                case GameConstants.effectTypes.Discard:
                    gm.HandleEffect(card);
                    break;

                case GameConstants.effectTypes.HealthDamageReductionPercent:
                    TriggerHealthDamageReductionPercent(entry.Value);
                    break;
            }
        }
            
    }

    private void TriggerHealthDamageReductionPercent(int value)
    {
        healthDamageReductionFlatValue = value;
        healthDamageReductionPercent = true;
    }

    private void TriggerResistanceDamageReductionPercent(List<GameConstants.radiationTypes> radiations, int value)
    {
        resistanceDamageReductionPercentValue = value;
        foreach (GameConstants.radiationTypes entry in radiations)
        {
            switch (entry)
            {
                case GameConstants.radiationTypes.Alpha:
                    alphaDamageReductionPercent = true;
                    break;

                case GameConstants.radiationTypes.Beta:
                    betaDamageReductionPercent = true;
                    break;

                case GameConstants.radiationTypes.Gamma:
                    gammaDamageReductionPercent = true;
                    break;
            }
        }
    }

    private void TriggerResistanceReductionFlat(List<GameConstants.radiationTypes> radiations, int value)
    {
        foreach (GameConstants.radiationTypes entry in radiations)
        {
            switch (entry)
            {
                case GameConstants.radiationTypes.Alpha:
                    alphaResistance += value;
                    break;

                case GameConstants.radiationTypes.Beta:
                    betaResistance += value;
                    break;

                case GameConstants.radiationTypes.Gamma:
                    gammaResistance -= value;
                    break;
            }
        }
    }
}
