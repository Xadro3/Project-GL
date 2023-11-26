using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{

    public int damageValue = 0;
    public int roundTimer;
    public TextMeshProUGUI roundTimerText;
    public TextMeshProUGUI actionText;

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
        roundTimerText.text = "Time left: " + roundTimer.ToString();
        damageStats[damageTypes[0].ToString()] = 0;
        damageStats[damageTypes[1].ToString()] = 0;
        damageStats[damageTypes[2].ToString()] = 0;
    }

    
    public bool UpdateTimer(int i)
    {
        roundTimer -= i;
        roundTimerText.text = "Time left: " + roundTimer.ToString();
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
                    damageStats[damageTypes[i].ToString()] = damageValue;
                    break;

                case GameConstants.radiationTypes.Beta:
                    damageValue = Random.Range(betaMin, betaMax + 1);
                    damageStats[damageTypes[i].ToString()] = damageValue;
                    break;

                case GameConstants.radiationTypes.Gamma:
                    damageValue = Random.Range(gammaMin, gammaMax + 1);
                    damageStats[damageTypes[i].ToString()] = damageValue;
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
}
