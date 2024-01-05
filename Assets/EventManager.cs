using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Start is called before the first frame update
    EventSafe eventSafe;
    public GameObject event_1;
    public GameObject event_2;
    public GameObject event_3;
    public GameObject event_4;
    public GameObject event_5;

    void Start()
    {
        eventSafe = GameObject.FindGameObjectWithTag("EventSafe").GetComponent<EventSafe>();
        eventSafe.eventCounter = eventSafe.eventCounter++;

    }

    void PopulateScene(int eventCounter)
    {
        switch (eventCounter)
        {
            case 1:
                Instantiate(event_1);
                break;
            case 2:
                Instantiate(event_2);
                break;
            case 3:
                Instantiate(event_3);
                break;
            case 4:
                Instantiate(event_4);
                break;
            case 5:
                Instantiate(event_5);
                break;

        }

    }

 
}
