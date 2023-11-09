using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int damageValue = 0;

    public enum DamageType
    {
        Alpha,
        Beta,
        Gamma
    }

    public DamageType damageType;

    private void Start()
    {
        GenerateRandomDamage();
    }

    // Damage Type currently is selected randomly
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
        Debug.Log("Enemy selected " + damageType.ToString() + " damage with value: " + damageValue);
    }

}
