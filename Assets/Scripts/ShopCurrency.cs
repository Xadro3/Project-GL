using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCurrency : MonoBehaviour
{
    public int money;
   
    public void AddMoney(int amountToAdd)
    {
        money = money + amountToAdd;
    }
    public void RemoveMoney(int amountToRemove)
    {
        money = money + amountToRemove;
    }
}
