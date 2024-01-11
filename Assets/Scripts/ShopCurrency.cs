using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class ShopCurrency : MonoBehaviour
{
    public static event Action<int> CurrencyUpdateEvent;
    
    public int money;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        TriggerCurrencyUpdateEvent();
    }

    private void Start()
    {
        TriggerCurrencyUpdateEvent();
    }

    public void AddMoney(int amountToAdd)
    {
        money = money + amountToAdd;
        TriggerCurrencyUpdateEvent();
    }
    public bool RemoveMoney(int amountToRemove)
    {
        if(money - amountToRemove >= 0)
        {
            money = money - amountToRemove;
            TriggerCurrencyUpdateEvent();
            return true;
            
        }
        else
        {
            return false;
        }
        
        
    }
    
    public void TriggerCurrencyUpdateEvent()
    {
        CurrencyUpdateEvent?.Invoke(money);
    }
}
