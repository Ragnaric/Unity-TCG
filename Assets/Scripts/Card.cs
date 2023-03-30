using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardScriptableObject cardSO;

    public int attackPower, currentHealth, manaCost;

    public TMP_Text attackText, defText, manaText, cardName, descriptionText, flavorText;

    public Image characterArt, bgArt;

    private Vector3 targetPoint;
    public float moveSpeed = 5f;

    public bool inHand;
    public int handPosition;

    private HandController theHC;

    // Start is called before the first frame update
    void Start()
    {
        SetupCard();
        theHC = FindObjectOfType<HandController>();
    }

    public void SetupCard()
    {
        attackPower = cardSO.attackPower;
        currentHealth = cardSO.currentHealth;
        manaCost = cardSO.manaCost;

        attackText.text = attackPower.ToString();
        defText.text = currentHealth.ToString();
        manaText.text = manaCost.ToString();

        cardName.text = cardSO.cardName;
        descriptionText.text = cardSO.descriptionText;
        flavorText.text = cardSO.flavorText;

        characterArt.sprite = cardSO.characterSprite;
        bgArt.sprite = cardSO.bgSprite;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPoint, moveSpeed * Time.deltaTime);
    }

    public void MoveToPoint(Vector3 destinationPoint)
    {
        targetPoint = destinationPoint;
    }

    void OnMouseOver()
    {
        if (inHand)
        {
            MoveToPoint(theHC.cardPositions[handPosition] + new Vector3(0f, 1f, 0f));
        }
    }
}