using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeInstancer : MonoBehaviour
{
    public GameObject node;
    public OverworldGenerator overworld;
    void Awake()
    {
        overworld.nodes.Add(Instantiate(node, transform.position, transform.rotation));
    }


}
