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
    
    public List<GameConstants.radiationTypes> protectionTypes;

    public string info;


}
