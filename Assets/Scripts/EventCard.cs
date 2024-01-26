using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventCard : MonoBehaviour
{
    public static event System.Action<GameObject> AnswerSpawned;
    
    public bool isCorrectAnswer;
    public string text;
    
    void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = text;
        AnswerSpawned?.Invoke(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
