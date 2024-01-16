using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSafe : MonoBehaviour
{
    public int eventCounter;
    public Dictionary<int, bool> events = new();
    bool populated=false;
    void Start()
    {

    }

    public void PopulateDict()
    {
        if (!populated)
        {
            events.Add(1, false);
            events.Add(2, false);
            events.Add(3, false);
            events.Add(4, false);
            events.Add(5, false);
            events.Add(6, false);
            events.Add(7, false);
            events.Add(8, false);
            events.Add(9, false);
            events.Add(10, false);


            populated = true;
        }
      
    }
    public int GetRandomEventWithFalseValue()
    {

        List<int> freeEvents = new List<int>();
       // Debug.Log("Im here");
        Debug.Log(events.Count);
        foreach (KeyValuePair<int,bool> entry in events)
        {
            Debug.Log(entry.Key);
            if (!entry.Value)
            {
                freeEvents.Add(entry.Key);
            }
        }

        if (freeEvents.Count == 0)
        {
            
            return -1; // You can return a specific value or handle this case accordingly
        }

        int randomIndex = UnityEngine.Random.Range(0, freeEvents.Count-1);
        return freeEvents[randomIndex];
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
