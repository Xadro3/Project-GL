using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class CardDisplay : MonoBehaviour
{
    private Card card;

    public TextMeshPro nameText;
    public TextMeshPro descriptionText;

    public SpriteRenderer artworkImage;
    
    public TextMeshPro costText;
    public TextMeshPro durabilityText;
    public TextMeshPro repairText;

    public SpriteRenderer shieldIcon;
    public SpriteRenderer entsorgenIcon;

    public TextMeshPro currencyCost;
    public GameObject currencyField;

    public GameObject UpgradeVisual;


    private void Awake()
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
        if (scene.name == "Shops" || scene.name == "Workshop")
        {
            currencyField.SetActive(true);
        }else if (scene.name == "Encounter")
        {
            currencyField.SetActive(false);
        }
    }
    private void OnSceneUnloaded(Scene arg0)
    {
        throw new NotImplementedException();
    }

    void Start()
    {
        card = GetComponent<Card>();
        if (card.cardInfo.upgraded)
        {
            UpgradeVisual.SetActive(true);
        }
        UpdateDisplay();
    }

    public void SetupDisplay()
    {
        nameText.text = card.cardInfo.name;
        descriptionText.text = card.cardDescription;

        artworkImage.sprite = card.cardInfo.artwork;

        costText.text = card.cost.ToString();
        durabilityText.text = card.durabilityCurrent.ToString();

        currencyCost.text = card.currencyCost.ToString();
    }

    public void UpdateDisplay()
    {
        descriptionText.text = card.cardDescription;

        if (card.cost < 0)
        {
            costText.text = "0";
        }
        else
        {
            costText.text = card.cost.ToString();
        }
        
        durabilityText.text = card.durabilityCurrent.ToString();
        currencyCost.text = card.currencyCost.ToString();
    }

    public void UpdateDurability(int value)
    {
        durabilityText.text = value.ToString();
    }


    public void ActivateShieldIcon(bool b)
    {
        shieldIcon.gameObject.SetActive(b);
    }

    public void ActivateEntsorgenIcon(bool b)
    {
        entsorgenIcon.gameObject.SetActive(b);
    }

    public void ActivateCurrencyCostField(bool b)
    {
        currencyField.SetActive(b);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
