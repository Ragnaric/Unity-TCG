using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card", order = 1)]

public class CardScriptableObject : ScriptableObject
{
    public string cardName;

    [TextArea]
    public string descriptionText, flavorText;

    public int attackPower, currentHealth, manaCost;

    public Sprite characterSprite, bgSprite;
}
