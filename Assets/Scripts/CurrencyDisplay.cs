using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CurrencyDisplay : MonoBehaviour
{
    public TextMeshProUGUI tokenAmount;

    private void Awake()
    {
        ShopCurrency.CurrencyUpdateEvent += HandleCurrencyUpdate;
    }

    private void HandleCurrencyUpdate(int value)
    {
        UpdateText(value);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText(int value)
    {
        tokenAmount.text = value.ToString();
    }
}
