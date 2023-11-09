using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerHealthManager : MonoBehaviour
{
    [Range(0,100)]
    public int health = 20;
    [Range(0, 100)]
    public int healthMax = 20;
    public int alphaResistance = 0;
    [Range(0, 50)]
    public int alphaResistanceMax = 25;
    public int betaResistance = 0;
    [Range(0, 50)]
    public int betaResistanceMax = 25;
    public int gammaResistance = 0;
    [Range(0, 50)]
    public int gammaResistanceMax = 25;

    private void Start()
    {
    }

    // function to apply damage -> currently only total damage no debuffs here
    public void ApplyDamage(int damageValue, string damageType)
    {
        if (damageType == "Alpha")
        {
            alphaResistance += damageValue;
        }
        if (damageType == "Beta")
        {
            betaResistance += damageValue;
        }
        if (damageType == "Gamma")
        {
            gammaResistance += damageValue;
        }
        if (damageType == "Pure")
        {
            health -= damageValue;
        }
        // check if player survived damage
        CheckResistances();
        if (health <= 0)
        {
            // trigger Game Over
            Debug.Log("Game Over!");
        }
    }

    private void CheckResistances()
    {
        if (alphaResistance >= alphaResistanceMax)
        {
            health -= Mathf.RoundToInt(healthMax * 0.75f);
            Debug.Log("Aua! Ich habe schaden bekommen!");
        }
        if (betaResistance >= betaResistanceMax)
        {
            ApplyDamage(3, "Pure");
        }
        if (gammaResistance >= gammaResistanceMax)
        {
            Debug.Log("Rework Gamma Konsequenzes!");
        }
    }

}
