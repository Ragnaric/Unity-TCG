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

    private bool isSelected;
    private bool justPressed;
    private Collider theCol;

    public LayerMask desktopLayer, handLayer, placementLayer;

    // Start is called before the first frame update
    void Start()
    {
        SetupCard();
        theHC = FindObjectOfType<HandController>();
        theCol = GetComponent<Collider>();
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
        if (isSelected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, desktopLayer))
            Debug.Log(hit.point);
            {
                MoveToPoint(hit.point + new Vector3(0f, 2f, 0.5f));
            }

            if (Input.GetMouseButtonDown(1))
            {
                ReturnToHand();
            }

            if (Input.GetMouseButtonDown(0) && justPressed == false)
            {
                if (Physics.Raycast(ray, out hit, 100f, placementLayer))
                {

                }
                else
                {
                    ReturnToHand();
                }
            }
        }
        justPressed = false;
    }

    public void MoveToPoint(Vector3 destinationPoint)
    {
        targetPoint = destinationPoint;
    }

    void OnMouseOver()
    {
        float center;
        if (theHC.heldCards.Count % 2 == 0)
        {
            center = theHC.heldCards.Count / 2;
        }
        else
        {
            center = Mathf.Floor(theHC.heldCards.Count / 2);
        }
        Vector3 currentPosition = theHC.cardPositions[handPosition];
        float shiftAxisX = currentPosition.x / center;
        if (inHand)
        {
            MoveToPoint(currentPosition + new Vector3(-shiftAxisX, 2f, 2.5f));
        }
    }

    void OnMouseExit()
    {
        if (inHand)
        {
            MoveToPoint(theHC.cardPositions[handPosition]);
        }
    }

    void OnMouseDown()
    {
        if (inHand)
        {
            isSelected = true;
            theCol.enabled = false;
            justPressed = true;
        }
    }

    public void ReturnToHand()
    {
        isSelected = false;
        theCol.enabled = true;
        MoveToPoint(theHC.cardPositions[handPosition]);
    }
}