using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public int health;
    public int healthMax;
    public int gammaResistance;
    public int gammaResistanceMax;
    public int betaResistance;
    public int betaResistanceMax;
    public int alphaResistance;
    public int alphaResistanceMax;
    public List<HealthBar> healthBars;
    PlayerHealthManager playerInfo = null;

    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameManager.UpdateUI += OnUpdateUI;
    }

    private void OnUpdateUI()
    {
        UpdatePlayerStats();
        UpdateUIElements();
    }

    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdatePlayerStats();
        UpdateUIElements();
    }

    private void UpdatePlayerStats()
    {
        playerInfo = FindAnyObjectByType<PlayerHealthManager>();
        if (playerInfo != null)
        {
            Debug.Log("Found PlayerHealthManager");
            healthMax = playerInfo.healthMax;
            health = playerInfo.health;
            gammaResistanceMax = playerInfo.gammaResistanceMax;
            gammaResistance = playerInfo.gammaResistance;
            betaResistanceMax = playerInfo.betaResistanceMax;
            betaResistance = playerInfo.betaResistance;
            alphaResistanceMax = playerInfo.alphaResistanceMax;
            alphaResistance = playerInfo.alphaResistance;
        }
    }

    private void UpdateUIElements()
    {
        Debug.Log("Updating UI Elements");
        healthBars.Clear();
        healthBars.AddRange(FindObjectsOfType<HealthBar>());

        if (healthBars.Count > 0)
        {
            foreach (HealthBar bar in healthBars)
            {
                switch (bar.gameObject.name)
                {
                    case "GammaBar":
                        bar.SetMaxHealth(gammaResistanceMax);
                        bar.SetHealth(gammaResistance);
                        break;
                    case "HealthBar":
                        bar.SetMaxHealth(healthMax);
                        bar.SetHealth(health);
                        break;
                    case "AlphaBar":
                        bar.SetMaxHealth(alphaResistanceMax);
                        bar.SetHealth(alphaResistance);
                        break;
                    case "BetaBar":
                        bar.SetMaxHealth(betaResistanceMax);
                        bar.SetHealth(betaResistance);
                        break;
                    default:
                        Debug.Log("No Bars");
                        break;
                }
            }
        }
        //else
        //{
        //    throw null;
        //}
    }

}
