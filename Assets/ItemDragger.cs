using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDragger : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("click");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, 100,5))
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.GetComponent<ItemBuyable>() != null)
                {
                    hit.collider.GetComponent<ItemBuyable>().isDragging = true;
                }
            }
        }
    }
}
