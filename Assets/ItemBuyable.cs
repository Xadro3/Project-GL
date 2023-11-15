using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBuyable : MonoBehaviour
{
    public bool isDragging;
    public int itemCost;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            //https://youtu.be/pFpK4-EqHXQ do this 
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "PlayerInventory" && !isDragging)
        {
            GameObject.FindGameObjectWithTag("Wallet").GetComponentInChildren<ShopCurrency>().RemoveMoney(itemCost);
            GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<Inventory>().AddItem(this.gameObject);
        }
    }
    public void Drag()
    {
        isDragging = true;
    }
}
