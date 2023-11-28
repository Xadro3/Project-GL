using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldGenerator : MonoBehaviour
{
    public List<GameObject> nodes;
    public int noShops;
    public int noEvents;
    public int noEncounters;
    public int noWorkshops;

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        
        Dictionary<int, int> countMap = new Dictionary<int, int>();
        countMap.Add(0, 0);
        countMap.Add(1, 0);
        countMap.Add(2, 0);
        countMap.Add(3, 0);
        int[] maxOccurrences = new int[] { noShops, noEvents, noEncounters, noWorkshops };

        if (noShops + noEvents + noEncounters + noWorkshops <= nodes.Count)
        {
            noEncounters = noEncounters - (nodes.Count - (noShops + noEvents + noEncounters + noWorkshops));
        }

        if (noShops + noEvents + noEncounters + noWorkshops > nodes.Count)
        {
            noEncounters = (noShops + noEvents + noWorkshops) - nodes.Count;
        }


        for(int i=0;i<=noShops+noEvents+noWorkshops;i++)
        {
            int randomNumber;
            do
            {
                randomNumber = Random.Range(0, 4);

            } while (countMap[randomNumber] > maxOccurrences[randomNumber]);

            nodes[i].GetComponent<Node>().eventType = randomNumber;
            nodes[i].GetComponent<Node>().DisplayNode();
            Debug.Log(randomNumber);

            if (countMap.ContainsKey(randomNumber)){
                countMap[randomNumber]++;
            }
            else
            {
                countMap[randomNumber] = 1;
            }

        }

    }

}
