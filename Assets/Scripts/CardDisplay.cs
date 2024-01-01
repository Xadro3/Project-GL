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
        card = GetComponent<Card>();
        UpdateDisplay();
    }

    public void SetupDisplay()
    {
        nameText.text = card.cardInfo.name;
        descriptionText.text = card.cardDescription;

        artworkImage.sprite = card.cardInfo.artwork;

        costText.text = card.cost.ToString();
        durabilityText.text = card.durabilityCurrent.ToString();
    }

    public void UpdateDisplay()
    {
        descriptionText.text = card.cardDescription;

        costText.text = card.cost.ToString();
        durabilityText.text = card.durabilityCurrent.ToString();
    }

    public void UpdateDurability(int value)
    {
        durabilityText.text = value.ToString();
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
