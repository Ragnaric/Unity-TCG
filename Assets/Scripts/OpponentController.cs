using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentController : MonoBehaviour
{
    public static OpponentController instance;

    private void Awake()
    {
        instance = this;
    }

    public List<CardScriptableObject> opponentDeck = new List<CardScriptableObject>();
    private List<CardScriptableObject> activeCards = new List<CardScriptableObject>();

    public Card cardSpawn;
    public Transform cardSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        SetupDeck();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetupDeck()
    {
        activeCards.Clear();
        List<CardScriptableObject> tempDeck = new List<CardScriptableObject>();
        tempDeck.AddRange(opponentDeck);
        while (tempDeck.Count > 0)
        {
            int selected = Random.Range(0, tempDeck.Count);
            activeCards.Add(tempDeck[selected]);
            tempDeck.RemoveAt(selected);
        }
    }

    public void StartOpponentTurn()
    {
        StartCoroutine(OpponentTurn());
    }

    IEnumerator OpponentTurn()
    {
        if (activeCards.Count == 0)
        {
            SetupDeck();
        }
        yield return new WaitForSeconds(1f);

        List<CardPlacePoint> cardPoints = new List<CardPlacePoint>();
        cardPoints.AddRange(CardPointsController.instance.opponentCardPoints);

        int randomPoint = Random.Range(0, cardPoints.Count);
        CardPlacePoint selectedPoint = cardPoints[randomPoint];
        while (selectedPoint.activeCard != null && cardPoints.Count > 0)
        {
            randomPoint = Random.Range(0, cardPoints.Count);
            selectedPoint = cardPoints[randomPoint];
            cardPoints.RemoveAt(randomPoint);
        }
        if (selectedPoint.activeCard == null)
        {
            Card newCard = Instantiate(cardSpawn, cardSpawnPoint.position, cardSpawnPoint.rotation);
            newCard.cardSO = activeCards[0];
            activeCards.RemoveAt(0);
            newCard.SetupCard();
            newCard.MoveToPoint(selectedPoint.transform.position, selectedPoint.transform.rotation);
            selectedPoint.activeCard = newCard;
            newCard.assignedPlace = selectedPoint;
        }

        BattleController.instance.AdvanceTurn();
    }
}
