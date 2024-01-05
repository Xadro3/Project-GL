using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSafe : MonoBehaviour
{
    public int eventCounter;
    public Dictionary<int, bool> events = new Dictionary<int, bool>();

    private void Start()
    {
        events.Add(1, false);
        events.Add(2, false);
        events.Add(3, false);
        events.Add(4, false);
        events.Add(5, false);
    }

}
