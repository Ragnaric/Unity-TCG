using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardScriptableObject cardSO;

    public bool isPlayer;

    public int attackPower, currentHealth, manaCost;

    public TMP_Text attackText, defText, manaText, cardName, descriptionText, flavorText;

    public Image characterArt, bgArt;

    private Vector3 targetPoint;
    private Quaternion targetRotation;
    public float moveSpeed = 2f, rotateSpeed = 540f;

    public bool inHand;
    public int handPosition;

    //references to other objects/components
    private HandController theHC;
    private BattleController theBC;

    private bool isSelected;
    private bool justPressed;
    private Collider theCol;

    public LayerMask desktopLayer, handLayer, placementLayer;

    public CardPlacePoint assignedPlace;

    public Animator animate;

    // Start is called before the first frame update
    void Start()
    {
        if (targetPoint == Vector3.zero)
        {
            targetPoint = transform.position;
        }

        SetupCard();
        theHC = FindObjectOfType<HandController>();
        theCol = GetComponent<Collider>();
    }

    public void SetupCard()
    {
        attackPower = cardSO.attackPower;
        currentHealth = cardSO.currentHealth;
        manaCost = cardSO.manaCost;

        // attackText.text = attackPower.ToString();
        // defText.text = currentHealth.ToString();
        // manaText.text = manaCost.ToString();
        UpdateCardDisplay();

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
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        if (isSelected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, desktopLayer))
            Debug.Log(hit.point);
            {
                MoveToPoint(hit.point + new Vector3(0f, 2f, 0.5f), Quaternion.identity);
            }

            if (Input.GetMouseButtonDown(1))
            {
                ReturnToHand();
            }

            if (Input.GetMouseButtonDown(0) && justPressed == false)
            {
                if (Physics.Raycast(ray, out hit, 100f, placementLayer) && BattleController.instance.currentPhase == BattleController.TurnOrder.playerMainPhase)
                {
                    CardPlacePoint selectedPoint = hit.collider.GetComponent<CardPlacePoint>();

                    if (selectedPoint.activeCard == null && selectedPoint.isPlayerPoint)
                    {
                        if (BattleController.instance.playerMana >= manaCost)
                        {
                            selectedPoint.activeCard = this;
                            assignedPlace = selectedPoint;

                            MoveToPoint(selectedPoint.transform.position, Quaternion.identity);

                            inHand = false;
                            isSelected = false;
                            theHC.RemoveCardHand(this);
                            BattleController.instance.spendMana(manaCost);
                        } else
                        {
                            UIController.instance.ShowManaWarning();
                            ReturnToHand();
                        }
                    }
                    else
                    {
                        ReturnToHand();
                    }
                }
                else
                {
                    ReturnToHand();
                }
            }
        }
        justPressed = false;

        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     StartFlip();
        // }
    }

    public void MoveToPoint(Vector3 destinationPoint, Quaternion intendedRotation)
    {
        targetPoint = destinationPoint;
        targetRotation = intendedRotation;
    }

    //this coroutine seems to be obsolete after refactoring MoveToPoint to accept a rotation parameter
    public void StartFlip()
    {
        StartCoroutine(FlipCard());
    }

    public IEnumerator FlipCard()
    {
        for (int i = 0; i < 90; i++)
        {
            yield return new WaitForSeconds(0.001f);
            transform.Rotate(new Vector3 (0f, 0f, 2f));
        }
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
        if (inHand && isPlayer)
        {
            MoveToPoint(currentPosition + new Vector3(-shiftAxisX, 2f, 2.5f), Quaternion.identity);
        }
    }

    void OnMouseExit()
    {
        if (inHand && isPlayer)
        {
            MoveToPoint(theHC.cardPositions[handPosition], Quaternion.identity);
        }
    }

    void OnMouseDown()
    {
        if (inHand && BattleController.instance.currentPhase == BattleController.TurnOrder.playerMainPhase)
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
        MoveToPoint(theHC.cardPositions[handPosition], Quaternion.identity);
    }

    public void DamageCard(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            assignedPlace.activeCard = null;
            animate.SetTrigger("Jump");
            MoveToPoint(BattleController.instance.discardPoint.position, BattleController.instance.discardPoint.rotation);
            this.StartFlip();
            this.FlipCard();
            Destroy(gameObject, 3f);
        }
        //animate.SetTrigger("Damage");
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        attackText.text = attackPower.ToString();
        defText.text = currentHealth.ToString();
        manaText.text = manaCost.ToString();
    }
}