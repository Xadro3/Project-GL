using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Drag : MonoBehaviour
{

    public Vector3 snapLocation;
    public int itemCost;
    Vector3 mousePosition;
    public bool bought;
    public int resaleValue;
    public Vector3 startPos;
    public bool isDragging;
    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }
    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition - GetMousePos();
        startPos = transform.position;
        
    }
    private void OnMouseDrag()
    {
        if (!bought)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
        }
        
    }
    private void OnMouseUp()
    {
        if (!bought)
        {
            transform.position = startPos;
        }
        else
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponentInParent<RectTransform>());
        }
       
        
    }

}
