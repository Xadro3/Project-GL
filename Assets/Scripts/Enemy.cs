using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public static event System.Action EncounterEnd;

    private EnemyModel enemyModel;

    public int damageValue = 0;
    public int roundTimer = 0;
    public int roundTimerMin = 2;
    public int roundTimerMax = 5;

    private int blockDurationAlpha = 0;
    private int blockDurationBeta = 0;
    private int blockDurationGamma = 0;

    private int reductionFlatValue = 0;
    private int reductionDurationAlphaFlat = 0;
    private int reductionDurationBetaFlat = 0;
    private int reductionDurationGammaFlat = 0;

    private int reductionPercentValue = 0;
    private int reductionDurationAlphaPercent = 0;
    private int reductionDurationBetaPercent = 0;
    private int reductionDurationGammaPercent = 0;

    public bool alphaDamageBuff = false;
    public int alphaDamageBuffValue = 2;
    public int alphaDamage;

    public bool betaDamageBuff = false;
    public int betaDamageBuffValue = 2;
    public int betaDamage;

    public bool gammaDamageBuff = false;
    public int gammaDamageBuffValue = 2;
    public int gammaDamage;

    private int alphaMin = 1;
    [Range(1, 50)]
    public int alphaMax;
    private int betaMin = 1;
    [Range(1, 50)]
    public int betaMax;
    private int gammaMin = 1;
    [Range(1, 50)]
    public int gammaMax;

    public int bossBonus = 1;
    public int encounterPhase = 0;
    public int phaseOneLimit = 5;
    public int phaseTwoLimit = 10;
    public int phaseThreeLimit = 15;
    public int phaseFourLimit = 20;
    public int phaseFiveLimit = 25;
    public int phaseSixLimit = 30;

    public List<GameConstants.radiationTypes> damageTypes;

    public Dictionary<GameConstants.radiationTypes, int> damageStats = new Dictionary<GameConstants.radiationTypes, int>();

    private GameManager gm;
    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
        CardMovementHandler.OnEnemyEffect += HandleEnemyEffect;
    }
    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
        CardMovementHandler.OnEnemyEffect -= HandleEnemyEffect;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Encounter")
        {
            enemyModel = FindObjectOfType<EnemyModel>();

        }
    }

    public void StartEncounter()
    {
        SetEncounterPhase();
        GenerateTimerValue();
        PopulateDamage();
    }

    private void SetEncounterPhase()
    {
        int timerMin, timerMax, damageMin, damageMax;
        
        int encounterCompleted = GetComponentInParent<GameManager>().GetCompletedEncounter();

        if (encounterCompleted < phaseOneLimit)
        {
            encounterPhase = 1;
            timerMin = 2;
            timerMax = 5;
            damageMin = 2;
            damageMax = 5;
            SetTimer(timerMin, timerMax);
            SetAlphaDamage(damageMin, damageMax);
            SetBetaDamage(damageMin, damageMax);
            SetGammaDamage(damageMin, damageMax);
        }
        else if (encounterCompleted < phaseTwoLimit)
        {
            encounterPhase = 2;
            timerMin = 3;
            timerMax = 6;
            damageMin = 4;
            damageMax = 7;
            SetTimer(timerMin, timerMax);
            SetAlphaDamage(damageMin, damageMax);
            SetBetaDamage(damageMin, damageMax);
            SetGammaDamage(damageMin, damageMax);
        }
        else if (encounterCompleted < phaseThreeLimit)
        {
            encounterPhase = 3;
            timerMin = 4;
            timerMax = 7;
            damageMin = 6;
            damageMax = 8;
            SetTimer(timerMin, timerMax);
            SetAlphaDamage(damageMin, damageMax);
            SetBetaDamage(damageMin, damageMax);
            SetGammaDamage(damageMin, damageMax);
        }
        else if (encounterCompleted < phaseFourLimit)
        {
            encounterPhase = 4;
            timerMin = 5;
            timerMax = 8;
            damageMin = 8;
            damageMax = 10;
            SetTimer(timerMin, timerMax);
            SetAlphaDamage(damageMin, damageMax);
            SetBetaDamage(damageMin, damageMax);
            SetGammaDamage(damageMin, damageMax);
        }
        else if (encounterCompleted < phaseFiveLimit)
        {
            encounterPhase = 5;
            timerMin = 6;
            timerMax = 9;
            damageMin = 10;
            damageMax = 12;
            SetTimer(timerMin, timerMax);
            SetAlphaDamage(damageMin, damageMax);
            SetBetaDamage(damageMin, damageMax);
            SetGammaDamage(damageMin, damageMax);
        }
        else if (encounterCompleted >= phaseSixLimit)
        {
            encounterPhase = 6;
            timerMin = 7;
            timerMax = 10;
            damageMin = 12;
            damageMax = 14;
            SetTimer(timerMin, timerMax);
            SetAlphaDamage(damageMin, damageMax);
            SetBetaDamage(damageMin, damageMax);
            SetGammaDamage(damageMin, damageMax);
        }
        if (GetComponentInParent<GameManager>().IsCurrentEncounterBoss())
        {
            MakeEncounterBoss();
        }
    }
    private void MakeEncounterBoss()
    {
        alphaMin += bossBonus;
        alphaMax += bossBonus;
        betaMin += bossBonus;
        betaMax += bossBonus;
        gammaMin += bossBonus;
        gammaMax += bossBonus;
    }

    private void HandleEnemyEffect(Card card)
    {
        HandleEffect(card);
    }

    private void GenerateTimerValue()
    {
        int i = Random.Range(roundTimerMin, roundTimerMax + 1);
        roundTimer = i;
        enemyModel.roundTimerText.text = roundTimer.ToString();
    }

    private void PopulateDamage()
    {
        foreach (GameConstants.radiationTypes damageType in damageTypes)
        {
            damageStats.TryAdd(damageType, 0);
        }
    }

    public void UpdateTimer(int i)
    {
        roundTimer -= i;
        enemyModel.roundTimerText.text = roundTimer.ToString();
        if (roundTimer <= 0)
        {
            EncounterEnd?.Invoke();
        }
    }

    public void TriggerEncounterEndAnimation()
    {
        enemyModel.armDisplayAnimator.SetTrigger("BreachEnd");
    }
    public void ActivateDamageBuff(GameConstants.radiationTypes damageType)
    {
        switch (damageType)
        {
            case GameConstants.radiationTypes.Alpha:
                alphaDamageBuff = true;
                break;

            case GameConstants.radiationTypes.Beta:
                betaDamageBuff = true;
                break;

            case GameConstants.radiationTypes.Gamma:
                gammaDamageBuff = true;
                break;
        }
    }
    public Dictionary<GameConstants.radiationTypes, int> GenerateDamage()
    {
        for (int i = 0; i < damageTypes.Count; i++)
        {
            switch (damageTypes[i])
            {
                case GameConstants.radiationTypes.Alpha:
                    damageValue = Random.Range(alphaMin, alphaMax + 1);
                    if (reductionDurationAlphaFlat > 0)
                    {
                        damageValue -= reductionFlatValue;
                        reductionDurationAlphaFlat -= 1;
                    }
                    if (reductionDurationAlphaPercent > 0)
                    {
                        damageValue -= damageValue * (reductionPercentValue / 100);
                        reductionDurationAlphaPercent -= 1;
                    }
                    if (alphaDamageBuff)
                    {
                        damageValue += alphaDamageBuffValue;
                    }
                    if (blockDurationAlpha > 0)
                    {
                        damageValue = 0;
                        blockDurationAlpha -= 1;
                    }
                    damageStats[damageTypes[i]] = damageValue;
                    alphaDamage = damageValue;
                    break;

                case GameConstants.radiationTypes.Beta:
                    damageValue = Random.Range(betaMin, betaMax + 1);
                    if (reductionDurationBetaFlat > 0)
                    {
                        damageValue -= reductionFlatValue;
                        reductionDurationBetaFlat -= 1;
                    }
                    if (reductionDurationBetaPercent > 0)
                    {
                        damageValue -= damageValue * (reductionPercentValue / 100);
                        reductionDurationBetaPercent -= 1;
                    }
                    if (betaDamageBuff)
                    {
                        damageValue += betaDamageBuffValue;
                    }
                    if (blockDurationBeta > 0)
                    {
                        damageValue = 0;
                        blockDurationBeta -= 1;
                    }
                    damageStats[damageTypes[i]] = damageValue;
                    betaDamage = damageValue;
                    break;

                case GameConstants.radiationTypes.Gamma:
                    damageValue = Random.Range(gammaMin, gammaMax + 1);
                    if (reductionDurationGammaFlat > 0)
                    {
                        damageValue -= reductionFlatValue;
                        reductionDurationGammaFlat -= 1;
                    }
                    if (reductionDurationGammaPercent > 0)
                    {
                        damageValue -= damageValue * (reductionPercentValue / 100);
                        reductionDurationGammaPercent -= 1;
                    }
                    if (gammaDamageBuff)
                    {
                        damageValue += gammaDamageBuffValue;
                    }
                    if (blockDurationGamma > 0)
                    {
                        damageValue = 0;
                        blockDurationGamma -= 1;
                    }
                    damageStats[damageTypes[i]] = damageValue;
                    gammaDamage = damageValue;
                    break;

                case GameConstants.radiationTypes.Pure:
                    damageValue = Random.Range(0,0);
                    break;
            }
        }
        UpdateDamageText();
        return damageStats;
    }

    public void UpdateDamageDuringRound(GameConstants.radiationTypes damageType, int value)
    {
        damageStats[damageType] = value;
        UpdateDamageText();
    }

    public void UpdateDamageText()
    {
        foreach (GameConstants.radiationTypes damageType in damageStats.Keys)
        {
            int damageValue = damageStats[damageType];
            switch (damageType)
            {
                case GameConstants.radiationTypes.Alpha:
                    enemyModel.alphaText.text = $"{damageValue}";
                    break;

                case GameConstants.radiationTypes.Beta:
                    enemyModel.betaText.text = $"{damageValue}";
                    break;

                case GameConstants.radiationTypes.Gamma:
                    enemyModel.gammaText.text = $"{damageValue}";
                    break;
            }
        }
    }

    

    public void HandleEffect(Card card)
    {
        Debug.Log("Enemy Handling Effect");
        foreach (var entry in card.cardEffects)
        {
            switch (entry.Key)
            {
                case GameConstants.effectTypes.RadiationReductionFlat:
                    Debug.Log("Effect: " + entry.Key);
                    TriggerRadiationReductionFlat(card.protectionTypes, entry.Value, card.duration);
                    break;

                case GameConstants.effectTypes.RadiationReductionPercent:
                    Debug.Log("Effect: " + entry.Key);
                    TriggerRadiationReductionPercent(card.protectionTypes, entry.Value, card.duration);
                    break;

                case GameConstants.effectTypes.RadiationBlock:
                    Debug.Log("Effect: " + entry.Key);
                    TriggerRadiationBlock(card.protectionTypes, card.duration);
                    break;

                case GameConstants.effectTypes.RadiationOrderChange:
                    Debug.Log("Effect: " + entry.Key);
                    Debug.Log("There is no such card in the game at this moment");
                    break;

                case GameConstants.effectTypes.TimerReductionFlat:
                    Debug.Log("Effect: " + entry.Key);
                    TriggerTimerReductionFlat(entry.Value);
                    break;
            }
        }
    }

    private void TriggerTimerReductionFlat(int value)
    {
        UpdateTimer(value);
    }

    private void TriggerRadiationBlock(List<GameConstants.radiationTypes> radiations, int duration)
    {
        foreach (GameConstants.radiationTypes entry in radiations)
        {
            switch (entry)
            {
                case GameConstants.radiationTypes.Alpha:
                    blockDurationAlpha = duration - 1;
                    damageStats[entry] = 0;
                    break;

                case GameConstants.radiationTypes.Beta:
                    blockDurationBeta = duration - 1;
                    damageStats[entry] = 0;
                    break;

                case GameConstants.radiationTypes.Gamma:
                    blockDurationGamma = duration - 1;
                    damageStats[entry] = 0;
                    break;
            }
        }
        UpdateDamageText();
    }
    private void TriggerRadiationReductionPercent(List<GameConstants.radiationTypes> radiations, int value, int duration)
    {
        reductionPercentValue = value;
        foreach (GameConstants.radiationTypes entry in radiations)
        {
            switch (entry)
            {
                case GameConstants.radiationTypes.Alpha:
                    reductionDurationAlphaPercent = duration - 1;
                    damageStats[entry] -= damageStats[entry] * (value / 100);
                    break;

                case GameConstants.radiationTypes.Beta:
                    reductionDurationBetaPercent = duration - 1;
                    damageStats[entry] -= damageStats[entry] * (value / 100);
                    break;

                case GameConstants.radiationTypes.Gamma:
                    reductionDurationGammaPercent = duration - 1;
                    damageStats[entry] -= damageStats[entry] * (value / 100);
                    break;
            }
        }
        UpdateDamageText();
    }
    private void TriggerRadiationReductionFlat(List<GameConstants.radiationTypes> radiations, int value, int duration)
    {
        reductionFlatValue = value;
        foreach (GameConstants.radiationTypes entry in radiations)
        {
            switch (entry)
            {
                case GameConstants.radiationTypes.Alpha:
                    reductionDurationAlphaFlat = duration - 1;
                    damageStats[entry] -= value;
                    break;

                case GameConstants.radiationTypes.Beta:
                    reductionDurationBetaFlat = duration - 1;
                    damageStats[entry] -= value;
                    break;

                case GameConstants.radiationTypes.Gamma:
                    reductionDurationGammaFlat = duration - 1;
                    damageStats[entry] -= value;
                    break;
            }
        }
        UpdateDamageText();
    }

    private void SetTimer(int min, int max)
    {
        roundTimerMin = min;
        roundTimerMax = max;
    }
    private void SetAlphaDamage(int min, int max)
    {
        alphaMin = min;
        alphaMax = max;
    }
    private void SetBetaDamage(int min, int max)
    {
        betaMin = min;
        betaMax = max;
    }
    private void SetGammaDamage(int min, int max)
    {
        gammaMin = min;
        gammaMax = max;
    }
}
