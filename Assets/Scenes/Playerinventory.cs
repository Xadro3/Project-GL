using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerinventory : MonoBehaviour
{
    List<GameObject> inventory = new List<GameObject>();


 
    public void Add(GameObject gameObject)
    {
        inventory.Add(gameObject);
    }

    public void Remove(GameObject gameObject)
    {
        inventory.Remove(gameObject);
    }
}
