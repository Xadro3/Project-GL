using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShowCash : MonoBehaviour
{
    public ShopCurrency playerCash;

    private void Start()
    {
        playerCash = GameObject.FindGameObjectWithTag("Wallet").GetComponentInChildren<ShopCurrency>();
        Debug.Log(playerCash.transform.parent.name);
    }
    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<TextMeshProUGUI>().text = playerCash.money.ToString();
    }
}
