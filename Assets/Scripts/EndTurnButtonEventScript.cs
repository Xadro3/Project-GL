using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EndTurnButtonEventScript : MonoBehaviour
{
    public static event System.Action EndTurnEvent;
    
    public void ButtonPressEndTurn()
    {
        EndTurnEvent?.Invoke();
    }
}
