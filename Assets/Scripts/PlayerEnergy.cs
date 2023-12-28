using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerEnergy : MonoBehaviour
{
    public TextMeshProUGUI playerEnergyText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePlayerEnergy(int value)
    {
        playerEnergyText.text = value.ToString();
    }
}
