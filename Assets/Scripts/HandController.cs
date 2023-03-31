using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public List<Card> heldCards = new List<Card>();
    public Transform minPos, maxPos;
    public List<Vector3> cardPositions = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        SetCardPositionsHand();
    }

    // Update is called once per frame
    void Update()
    {

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
            heldCards[i].MoveToPoint(cardPositions[i]);
            heldCards[i].inHand = true;
            heldCards[i].handPosition = i;
        }
    }

    void OnMouseEnter()
    {
        for (int i = 0; i < cardPositions.Count; i++)
        {
            cardPositions[i] += new Vector3(0f, 0f, 1f);
            heldCards[i].MoveToPoint(cardPositions[i]);
        }
    }

    void OnMouseExit()
    {
        for (int i = 0; i < cardPositions.Count; i++)
        {
            cardPositions[i] -= new Vector3(0f, 0f, 1f);
            heldCards[i].MoveToPoint(cardPositions[i]);
        }
    }
}