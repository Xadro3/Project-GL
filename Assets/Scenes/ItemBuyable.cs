using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBuyable : MonoBehaviour
{
    public Vector3 snapLocation;
    public int itemCost;
    Vector3 mousePosition;
    public bool bought;
    public int resaleValue;

    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }
    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition - GetMousePos();
    }
    private void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
    }
    private void OnMouseUp()
    {
        transform.position = snapLocation;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (isDragging)
    //    {
    //        //https://youtu.be/pFpK4-EqHXQ do this 
    //        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    }
    //}

    //public void Drag()
    //{
    //    isDragging = true;
    //}
}
