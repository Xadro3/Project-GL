using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class SO_Card : ScriptableObject
{
    [Header("General Info")]
    public new string name;
    [TextAreaAttribute]
    public string description;
    public Sprite artwork;
    public bool upgraded;
    public SO_Card upgradedCardInfo;

    [Header("Shop Info")]
    public int currencyCost;

    [Header("Card Stats")]
    public int cost;
    public bool energyCostAffected = false;
    public int energyCostIncrease = 0;
    public int durability;
    public int durabilityDebuffValue = 0;

    public List<GameConstants.cardType> cardType;
    public List<GameConstants.cardRarity> cardRarity;
    public List<GameConstants.radiationTypes> protectionTypes;

    [Header("Ability Info")]
    public bool ability;
    public int duration;
    public List<GameConstants.abilityTargets> abilityTypes;

    [Header("Effect Info")]
    public bool effect;
    public bool onBruch;
    public bool onPlay;
    public List<int> effectValues;
    public List<GameConstants.effectTypes> effectTypes;
    
    [Header("Immunities for shields")]
    public bool immunity;
    public List<GameConstants.radiationTypes> immunityTypes;

    [Header("Entsorgen")]
    public bool entsorgen;

    [Header("Wissenschaftliche Info")]
    [TextAreaAttribute]
    public string cardInfo;




}
