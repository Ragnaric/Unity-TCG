using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public static HandController instance;

    private void Awake()
    {
        instance = this;
    }

    public List<Card> heldCards = new List<Card>();
    public List<Card> opponentCards = new List<Card>();
    public Transform minPos, maxPos;
    public Transform minPos2, maxPos2;
    public List<Vector3> cardPositions = new List<Vector3>();
    public List<Vector3> opponentPositions = new List<Vector3>();

    public LayerMask desktopLayer;
    public LayerMask handLayer;
    private Card theCard;

    // Start is called before the first frame update
    void Start()
    {
        SetCardPositionsHand();
        theCard = FindObjectOfType<Card>();
    }

    // Update is called once per frame
    void Update()
    {
        // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // RaycastHit hit;
        // if (Physics.Raycast(ray, out hit, 100f, handLayer))
        // {
        //     RaiseHand();
        // }
        // else
        // {
        //     SetCardPositionsHand();
        // }
    }

    public void SetCardPositionsHand()
    {
        cardPositions.Clear();
        Vector3 center = new Vector3((maxPos.position.x + minPos.position.x), maxPos.position.y, maxPos.position.z);

        Vector3 shiftDistance = Vector3.left * 0.75f;
        for (int i = 0; i < heldCards.Count; i++)
        {
            if (heldCards.Count % 2 == 0)
            {
                float middlePoint = (heldCards.Count / 2) - 0.5f;
                cardPositions.Add(center + (shiftDistance * (middlePoint - i) + Vector3.up * (i * 0.005f)));
                heldCards[i].MoveToPoint(cardPositions[i], Quaternion.identity);
                heldCards[i].inHand = true;
                heldCards[i].handPosition = i;
            }
            else
            {
                float middlePoint = heldCards.Count / 2;
                cardPositions.Add(center + (shiftDistance * (middlePoint - i) + Vector3.up * (i * 0.005f)));
                heldCards[i].MoveToPoint(cardPositions[i], Quaternion.identity);
                heldCards[i].inHand = true;
                heldCards[i].handPosition = i;
            }
        }
    }

    public void SetOpponentHand()
    {
        opponentPositions.Clear();
        Vector3 center = new Vector3((maxPos2.position.x + minPos2.position.x), maxPos2.position.y, maxPos2.position.z);

        Vector3 shiftDistance = Vector3.left * 0.75f;
        for (int i = 0; i < opponentCards.Count; i++)
        {
            if (opponentCards.Count % 2 == 0)
            {
                float middlePoint = (opponentCards.Count / 2) - 0.5f;
                opponentPositions.Add(center + (shiftDistance * (middlePoint - i) + Vector3.up * (i * 0.005f)));
                opponentCards[i].MoveToPoint(opponentPositions[i], minPos2.rotation);
                //opponentCards[i].inHand = true;
                opponentCards[i].handPosition = i;
            }
            else
            {
                float middlePoint = opponentCards.Count / 2;
                opponentPositions.Add(center + (shiftDistance * (middlePoint - i) + Vector3.up * (i * 0.005f)));
                opponentCards[i].MoveToPoint(opponentPositions[i], minPos2.rotation);
                //opponentCards[i].inHand = true;
                opponentCards[i].handPosition = i;
            }
        }
    }

    //this feature not working as intended, will get back to it later
    public void RaiseHand()
    {
        cardPositions.Clear();

        Vector3 distanceBetweenCards = Vector3.zero;
        if (heldCards.Count > 1)
        {
            distanceBetweenCards = (maxPos.position - minPos.position) / (heldCards.Count - 1);

        }

        for (int i = 0; i < heldCards.Count; i++)
        {
            cardPositions.Add(minPos.position + (distanceBetweenCards * i + Vector3.up * (i * 0.005f)) + Vector3.forward);
            heldCards[i].MoveToPoint(cardPositions[i], Quaternion.identity);
        }
    }

    public void RemoveCardHand(Card cardToRemove)
    {
        if (heldCards[cardToRemove.handPosition] == cardToRemove)
        {
            heldCards.RemoveAt(cardToRemove.handPosition);
        } else
        {
            Debug.LogError("Card at position " + cardToRemove.handPosition + " is not the card being removed from hand");
        }
        SetCardPositionsHand();
    }

    public void AddCardHand(Card cardToAdd)
    {
        heldCards.Add(cardToAdd);
        SetCardPositionsHand();
    }

    public void AddOpponentHand(Card cardToAdd)
    {
        opponentCards.Add(cardToAdd);
        SetOpponentHand();
    }

    // public void MoveHandController(Vector3 destination)
    // {
    //     theHC.transform.position = destination;
    // }

    // void OnMouseEnter()
    // {
    //     for (int i = 0; i < heldCards.Count; i++)
    //     {
    //         cardPositions[i] = cardPositions[i] + new Vector3 (0f, 0f, 0.75f);
    //         heldCards[i].MoveToPoint(cardPositions[i]);
    //     }
    // }

    // void OnMouseExit()
    // {
    //     SetCardPositionsHand();
    // }
}