using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCardSlots : MonoBehaviour
{
    public List<Slot> activeCardSlots;


    private void Awake()
    {
        activeCardSlots.AddRange(GetComponentsInChildren<Slot>());
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
