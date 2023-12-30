using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeInstancer : MonoBehaviour
{
    public GameObject node;
    public OverworldGenerator overworld;
    public GameObject nextNode;
    public GameObject nextNode2;
    public GameObject instantiatedNode;
    public bool isStartNode;
    public bool isEndNode;
    public string nextMap;

    public void InstantiateNode()
    {
        instantiatedNode = Instantiate(node, transform.position, transform.rotation);
        instantiatedNode.GetComponent<Node>().isLastNode = isEndNode;
        instantiatedNode.GetComponent<Node>().isUnlocked = isStartNode;
        instantiatedNode.GetComponent<Node>().isFirstNode = isStartNode;
        instantiatedNode.GetComponent<Node>().nextNode = nextNode;
        instantiatedNode.GetComponent<Node>().nextMap = nextMap;
        if(nextNode2 != null)
        {
            instantiatedNode.GetComponent<Node>().nextNode2 = nextNode2;
        }
        instantiatedNode.GetComponent<Node>().WakeUp();
        overworld.nodes.Add(instantiatedNode);
    }


}
