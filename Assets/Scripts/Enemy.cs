using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{

    public int damageValue = 0;
    public int roundTimer = 0;
    public TextMeshProUGUI roundTimerText;
    public TextMeshProUGUI actionText;

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

    public int alphaDamage;
    public int betaDamage;
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

    public List<GameConstants.radiationTypes> damageTypes;

    public Dictionary<string, int> damageStats = new Dictionary<string, int>();

    GameManager gm;

    private void Start()
    {
        roundTimerText.text = roundTimer.ToString();
        populateDamage();
    }

    private void populateDamage()
    {
        foreach (GameConstants.radiationTypes damageType in damageTypes)
        {
            damageStats.TryAdd(damageType.ToString(), 0);
        }
    }

    public bool UpdateTimer(int i)
    {
        roundTimer -= i;
        roundTimerText.text = roundTimer.ToString();
        if (roundTimer <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Dictionary<string, int> GenerateDamage()
    {

        for (int i = 0; i < damageTypes.Count; i++)
        {
            switch (damageTypes[i])
            {
                case GameConstants.radiationTypes.Alpha:
                    damageValue = Random.Range(alphaMin, alphaMax + 1);
                    if (blockDurationAlpha > 0)
                    {
                        damageValue = 0;
                        blockDurationAlpha -= 1;
                    }
                    else if (reductionDurationAlphaFlat > 0)
                    {
                        damageValue -= reductionFlatValue;
                        reductionDurationAlphaFlat -= 1;
                    }
                    else if (reductionDurationAlphaPercent > 0)
                    {
                        damageValue -= damageValue * (reductionPercentValue / 100);
                        reductionDurationAlphaPercent -= 1;
                    }
                    damageStats[damageTypes[i].ToString()] = damageValue;
                    alphaDamage = damageValue;
                    break;

                case GameConstants.radiationTypes.Beta:
                    damageValue = Random.Range(betaMin, betaMax + 1);
                    if (blockDurationBeta > 0)
                    {
                        damageValue = 0;
                        blockDurationBeta -= 1;
                    }
                    else if (reductionDurationBetaFlat > 0)
                    {
                        damageValue -= reductionFlatValue;
                        reductionDurationBetaFlat -= 1;
                    }
                    else if (reductionDurationBetaPercent > 0)
                    {
                        damageValue -= damageValue * (reductionPercentValue / 100);
                        reductionDurationBetaPercent -= 1;
                    }
                    damageStats[damageTypes[i].ToString()] = damageValue;
                    betaDamage = damageValue;
                    break;

                case GameConstants.radiationTypes.Gamma:
                    damageValue = Random.Range(gammaMin, gammaMax + 1);
                    if (blockDurationGamma > 0)
                    {
                        damageValue = 0;
                        blockDurationGamma -= 1;
                    }
                    else if (reductionDurationGammaFlat > 0)
                    {
                        damageValue -= reductionFlatValue;
                        reductionDurationGammaFlat -= 1;
                    }
                    else if (reductionDurationGammaPercent > 0)
                    {
                        damageValue -= damageValue * (reductionPercentValue / 100);
                        reductionDurationGammaPercent -= 1;
                    }
                    damageStats[damageTypes[i].ToString()] = damageValue;
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

    public void UpdateDamageText()
    {
        actionText.text = "";
        foreach (string damageType in damageStats.Keys)
        {
            int damageValue = damageStats[damageType];
            actionText.text += $"{damageType}: {damageValue}\n";
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
                    HandleRadiationReductionFlat(card.protectionTypes, entry.Value, card.duration);
                    break;

                case GameConstants.effectTypes.RadiationReductionPercent:
                    HandleRadiationReductionPercent(card.protectionTypes, entry.Value, card.duration);
                    break;

                case GameConstants.effectTypes.RadiationBlock:
                    HandleRadiationBlock(card.protectionTypes, card.duration);
                    break;

                case GameConstants.effectTypes.RadiationOrderChange:
                    Debug.Log("There is no such card in the game at this moment");
                    break;

                case GameConstants.effectTypes.TimerReductionFlat:
                    HandleTimerReductionFlat(entry.Value);
                    break;
            }
        }
    }

    private void HandleTimerReductionFlat(int value)
    {
        UpdateTimer(value);
    }

    private void HandleRadiationBlock(List<GameConstants.radiationTypes> radiations, int duration)
    {
        foreach (GameConstants.radiationTypes entry in radiations)
        {
            switch (entry)
            {
                case GameConstants.radiationTypes.Alpha:
                    blockDurationAlpha = duration - 1;
                    damageStats[entry.ToString()] = 0;
                    break;

                case GameConstants.radiationTypes.Beta:
                    blockDurationBeta = duration - 1;
                    damageStats[entry.ToString()] = 0;
                    break;

                case GameConstants.radiationTypes.Gamma:
                    blockDurationGamma = duration - 1;
                    damageStats[entry.ToString()] = 0;
                    break;
            }
        }
    }
    private void HandleRadiationReductionPercent(List<GameConstants.radiationTypes> radiations, int value, int duration)
    {
        reductionPercentValue = value;
        foreach (GameConstants.radiationTypes entry in radiations)
        {
            switch (entry)
            {
                case GameConstants.radiationTypes.Alpha:
                    reductionDurationAlphaPercent = duration - 1;
                    damageStats[entry.ToString()] -= damageStats[entry.ToString()] * (value / 100);
                    break;

                case GameConstants.radiationTypes.Beta:
                    reductionDurationBetaPercent = duration - 1;
                    damageStats[entry.ToString()] -= damageStats[entry.ToString()] * (value / 100);
                    break;

                case GameConstants.radiationTypes.Gamma:
                    reductionDurationGammaPercent = duration - 1;
                    damageStats[entry.ToString()] -= damageStats[entry.ToString()] * (value / 100);
                    break;
            }
        }
    }
    private void HandleRadiationReductionFlat(List<GameConstants.radiationTypes> radiations, int value, int duration)
    {
        reductionFlatValue = value;
        foreach (GameConstants.radiationTypes entry in radiations)
        {
            switch (entry)
            {
                case GameConstants.radiationTypes.Alpha:
                    reductionDurationAlphaFlat = duration - 1;
                    damageStats[entry.ToString()] -= value;
                    break;

                case GameConstants.radiationTypes.Beta:
                    reductionDurationBetaFlat = duration - 1;
                    damageStats[entry.ToString()] -= value;
                    break;

                case GameConstants.radiationTypes.Gamma:
                    reductionDurationGammaFlat = duration - 1;
                    damageStats[entry.ToString()] -= value;
                    break;
            }
        }
    }
}
