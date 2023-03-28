using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    public CardScriptableObject cardSO;

    public int attackPower, currentHealth, manaCost;

    public TMP_Text attackText, defText, manaText;
    // Start is called before the first frame update
    void Start()
    {
        attackText.text = attackPower.ToString();
        defText.text = currentHealth.ToString();
        manaText.text = manaCost.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}