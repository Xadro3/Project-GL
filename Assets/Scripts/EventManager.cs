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
    public GameObject event_6;
    public GameObject event_7;
    public GameObject event_8;
    public GameObject event_9;
    public GameObject event_10;
    public int currentEvent;
    void Start()
    {
        eventSafe = GameObject.FindGameObjectWithTag("EventSafe").GetComponent<EventSafe>();
        eventSafe.PopulateDict();
        currentEvent =eventSafe.GetRandomEventWithFalseValue();
        PopulateScene(currentEvent);
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
            case 6:
                Instantiate(event_6);
                break;
            case 7:
                Instantiate(event_7);
                break;
            case 8:
                Instantiate(event_8);
                break;
            case 9:
                Instantiate(event_9);
                break;
            case 10:
                Instantiate(event_10);
                break;
            

        }

    }
    public void CompleteEvent()
    {
        eventSafe.CompleteEvent(currentEvent);
    }



 
}
