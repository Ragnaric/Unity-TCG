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
    public Transform minPos, maxPos;
    public List<Vector3> cardPositions = new List<Vector3>();

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

        Vector3 distanceBetweenPoints = Vector3.zero;
        if (heldCards.Count > 1)
        {
            distanceBetweenPoints = (maxPos.position - minPos.position) / (heldCards.Count - 1);

        }

        for (int i = 0; i < heldCards.Count; i++)
        {
            cardPositions.Add(minPos.position + (distanceBetweenPoints * i + Vector3.up * (i * 0.005f)));
            heldCards[i].FlipCard();
            heldCards[i].MoveToPoint(cardPositions[i]);
            heldCards[i].inHand = true;
            heldCards[i].handPosition = i;
        }
    }

    //this feature not working as intended, will get back to it later
    public void RaiseHand()
    {
        cardPositions.Clear();

        Vector3 distanceBetweenPoints = Vector3.zero;
        if (heldCards.Count > 1)
        {
            distanceBetweenPoints = (maxPos.position - minPos.position) / (heldCards.Count - 1);

        }

        for (int i = 0; i < heldCards.Count; i++)
        {
            cardPositions.Add(minPos.position + (distanceBetweenPoints * i + Vector3.up * (i * 0.005f)) + Vector3.forward);
            heldCards[i].MoveToPoint(cardPositions[i]);
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