using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public int GetRandomEvenWithFalseValue()
    {
        var eventFalseEvents = events.Where(entry => entry.Key % 2 == 0 && entry.Value == false).ToList();

        if (eventFalseEvents.Count == 0)
        {
            
            return -1; // You can return a specific value or handle this case accordingly
        }

        int randomIndex = UnityEngine.Random.Range(0, eventFalseEvents.Count);
        return eventFalseEvents[randomIndex].Key;
    }

    public void CompleteEvent(int key)
    {
        if (events.ContainsKey(key))
        {
            events[key] = true;
            Debug.Log("Event " + key + " completed!");
        }
        else
        {
            Debug.Log("Event with key " + key + " not found in the dictionary.");
        }
    }
}
