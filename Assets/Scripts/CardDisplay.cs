using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    private Card card;

    public TextMeshPro nameText;
    public TextMeshPro descriptionText;

    public SpriteRenderer artworkImage;
    
    public TextMeshPro costText;
    public TextMeshPro durabilityText;
    public TextMeshPro repairText;

    public SpriteRenderer shieldIcon;
    public SpriteRenderer entsorgenIcon;


    // Start is called before the first frame update
    void Start()
    {
        card = GetComponentInParent<Card>();
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        nameText.text = card.cardInfo.name;
        descriptionText.text = card.cardInfo.description;

        artworkImage.sprite = card.cardInfo.artwork;

        costText.text = card.cost.ToString();
        durabilityText.text = card.durability.ToString();
    }


    public void ActivateShieldIcon(bool b)
    {
        shieldIcon.gameObject.SetActive(b);
    }

    public void ActivateEntsorgenIcon(bool b)
    {
        entsorgenIcon.gameObject.SetActive(b);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
