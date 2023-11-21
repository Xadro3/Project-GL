using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Playerinventory inventory;
    public List<GameObject> localinventory;
    public List<GameObject> inventoryslots;

    private void Start()
    {
        Debug.Log(GameObject.Find("Inventory").name);
        inventory = GameObject.Find("Inventory").GetComponent<Playerinventory>();
    }
    public void AddItem(GameObject item)
    {
        Debug.Log(item);
        Debug.Log(inventory);
        inventory.Add(item);
        localinventory.Add(item);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if (other.gameObject.tag=="Items")
        {
            GameObject.FindGameObjectWithTag("Wallet").GetComponentInChildren<ShopCurrency>().RemoveMoney(other.GetComponent<ItemBuyable>().itemCost);
            GameObject game  = other.gameObject;
            Debug.Log(game);
            AddItem(game);
            SnapToGrid(game);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Items")
        {
            
            GameObject game = other.gameObject;
            Debug.Log(game);
            RemoveItem(game);
            
        }
    }
    private void RemoveItem(GameObject game)
    {
        inventory.Remove(game);
        localinventory.Remove(game);
    }

    public void SnapToGrid(GameObject item)
    {
        item.GetComponent<ItemBuyable>().snapLocation = inventoryslots[localinventory.Count - 1].transform.position;
    }
}
