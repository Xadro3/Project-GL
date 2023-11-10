using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{

    public int damageValue = 0;
    public int roundTimer;
    public TextMeshProUGUI roundTimerText;

    public enum DamageType
    {
        Alpha,
        Beta,
        Gamma
    }

    public DamageType damageType;

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

    private void GenerateRandomDamage()
    {
        // select random type
        //DamageType damageType = (DamageType)Random.Range(0, 3);
        DamageType damageType = DamageType.Alpha;

        // random damage value based on type
        switch (damageType)
        {
            case DamageType.Alpha:
                damageValue = Random.Range(1, 13); // Alpha damage range: 1 to 12
                break;
            case DamageType.Beta:
                damageValue = Random.Range(1, 7);  // Beta damage range: 1 to 6
                break;
            case DamageType.Gamma:
                damageValue = Random.Range(1, 5);  // Gamma damage range: 1 to 4
                break;
        }
    }

}
