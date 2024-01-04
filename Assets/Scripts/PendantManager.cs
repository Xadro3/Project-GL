using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PendantManager : MonoBehaviour
{
    public static event System.Action<GameConstants.pendantEffect, int> ActivateEncounterBuff;
    public static event System.Action<GameConstants.pendantEffect, int> ActivatePlayerBuff;
    public static event System.Action<GameConstants.pendantEffect, int> ActivateShopBuff;
    
    public GameObject pendantContainer;
    public GameManager gameManager;
    public PlayerHealthManager player;
    public List<GameObject> pendantPrefabs;
    public List<GameObject> pendantInstances;

    private void Awake()
    {
        foreach (GameObject pendantPrefab in pendantPrefabs)
        {
            GameObject newPendantObject = Instantiate(pendantPrefab, Vector3.zero, Quaternion.identity);
            pendantInstances.Add(newPendantObject);
            newPendantObject.transform.SetParent(transform);

        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        if (scene.name == "Encounter" || scene.name == "Overworld" || scene.name == "Shops")
        {
            pendantContainer = GameObject.FindGameObjectWithTag("PendantContainer");
            
            foreach (GameObject pendant in pendantInstances)
            {
                if (pendant.GetComponent<PendantScript>().isActive)
                {
                    pendant.transform.SetParent(pendantContainer.transform);
                    pendant.GetComponent<PendantScript>().spriteRenderer.enabled = true;
                    pendant.GetComponent<RectTransform>().localScale = new Vector3(60f, 60f, 60f);
                    pendant.GetComponent<RectTransform>().SetLocalPositionAndRotation(new Vector3(0f,0f,0f), Quaternion.identity);
                }
                else
                {
                    pendant.transform.SetParent(this.gameObject.transform);
                    pendant.GetComponent<PendantScript>().spriteRenderer.enabled = false;
                }
            }
            TriggerPendantEffects();
        }
        
    }
    
    public void TriggerPendantEffects()
    {
        foreach (GameObject pendantObject in pendantInstances)
        {
            PendantScript pendantScript = pendantObject.GetComponent<PendantScript>();

            if (pendantScript != null && pendantScript.isActive && !pendantScript.isInEffect)
            {
                GameConstants.pendantEffect effect = pendantScript.pendantEffectType;
                int value = pendantScript.pendantEffectValue;

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
                pendantScript.SetPendantInEffect(true);
            }
        }
    }

    public void AwardRandomPendant()
    {
        // Filter the list to include only inactive pendants
        List<GameObject> inactivePendants = pendantInstances.FindAll(pendant => !pendant.GetComponent<PendantScript>().isActive);

        // Check if there are any inactive pendants
        if (inactivePendants.Count > 0)
        {
            // Pick a random index from the list of inactive pendants
            int randomIndex = Random.Range(0, inactivePendants.Count);

            // Return the randomly chosen inactive pendant
            inactivePendants[randomIndex].GetComponent<PendantScript>().SetPendantActive(true);
        }
        else
        {
            // No inactive pendants found
            Debug.LogWarning("No inactive pendants available.");
        }
    }
}
