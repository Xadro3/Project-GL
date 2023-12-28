using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckDisplay : MonoBehaviour
{
    public TextMeshProUGUI cardCounter;
    // Start is called before the first frame update
    private void Awake()
    {
        GameManager.UpdateDeckDisplay += HandleUpdateDisplay;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleUpdateDisplay(int value)
    {
        cardCounter.text = value.ToString();
    }
}
