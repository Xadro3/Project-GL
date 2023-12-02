using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class SO_Card : ScriptableObject
{
    public new string name;
    public string description;

    public Sprite artwork;

    public int cost;
    public int durability;
    public int repair;


    public List<GameConstants.cardTypes> cardArchetypes;
    public List<GameConstants.radiationTypes> protectionTypes;
    public bool ability;
    public int duration;
    public List<GameConstants.abilityTargets> abilityTypes;
    public bool effect;
    public bool bruch;
    public List<int> effectValues;
    public List<GameConstants.effectTypes> effectTypes;
    public bool immunity;
    public List<GameConstants.radiationTypes> immunityTypes;
    public bool entsorgen;



    public string cardInfo;




}
