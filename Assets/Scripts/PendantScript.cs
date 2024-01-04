using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PendantScript : MonoBehaviour
{
    [Header("General Info")]
    public new string name;
    [TextAreaAttribute]
    public string description;
    public SpriteRenderer bandRenderer;
    public SpriteRenderer anhaengerRenderer;
    public PendantManager homeBase;
    public GameObject infoPopup;
    
    [Header("Effect Info")]
    public int pendantEffectValue;
    public GameConstants.pendantEffect pendantEffectType;
    public bool isActive = false;
    public bool isInEffect = false;

    public void SetSpriteRendererActive(bool b)
    {
        anhaengerRenderer.enabled = b;
        bandRenderer.enabled = b;
    }

    private void Awake()
    {
        homeBase = FindObjectOfType<PendantManager>();
        OverworldSceneChanger.SceneChangeEvent += EveryoneGetInHere;
        ButtonManager.SceneChangeEvent += EveryoneGetInHere;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if (scene.name == "Encounter" && isActive)
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        if (scene.name == "Encounter" && isActive)
        {
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    private void EveryoneGetInHere()
    {
        //Bringing Pendants to the safety of the Wallet :>
        gameObject.transform.SetParent(homeBase.transform);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    private void OnDestroy()
    {
        OverworldSceneChanger.SceneChangeEvent -= EveryoneGetInHere;
        ButtonManager.SceneChangeEvent -= EveryoneGetInHere;
    }

    private void Start()
    {
        
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ShowInfoPopup();
        }
    }

    private void ShowInfoPopup()
    {
        // Instantiate the card popup prefab
        GameObject cardPopup = Instantiate(infoPopup, transform.position, Quaternion.identity);

        // Set the card popup content (you can customize this based on your card's data)
        CardPopup popupScript = cardPopup.GetComponent<CardPopup>();
        if (popupScript != null)
        {
            popupScript.SetCardInfo(description);
        }
    }

    public void SetPendantActive(bool b)
    {
        isActive = b;
    }

    public void SetPendantInEffect(bool b)
    {
        isInEffect = b;
    }

}
