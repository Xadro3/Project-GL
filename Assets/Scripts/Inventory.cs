using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    List<GameObject> inventory;
    public void AddItem(GameObject item)
    {
        inventory.Add(item);
    }
}
