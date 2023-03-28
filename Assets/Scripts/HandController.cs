using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public Card[] heldCards;
    public Transform minPos, maxPos;
    // Start is called before the first frame update
    void Start()
    {
        SetCardPositionsHand();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCardPositionsHand();
}
