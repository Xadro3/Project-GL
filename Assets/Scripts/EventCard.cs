using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventCard : MonoBehaviour
{
    public bool isCorrectAnswer;
    public string text;
    
    void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
