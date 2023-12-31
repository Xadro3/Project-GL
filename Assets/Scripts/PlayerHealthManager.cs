using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerHealthManager : MonoBehaviour
{
    public static event System.Action EncounterEnd;
    public static event Action<String> ActivatePendantDebuff;

    [Header("----- Health -----")]
    public int health;
    [Range(0, 100)]
    public int healthMax = 100;
    public int hpThresholdLvOne;
    public int hpThresholdLvTwo;

    [Header("----- Alpha Resistance -----")]
    public int alphaResistance;
    [Range(0, 50)]
    public int alphaResistanceMax = 20;

    [Header("----- Beta Resistance -----")]
    public int betaResistance;
    [Range(0, 50)]
    public int betaResistanceMax = 20;

    [Header("----- Gamma Resistance -----")]
    public int gammaResistance;
    [Range(0, 50)]
    public int gammaResistanceMax = 30;

    private PlayerModel playerModel;

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

    private List<Tuple<System.Action, string>> triggeredActions = new List<Tuple<System.Action, string>>();

    GameManager gm;

    private void Awake()
    {
        alphaResistance = alphaResistanceMax;
        betaResistance = betaResistanceMax;
        gammaResistance = gammaResistanceMax;
    }
    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
        CardMovementHandler.OnPlayerEffect += HandlePlayerEffect;
    }
    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
        CardMovementHandler.OnPlayerEffect -= HandlePlayerEffect;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Encounter")
        {
            playerModel = FindObjectOfType<PlayerModel>();
            playerModel.alphaBar.SetMaxHealth(alphaResistanceMax);
            playerModel.alphaBar.SetHealth(alphaResistanceMax);
            playerModel.betaBar.SetMaxHealth(betaResistanceMax);
            playerModel.betaBar.SetHealth(betaResistanceMax);
            playerModel.gammaBar.SetMaxHealth(gammaResistanceMax);
            playerModel.gammaBar.SetHealth(gammaResistance);
            playerModel.healthBar.SetMaxHealth(healthMax);
            playerModel.healthBar.SetHealth(health);
            alphaResistance = alphaResistanceMax;
            betaResistance = betaResistanceMax;
            CheckHealth();
            UpdateTexts();
        }
    }

    private void HandlePlayerEffect(Card card)
    {
        HandleEffect(card);
    }

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
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
            if (!gm.encounterEndScreenActive)
            {
                // trigger Game Over
                Debug.Log("Game Over!");
                EncounterEnd?.Invoke();
            }
        }
    }

    private void HandlePuredamage(int damageValue)
    {
        if (healthDamageReductionPercent)
        {
            damageValue = damageValue * (1 - (healthDamageReductionFlatValue/100));
        }
        health -= damageValue;
        playerModel.alphaBar.SetHealth(health);
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
        playerModel.gammaBar.SetHealth(gammaResistance);
        UpdateTexts();
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
        playerModel.betaBar.SetHealth(betaResistance);
        UpdateTexts();
        Debug.Log("I just took: " + damageValue + " beta damage. My resistance is at: " + betaResistance);
        CheckResistances();
    }
    public void UpdateTexts()
    {
        playerModel.alphaText.text = alphaResistance.ToString();
        playerModel.betaText.text = betaResistance.ToString();
        playerModel.gammaText.text = gammaResistance.ToString();
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
        playerModel.alphaBar.SetHealth(alphaResistance);
        UpdateTexts();
        Debug.Log("I just took: " + damageValue + " alpha damage. My resistance is at: " + alphaResistance);
        CheckResistances();
    }

    private void CheckResistances()
    {
        if (alphaResistance <= 0)
        {
            playerModel.healthBar.SetHealth(health -= Mathf.RoundToInt(healthMax * 0.75f));
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

    public void TriggerRandomDebuff()
    {
        // Array of actions with names
        Tuple<System.Action, string>[] actions = new Tuple<System.Action, string>[]
        {
        Tuple.Create((System.Action)(() => gm.ActivateDamageBuff(GameConstants.radiationTypes.Alpha)), "ActivateDamageBuffAlpha"),
        Tuple.Create((System.Action)(() => gm.ActivateDamageBuff(GameConstants.radiationTypes.Beta)), "ActivateDamageBuffBeta"),
        Tuple.Create((System.Action)(() => gm.ActivateDamageBuff(GameConstants.radiationTypes.Gamma)), "ActivateDamageBuffGamma"),
        Tuple.Create((System.Action)(() => gm.SetCardCostIncrease(1)), "SetCardCostIncrease"),
        Tuple.Create((System.Action)(() => gm.playerRessourceMax -= 1), "DecreasePlayerResourceMax"),
        Tuple.Create((System.Action)(() => gm.playerHandMax -= 1), "DecreasePlayerHandMax"),
        Tuple.Create((System.Action)(() => gm.ActivateShieldDebuff()), "ActivateShieldDebuff")
        };

        // Filter out actions that have already been triggered
        List<Tuple<System.Action, string>> availableActions = actions.Except(triggeredActions).ToList();

        // Check if there are available actions
        if (availableActions.Count > 0)
        {
            // Randomly select and invoke an action
            int randomIndex = UnityEngine.Random.Range(0, availableActions.Count);
            Tuple<System.Action, string> selectedAction = availableActions[randomIndex];
            selectedAction.Item1.Invoke();

            // Add the triggered action to the list
            triggeredActions.Add(selectedAction);

            // Get the name of the triggered action
            string actionName = selectedAction.Item2;

            ActivatePendantDebuff?.Invoke(actionName);
            Debug.Log("Triggered Debuff: " + actionName);
        }
        else
        {
            Debug.Log("All actions have been triggered.");
        }
    }
    
    public int CheckHealth()
    {
        if (health > hpThresholdLvOne)
        {
            playerModel.characterOne.SetActive(true);
            playerModel.characterTwo.SetActive(false);
            playerModel.characterThree.SetActive(false);
        }else if(health >= hpThresholdLvTwo)
        {
            playerModel.characterOne.SetActive(false);
            playerModel.characterTwo.SetActive(true);
            playerModel.characterThree.SetActive(false);
        }else if (health < hpThresholdLvTwo)
        {
            playerModel.characterOne.SetActive(false);
            playerModel.characterTwo.SetActive(false);
            playerModel.characterThree.SetActive(true);
        }
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
                    gm.HandleEffect(entry.Key, entry.Value);
                    break;

                case GameConstants.effectTypes.HealthDamageReductionPercent:
                    TriggerHealthDamageReductionPercent(entry.Value);
                    break;
            }
            UpdateTexts();
        }
            
    }

    private void TriggerHealthDamageReductionPercent(int value)
    {
        healthDamageReductionFlatValue = value;
        healthDamageReductionPercent = true;
        playerModel.healthBar.SetHealth(health);
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
                    playerModel.alphaBar.SetHealth(alphaResistance);
                    break;

                case GameConstants.radiationTypes.Beta:
                    betaResistance += value;
                    playerModel.betaBar.SetHealth(betaResistance);
                    break;

                case GameConstants.radiationTypes.Gamma:
                    gammaResistance += value;
                    playerModel.gammaBar.SetHealth(gammaResistance);
                    break;
            }
            UpdateTexts();
        }
        
    }
    public void TriggerEncounterEndAnimation()
    {
        playerModel.playerModelAnimator.SetTrigger("BreachEnd");
        playerModel.playerAbilitySlotAnimator.SetTrigger("BreachEnd");
    }

    public void HandlePendantBuffActivation(GameConstants.pendantEffect effect, int effectValue)
    {
        switch (effect)
        {
            case GameConstants.pendantEffect.buffResistanceAlpha:
                alphaResistanceMax += effectValue;
                alphaResistance += effectValue;
                break;

            case GameConstants.pendantEffect.buffResistanceBeta:
                betaResistanceMax += effectValue;
                betaResistance += effectValue;
                break;

            case GameConstants.pendantEffect.buffResistanceGamma:
                gammaResistanceMax += effectValue;
                gammaResistance += effectValue;
                break;

            case GameConstants.pendantEffect.buffHealth:
                healthMax += effectValue;
                health += effectValue;
                break;
        }
        UpdateTexts();
    }
}
