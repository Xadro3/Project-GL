using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursedPendantScript : MonoBehaviour
{
    [Header("General Info")]
    public new string name;
    [TextAreaAttribute]
    public string description;
    public SpriteRenderer bandRenderer;
    public SpriteRenderer anhaengerRenderer;
    public CursedPendantManager homeBase;
    public GameObject infoPopup;
    public bool popupActive = false;

    [Header("Effect Info")]
    public bool isActive = false;

    public void SetSpriteRendererActive(bool b)
    {
        anhaengerRenderer.enabled = b;
        bandRenderer.enabled = b;
    }
    private void OnPopupTrigger(bool b)
    {
        popupActive = b;
    }

    private void Awake()
    {
        homeBase = FindObjectOfType<CursedPendantManager>();
        OverworldSceneChanger.SceneChangeEvent += EveryoneGetInHere;
        ButtonManager.SceneChangeEvent += EveryoneGetInHere;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        CardPopup.PauseGame += OnPopupTrigger;

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
        if (isActive && scene.name == "Encounter")
        {
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    private void EveryoneGetInHere()
    {
        //Bringing Pendants to the safety of the Wallet :>
        //gameObject.transform.SetParent(homeBase.transform);
        //GetComponent<BoxCollider2D>().enabled = false;
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        CardPopup.PauseGame -= OnPopupTrigger;
    }
    private void OnDestroy()
    {
        OverworldSceneChanger.SceneChangeEvent -= EveryoneGetInHere;
        ButtonManager.SceneChangeEvent -= EveryoneGetInHere;
        CardPopup.PauseGame -= OnPopupTrigger;
    }

    private void Start()
    {
        
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && !popupActive)
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
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
