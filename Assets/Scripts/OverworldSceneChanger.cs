using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OverworldSceneChanger : MonoBehaviour
{
    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("click");
            Debug.DrawRay(Input.mousePosition, Vector3.forward, Color.green,100);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {
                
                Debug.Log(hit.transform.name);
                hit.collider.gameObject.GetComponent<Node>().EnterNode();
            }
        }
       
    }
}
