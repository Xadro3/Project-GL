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

    private int alphaDamageMin = 1;
    [Range(2, 50)]
    public int alphaDamageMax;
    private int betaDamageMin = 1;
    [Range(2, 50)]
    public int betaDamageMax;
    private int gammaDamageMin = 1;
    [Range(2, 50)]
    public int gammaDamageMax;

    public List<GameConstants.radiationTypes> damageTypes;

    private void Start()
    {
        roundTimerText.text = roundTimer.ToString();
        GenerateRandomDamage();
    }

    // Damage Type currently is selected randomly
    
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

    public void GenerateRandomDamage()
    {
        // select random type
        //damageTypes.Add((GameConstants.radiationTypes)Random.Range(0, 3));

        for (int i = 0; i < damageTypes.Count; i++)
        {
            if (damageTypes[i].ToString() == "Alpha")
            {
                damageValue = Random.Range(alphaDamageMin, alphaDamageMax + 1);
                break;
            }
            if (damageTypes[i].ToString() == "Beta")
            {
                damageValue = Random.Range(betaDamageMin, betaDamageMax + 1);
                break;
            }
            if (damageTypes[i].ToString() == "Gamma")
            {
                damageValue = Random.Range(gammaDamageMin, gammaDamageMax + 1);
                break;
            }
        }

        actionText.text = damageTypes[0].ToString() + " " + damageValue.ToString();

    }

}
