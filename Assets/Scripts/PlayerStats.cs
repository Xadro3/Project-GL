using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    private int health;
    private int gammaResistance;
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
            health = playerInfo.health;
            gammaResistance = playerInfo.gammaResistance;
        }
    }

    private void UpdateUIElements()
    {
        Debug.Log("Updating UI Elements");
        List<HealthBar> healthBars = new List<HealthBar>();  
        healthBars.AddRange(FindObjectsOfType<HealthBar>());

        if (healthBars.Count > 0)
        {
            foreach (HealthBar bar in healthBars)
            {
                switch (bar.gameObject.name)
                {
                    case "GammaBar":
                        bar.SetHealth(gammaResistance);
                        break;
                    case "HealthBar":
                        bar.SetHealth(health);
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
