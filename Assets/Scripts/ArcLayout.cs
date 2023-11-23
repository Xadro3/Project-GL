using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcLayout : MonoBehaviour
{
    public float arcRadius = 2f;
    public float arcAngle = 90f;
    private int cardCount = 0;

    public int xSpacing = 0;
    public int ySpacing = 0;

    public float maxTiltAngle = 15f;

    private SpriteRenderer[] spriteRenderers;
    private MeshRenderer[] meshRenderers;
    private List<Card> cards;

    void Start()
    {

    }


    void Update()
    {
        if (cardCount == transform.childCount)
        {
            SetSortingOrder();
        }
        else
        {
            cardCount = transform.childCount;
        }
        
        //ArrangeCardsInArc();
    }

    private void SetSortingOrder()
    {
        for (int i = 0; i < cardCount; i++)
        {
            Transform card = transform.GetChild(i);
            if (card.GetComponent<Card>())
            {
                card.GetComponent<SpriteRenderer>().sortingOrder = i;
                SpriteRenderer[] spriteRenderers = card.GetComponentsInChildren<SpriteRenderer>();
                MeshRenderer[] meshRenderers = card.GetComponentsInChildren<MeshRenderer>();
                foreach (SpriteRenderer renderer in spriteRenderers)
                {
                    renderer.sortingOrder = card.GetComponent<SpriteRenderer>().sortingOrder + 1;
                }
                foreach (MeshRenderer renderer in meshRenderers)
                {
                    renderer.sortingOrder = card.GetComponent<SpriteRenderer>().sortingOrder + 1;
                }
            }
            
        }
        
    }

    void ArrangeCardsInArc()
    {
        if (cardCount == 0)
        {
            return;
        }

        float angleStep = arcAngle / (cardCount);
        float startAngle = arcAngle / 4f;

        for (int i = 0; i < cardCount; i++)
        {
            Transform card = transform.GetChild(i);
            card.GetComponent<SpriteRenderer>().sortingOrder = i;
            

            float angle = startAngle + i * angleStep;
            float x = arcRadius * Mathf.Cos(Mathf.Deg2Rad * angle);
            float y = arcRadius * Mathf.Sin(Mathf.Deg2Rad * angle);
            ySpacing = i * 1;

            card.localPosition = new Vector3(x + xSpacing, y + ySpacing, 0f);

            // Tilt the card
            float tiltAngle = Mathf.Clamp(angle, -maxTiltAngle, maxTiltAngle);
            if (i > Mathf.Round(cardCount / 2))
            {
                card.localRotation = Quaternion.Euler(0f, 0f, tiltAngle);
            }
            if (i == Mathf.Round(cardCount / 2))
            {
                card.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
            if (i < Mathf.Round(cardCount / 2))
            {
                card.localRotation = Quaternion.Euler(0f, 0f, -tiltAngle);
            }
            
        }
    }
}
