using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeInstancer : MonoBehaviour
{
    public GameObject node;
    public OverworldGenerator overworld;
    public NodeInstancer nextNode;
    public GameObject instantiatedNode;
    bool nodesInstanced;

    public void InstantiateNode()
    {
            instantiatedNode = Instantiate(node, transform.position, transform.rotation);
            instantiatedNode.GetComponent<Node>().nextNode = instantiatedNode;
            overworld.nodes.Add(instantiatedNode);
            nodesInstanced=true;
    }


}
