using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
    }
    private void OnMouseUp()
    {
        transform.position = startPos;
        
    }

}
