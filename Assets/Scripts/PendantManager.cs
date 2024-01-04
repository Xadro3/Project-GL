using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PendantManager : MonoBehaviour
{
    public static event System.Action<GameConstants.pendantEffect, int> ActivateEncounterBuff;
    public static event System.Action<GameConstants.pendantEffect, int> ActivatePlayerBuff;
    public static event System.Action<GameConstants.pendantEffect, int> ActivateShopBuff;
    
    public GameObject pendantContainer;
    public GameManager gameManager;
    public PlayerHealthManager player;


    public List<GameObject> pendants;


    Dictionary<GameConstants.pendantEffect, Dictionary<int, bool>> pendantEffects = new Dictionary<GameConstants.pendantEffect, Dictionary<int, bool>>();

    private void Awake()
    {

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        if (scene.name != "Menu")
        {
            pendantContainer = GameObject.FindGameObjectWithTag("PendantContainer");
            foreach (GameObject pendant in pendants)
            {
                if (pendant.GetComponent<PendantScript>().isActive)
                {
                    pendant.transform.SetParent(pendantContainer.transform);
                    pendant.SetActive(true);
                    pendant.GetComponent<PendantScript>().SendPendantInfoToManager(this);
                }
            }
        }

    }
    private void OnSceneUnloaded(Scene scene)
    {
        foreach (GameObject pendant in pendants)
        {
            pendant.transform.SetParent(this.gameObject.transform);
            pendant.SetActive(false);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddPendantInfo(GameConstants.pendantEffect type, int value, bool active)
    {
        pendantEffects[type] = new Dictionary<int, bool>();

        pendantEffects[type][value] = active;
    }

    public void TriggerPendantEffects()
    {
        foreach (var effectEntry in pendantEffects)
        {
            GameConstants.pendantEffect effect = effectEntry.Key;
            Dictionary<int, bool> effectData = effectEntry.Value;

            // Iterate through all values in the nested dictionary
            foreach (var valueEntry in effectData)
            {
                int value = valueEntry.Key;
                bool isActive = valueEntry.Value;

                // Check if the effect is active for the current value
                if (isActive)
                {
                    // Switch based on the effect
                    switch (effect)
                    {
                        case GameConstants.pendantEffect.encounterEndMoreToken:
                        case GameConstants.pendantEffect.firstCardLessCost:
                        case GameConstants.pendantEffect.buffAlu:
                        case GameConstants.pendantEffect.buffPaper:
                        case GameConstants.pendantEffect.buffBlei:
                        case GameConstants.pendantEffect.firstTurnMoreEnergy:
                        case GameConstants.pendantEffect.firstTurnMoreCards:
                            Debug.Log($"{effect} is active for value {value}");
                            gameManager.HandlePendantBuffActiavtion(effect, value);
                            break;

                        case GameConstants.pendantEffect.buffResistanceAlpha:
                        case GameConstants.pendantEffect.buffResistanceBeta:
                        case GameConstants.pendantEffect.buffResistanceGamma:
                        case GameConstants.pendantEffect.buffHealth:
                            Debug.Log($"{effect} is active for value {value}");
                            player.HandlePendantBuffActivation(effect, value);
                            break;

                        case GameConstants.pendantEffect.moreChanceBetterCards:
                        case GameConstants.pendantEffect.moreHealing:
                        case GameConstants.pendantEffect.shopCostReduction:
                        case GameConstants.pendantEffect.lessCostRemovingCards:
                        case GameConstants.pendantEffect.lessCostUpgradingCards:
                            Debug.Log($"{effect} is active for value {value}");
                            ActivateShopBuff?.Invoke(effect, value);
                            break;

                        default:
                            // Handle unknown effects
                            Debug.Log($"Unknown effect: {effect}");
                            break;
                    }
                }
            }
        }
    }


}
