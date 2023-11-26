using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerHealthManager : MonoBehaviour
{
    public int health = 1;
    [Range(0, 100)]
    public int healthMax = 100;
    public int alphaResistance = 0;
    [Range(0, 50)]
    public int alphaResistanceMax = 5;
    public int betaResistance = 0;
    [Range(0, 50)]
    public int betaResistanceMax = 25;
    public int gammaResistance = 0;
    [Range(0, 50)]
    public int gammaResistanceMax = 25;

    public HealthBar alphaBar;
    public HealthBar betaBar;
    public HealthBar gammaBar;
    public HealthBar healthBar;

    private void Start()
    {
        alphaBar.SetMaxHealth(alphaResistanceMax);
        alphaBar.SetHealth(alphaResistance);
        betaBar.SetMaxHealth(betaResistanceMax);
        betaBar.SetHealth(betaResistance);
        gammaBar.SetMaxHealth(gammaResistanceMax);
        gammaBar.SetHealth(gammaResistance);
        healthBar.SetMaxHealth(healthMax);
        healthBar.SetHealth(healthMax);
    }

    // function to apply damage -> currently only total damage no debuffs here
    public void ApplyDamage(int damageValue, string damageType)
    {
        //switch > if :)
        switch (damageType)
        {
            case "Alpha":
                alphaResistance += damageValue;
                alphaBar.SetHealth(alphaResistance);
                Debug.Log("I just took: " + damageValue + " alpha damage. My resistance is at: " + alphaResistance);
                CheckResistances();
                break;

            case "Beta":
                betaResistance += damageValue;
                betaBar.SetHealth(betaResistance);
                Debug.Log("I just took: " + damageValue + " beta damage. My resistance is at: " + betaResistance);
                CheckResistances();
                break;

            case "Gamma":
                gammaResistance += damageValue;
                gammaBar.SetHealth(gammaResistance);
                Debug.Log("I just took: " + damageValue + " gamma damage. My resistance is at: " + gammaResistance);
                CheckResistances();
                break;

            case "Pure":
                health -= damageValue;
                healthBar.SetHealth(health);
                CheckResistances();
                break;
        }
        // check if player survived damage
        CheckResistances();
        if (CheckHealth() <= 0)
        {
            // trigger Game Over
            Debug.Log("Game Over!");
        }
    }

    private void CheckResistances()
    {
        if (alphaResistance >= alphaResistanceMax)
        {
            healthBar.SetHealth(health -= Mathf.RoundToInt(healthMax * 0.75f));
            alphaResistance = 0;
            Debug.Log("Aua! Ich habe schaden bekommen!");
        }
        if (betaResistance >= betaResistanceMax)
        {
            ApplyDamage(3, "Pure");
            betaResistance = 0;
        }
        if (gammaResistance >= gammaResistanceMax)
        {
            Debug.Log("Rework Gamma Konsequenzes!");
            gammaResistance = 0;
        }
    }

    public int CheckResistance(string radiationType)
    {
        switch (radiationType)
        {
            case "Alpha":
                return alphaResistance;

            case "Beta":
                return alphaResistance;

            case "Gamma":
                return alphaResistance;
        }
        return 0;
    }
    
    public int CheckHealth()
    {
        return health;
    }

}
