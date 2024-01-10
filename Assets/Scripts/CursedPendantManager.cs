using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class CursedPendantManager : MonoBehaviour
{

    public GameObject pendantContainer;
    public GameManager gameManager;
    public PlayerHealthManager player;
    public List<GameObject> pendantPrefabs;
    public List<GameObject> pendantInstances;
    public List<bool> pendantActive;

    private void Awake()
    {
        PlayerHealthManager.ActivatePendantDebuff += OnPendantDebuffActivated;
        //InitializePendants();
    }

    private void InitializePendants()
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

    private void OnPendantDebuffActivated(string pendantDebuff)
    {
        switch (pendantDebuff)
        {
            case "ActivateDamageBuffAlpha":
                pendantInstances[0].GetComponent<CursedPendantScript>().SetPendantActive(true);
                pendantActive[0] = true;
                break;
            case "ActivateDamageBuffBeta":
                pendantInstances[1].GetComponent<CursedPendantScript>().SetPendantActive(true);
                pendantActive[1] = true;
                break;
            case "ActivateDamageBuffGamma":
                pendantInstances[2].GetComponent<CursedPendantScript>().SetPendantActive(true);
                pendantActive[2] = true;
                break;
            case "DecreasePlayerResourceMax":
                pendantInstances[3].GetComponent<CursedPendantScript>().SetPendantActive(true);
                pendantActive[3] = true;
                break;
            case "DecreasePlayerHandMax":
                pendantInstances[4].GetComponent<CursedPendantScript>().SetPendantActive(true);
                pendantActive[4] = true;
                break;
            case "SetCardCostIncrease":
                pendantInstances[5].GetComponent<CursedPendantScript>().SetPendantActive(true);
                pendantActive[5] = true;
                break;
            case "ActivateShieldDebuff":
                pendantInstances[6].GetComponent<CursedPendantScript>().SetPendantActive(true);
                pendantActive[6] = true;
                break;
        }
        AktivatePendantDisplay();
        Debug.Log("Pendant activated");
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        if (scene.name == "Encounter" || scene.name == "Overworld" || scene.name == "Shops" || scene.name == "DesertMap")
        {
            pendantInstances.Clear();
            InitializePendants();
            for (int i = 0; i < pendantActive.Count; i++)
            {
                if (pendantActive[i])
                {
                    pendantInstances[i].GetComponent<CursedPendantScript>().SetPendantActive(true);
                }
            }
            AktivatePendantDisplay();
        }
    }

    private void AktivatePendantDisplay()
    {
        pendantContainer = GameObject.FindGameObjectWithTag("CursedPendantContainer");

        foreach (GameObject pendant in pendantInstances)
        {
            if (pendant != null)
            {
                if (pendant.GetComponent<CursedPendantScript>().isActive)
                {
                    pendant.transform.SetParent(pendantContainer.transform);
                    pendant.GetComponent<CursedPendantScript>().SetSpriteRendererActive(true);
                    pendant.GetComponent<RectTransform>().localScale = new Vector3(60f, 60f, 60f);
                    pendant.GetComponent<RectTransform>().SetLocalPositionAndRotation(new Vector3(0f, 0f, 0f), Quaternion.identity);
                }
                else
                {
                    pendant.transform.SetParent(this.gameObject.transform);
                    pendant.GetComponent<CursedPendantScript>().SetSpriteRendererActive(false);
                }
            }            
        }
    }
}
