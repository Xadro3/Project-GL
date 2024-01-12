using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDebugScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateDebug()
    {
        Debug.LogWarning("Pressed the fkn Button!");
    }
}
