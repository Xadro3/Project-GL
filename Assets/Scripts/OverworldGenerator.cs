using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldGenerator : MonoBehaviour
{
    public GameObject[] nodes;
    public int noShops;
    public int noEvents;
    public int noEncounters;
    public int noWorkshops;

    private void Start()
    {
         nodes = GameObject.FindGameObjectsWithTag("Node");
        GenerateMap();
    }
    public void GenerateMap()
    {
        int setShops = 0;
        int setEvents = 0;
        int setEncounters = 0;
        int setWorkshops = 0;
        
        
        if(noShops+noEvents+noEncounters+noWorkshops <= nodes.Length)
        {
            noEncounters = noEncounters-( nodes.Length - (noShops + noEvents + noEncounters + noWorkshops));
        }

        if(noShops + noEvents + noEncounters + noWorkshops > nodes.Length)
        {
            noEncounters = (noShops + noEvents + noWorkshops) - nodes.Length;
        }

       
        foreach (GameObject node in nodes){
            if (setShops <= noShops)
            {
                node.GetComponent<Node>().eventType = Random.Range(1, 4);
                node.GetComponent<Node>().DisplayNode();
                setShops++;
                continue;
            }
            if (setEvents <= noEvents)
            {
                node.GetComponent<Node>().eventType = Random.Range(3, 4);
                node.GetComponent<Node>().DisplayNode();
                setEvents++;
                continue;
            }
            if (setEncounters <= noEncounters)
            {
                node.GetComponent<Node>().eventType = Random.Range(4, 4);
                node.GetComponent<Node>().DisplayNode();
                setEncounters++;
                continue;
            }
            if (setWorkshops <= noWorkshops)
            {
                node.GetComponent<Node>().eventType = Random.Range(2, 4);
                node.GetComponent<Node>().DisplayNode();
                setWorkshops++;
                continue;
            }
        }
    }

}
