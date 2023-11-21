using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInventory : MonoBehaviour
{
    public Playerinventory inventory;
    public List<GameObject> localinventory;
    public List<GameObject> shopinventoryslots;

    private void Start()
    {
        Debug.Log(GameObject.Find("Inventory").name);
        inventory = GameObject.Find("Inventory").GetComponent<Playerinventory>();
    }
    public void AddItem(GameObject item)
    {
        Debug.Log(item);
        Debug.Log(inventory);
        inventory.Remove(item);
        localinventory.Add(item);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if (other.gameObject.tag == "Items")
        {
            GameObject.FindGameObjectWithTag("Wallet").GetComponentInChildren<ShopCurrency>().AddMoney(other.GetComponent<ItemBuyable>().resaleValue);
            GameObject game = other.gameObject;
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
        item.GetComponent<ItemBuyable>().snapLocation = shopinventoryslots[localinventory.Count - 1].transform.position;
    }
}
