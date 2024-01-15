using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public static event System.Action EncounterEnd;

    private EnemyModel enemyModel;
    [Header("----- Round Timer -----")]
    public int damageValue = 0;
    public int roundTimer = 0;
    public int roundTimerMin = 2;
    public int roundTimerMax = 5;
    [Header("----- Blocking -----")]
    public int blockDurationAlpha = 0;
    public int blockDurationBeta = 0;
    public int blockDurationGamma = 0;
    [Header("----- Reduction -----")]
    public int reductionFlatValue = 0;
    public int reductionDurationAlphaFlat = 0;
    public int reductionDurationBetaFlat = 0;
    public int reductionDurationGammaFlat = 0;

    public float reductionPercentValue = 0f;
    public int reductionDurationAlphaPercent = 0;
    public int reductionDurationBetaPercent = 0;
    public int reductionDurationGammaPercent = 0;
    [Header("----- Alpha Damage -----")]
    public bool scaleAlphaWithPhase;
    public int alphaDamageReduction;
    public bool alphaDamageBuff = false;
    public int alphaDamageBuffValue = 2;
    public int alphaDamage;
    [Header("----- Beta Damage -----")]
    public bool betaDamageBuff = false;
    public int betaDamageBuffValue = 2;
    public int betaDamage;
    [Header("----- Gamma Damage -----")]
    public bool gammaDamageBuff = false;
    public int gammaDamageBuffValue = 2;
    public int gammaDamage;

    [Header("----- Scaling -----")]
    private bool settingPhase = false;
    public int encounterCompleted = 0;
    public int encounterPhase = 0;
    public int bossBonus;

    [Header("----- Phase 1 -----")]
    public int phaseOneLimit;
    public int phaseOneTimerMin;
    public int phaseOneTimerMax;
    public int phaseOneDamageMin;
    public int phaseOneDamageMax;
    [Header("----- Phae 2 -----")]
    public int phaseTwoLimit;
    public int phaseTwoTimerMin;
    public int phaseTwoTimerMax;
    public int phaseTwoDamageMin;
    public int phaseTwoDamageMax;
    [Header("----- Phase 3 -----")]
    public int phaseThreeLimit;
    public int phaseThreeTimerMin;
    public int phaseThreeTimerMax;
    public int phaseThreeDamageMin;
    public int phaseThreeDamageMax;
    [Header("----- Phase 4 -----")]
    public int phaseFourLimit;
    public int phaseFourTimerMin;
    public int phaseFourTimerMax;
    public int phaseFourDamageMin;
    public int phaseFourDamageMax;
    [Header("----- Phase 5 -----")]
    public int phaseFiveLimit;
    public int phaseFiveTimerMin;
    public int phaseFiveTimerMax;
    public int phaseFiveDamageMin;
    public int phaseFiveDamageMax;
    [Header("----- Phase 6 -----")]
    public int phaseSixLimit;
    public int phaseSixTimerMin;
    public int phaseSixTimerMax;
    public int phaseSixDamageMin;
    public int phaseSixDamageMax;

    [Header("----- Damage Display No Function -----")]
    public int alphaMin;
    public int alphaMax;
    public int betaMin;
    public int betaMax;
    public int gammaMin;
    public int gammaMax;


    public List<GameConstants.radiationTypes> damageTypes;

    public Dictionary<GameConstants.radiationTypes, int> damageStats = new Dictionary<GameConstants.radiationTypes, int>();

    private GameManager gm;
    private void OnEnable()
    {
        Node.EnteringNodeEvent += IncreaseCompletedEncounterCount;
        SceneManager.sceneLoaded += OnSceneLoaded;
        CardMovementHandler.OnEnemyEffect += HandleEnemyEffect;
    }

    private void IncreaseCompletedEncounterCount(GameObject node)
    {
        encounterCompleted++;
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
            ResetDurationCounter();
}
    }

    public IEnumerator StartEncounter()
    {
        PopulateDamage();
        settingPhase = true;
        SetEncounterPhase();
        while (settingPhase)
        {
            yield return null;
        }
        GenerateTimerValue();
        
        yield break;
    }

    private void ResetDurationCounter()
    {
        reductionDurationAlphaFlat = 0;
        reductionDurationBetaFlat = 0;
        reductionDurationGammaFlat = 0;
        reductionDurationAlphaPercent = 0;
        reductionDurationBetaPercent = 0;
        reductionDurationGammaPercent = 0;
    }
    private void SetEncounterPhase()
    {
        int timerMin, timerMax, damageMin, damageMax;

        if (encounterCompleted < phaseOneLimit)
        {
            encounterPhase = 1;
            timerMin = phaseOneTimerMin;
            timerMax = phaseOneTimerMax;
            damageMin = phaseOneDamageMin;
            damageMax = phaseOneDamageMax;
            SetTimer(timerMin, timerMax);
            SetAlphaDamage(damageMin, damageMax);
            SetBetaDamage(damageMin, damageMax);
            SetGammaDamage(damageMin, damageMax);
        }
        else if (encounterCompleted < phaseTwoLimit)
        {
            encounterPhase = 2;
            timerMin = phaseTwoTimerMin;
            timerMax = phaseTwoTimerMax;
            damageMin = phaseTwoDamageMin;
            damageMax = phaseTwoDamageMax;
            SetTimer(timerMin, timerMax);
            SetAlphaDamage(damageMin, damageMax);
            SetBetaDamage(damageMin, damageMax);
            SetGammaDamage(damageMin, damageMax);
        }
        else if (encounterCompleted < phaseThreeLimit)
        {
            encounterPhase = 3;
            timerMin = phaseThreeTimerMin;
            timerMax = phaseThreeTimerMax;
            damageMin = phaseThreeDamageMin;
            damageMax = phaseThreeDamageMax;
            SetTimer(timerMin, timerMax);
            SetAlphaDamage(damageMin, damageMax);
            SetBetaDamage(damageMin, damageMax);
            SetGammaDamage(damageMin, damageMax);
        }
        else if (encounterCompleted < phaseFourLimit)
        {
            encounterPhase = 4;
            timerMin = phaseFourTimerMin;
            timerMax = phaseFourTimerMax;
            damageMin = phaseFourDamageMin;
            damageMax = phaseFourDamageMax;
            SetTimer(timerMin, timerMax);
            SetAlphaDamage(damageMin, damageMax);
            SetBetaDamage(damageMin, damageMax);
            SetGammaDamage(damageMin, damageMax);
        }
        else if (encounterCompleted < phaseFiveLimit)
        {
            encounterPhase = 5;
            timerMin = phaseFiveTimerMin;
            timerMax = phaseFiveTimerMax;
            damageMin = phaseFiveDamageMin;
            damageMax = phaseFiveDamageMax;
            SetTimer(timerMin, timerMax);
            SetAlphaDamage(damageMin, damageMax);
            SetBetaDamage(damageMin, damageMax);
            SetGammaDamage(damageMin, damageMax);
        }
        else if (encounterCompleted >= phaseSixLimit)
        {
            encounterPhase = 6;
            timerMin = phaseSixTimerMin;
            timerMax = phaseSixTimerMax;
            damageMin = phaseSixDamageMin;
            damageMax = phaseSixDamageMax;
            SetTimer(timerMin, timerMax);
            SetAlphaDamage(damageMin, damageMax);
            SetBetaDamage(damageMin, damageMax);
            SetGammaDamage(damageMin, damageMax);
        }
        if (GetComponentInParent<GameManager>().IsCurrentEncounterBoss())
        {
            MakeEncounterBoss();
        }
        settingPhase = false;
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
        Debug.Log("Setting timer. Min: " + roundTimerMin + " Max: " + roundTimerMax + " RoundTimer: " + roundTimer);
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
                        damageValue -= damageValue * (int)(reductionPercentValue / 100f);
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
                        damageValue -= damageValue * (int)(reductionPercentValue / 100f);
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
                        damageValue -= damageValue * (int)(reductionPercentValue / 100f);
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
    private void TriggerRadiationReductionPercent(List<GameConstants.radiationTypes> radiations, float value, int duration)
    {
        reductionPercentValue = value;
        
        foreach (GameConstants.radiationTypes entry in radiations)
        {
            float temp = 0f;
            int originalValue = damageStats[entry];
            switch (entry)
            {
                case GameConstants.radiationTypes.Alpha:
                    reductionDurationAlphaPercent = duration - 1;
                    temp = originalValue * (value / 100f);
                    damageStats[entry] -= (int)temp;
                    UpdateDamageDuringRound(entry, damageStats[entry]);
                    break;

                case GameConstants.radiationTypes.Beta:
                    reductionDurationBetaPercent = duration - 1;
                    temp = originalValue * (value / 100f);
                    damageStats[entry] -= (int)temp;
                    UpdateDamageDuringRound(entry, damageStats[entry]);
                    break;

                case GameConstants.radiationTypes.Gamma:
                    reductionDurationGammaPercent = duration - 1;
                    temp = originalValue * (value / 100f);
                    damageStats[entry] -= (int)temp;
                    UpdateDamageDuringRound(entry, damageStats[entry]);
                    break;
            }
            
        }
        UpdateDamageText();
    }
    private void TriggerRadiationReductionFlat(List<GameConstants.radiationTypes> radiations, int value, int duration)
    {
        reductionFlatValue += value;
        foreach (GameConstants.radiationTypes entry in radiations)
        {
            switch (entry)
            {
                case GameConstants.radiationTypes.Alpha:
                    reductionDurationAlphaFlat = duration - 1;
                    damageStats[entry] -= value;
                    Debug.Log("Reduced Alpha damage by: " + value + " to a new total of: " + damageStats[entry]);
                    UpdateDamageDuringRound(entry, damageStats[entry]);
                    break;

                case GameConstants.radiationTypes.Beta:
                    reductionDurationBetaFlat = duration - 1;
                    damageStats[entry] -= value;
                    UpdateDamageDuringRound(entry, damageStats[entry]);
                    Debug.Log("Reduced Beta damage by: " + value + " to a new total of: " + damageStats[entry]);
                    break;

                case GameConstants.radiationTypes.Gamma:
                    reductionDurationGammaFlat = duration - 1;
                    damageStats[entry] -= value;
                    UpdateDamageDuringRound(entry, damageStats[entry]);
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
        if (scaleAlphaWithPhase)
        {
            alphaMin = min - encounterPhase;
            alphaMax = max - encounterPhase;
        }
        else
        {
            alphaMin = min - alphaDamageReduction;
            alphaMax = max - alphaDamageReduction;
        }
        if (alphaMin <= 0)
        {
            alphaMin = 1;
        }
        
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
