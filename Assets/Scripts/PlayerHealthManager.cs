using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerHealthManager : MonoBehaviour
{
    [Range(0,100)]
    public int health = 100;
    public int alphaResistance = 0;
    [Range(0, 50)]
    public int alphaResistanceMax = 25;
    public int betaResistance = 0;
    [Range(0, 50)]
    public int betaResistanceMax = 25;
    public int gammaResistance = 0;
    [Range(0, 50)]
    public int gammaResistanceMax = 25;

    private int totalDamage = 0;

    private void Start()
    {
    }

    // function to apply damage -> currently only total damage no debuffs here
    public void ApplyDamage(int alphaDamage, int betaDamage, int gammaDamage)
    {
        if (alphaResistance + alphaDamage > alphaResistanceMax)
        {
            totalDamage += alphaDamage;
        }
        if (betaResistance + betaDamage > betaResistanceMax)
        {
            totalDamage += betaDamage;
        }
        if (gammaResistance + gammaDamage > gammaResistanceMax)
        {
            totalDamage += gammaDamage;
        }

        health -= totalDamage;

        // check if player survived damage
        if (health <= 0)
        {
            // trigger Game Over
            Debug.Log("Game Over!");
        }
    }
}
