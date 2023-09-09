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

    public enum AItype { single, multiple, defensive, aggro }
    public AItype opponentType;

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

    public void OpponentDraw()
    {
        if (activeCards.Count == 0)
        {
            SetupDeck();
        }
        Card newCard = Instantiate(cardSpawn, cardSpawnPoint.position, cardSpawnPoint.rotation);
        newCard.cardSO = activeCards[0];
        activeCards.RemoveAt(0);
        HandController.instance.AddOpponentHand(newCard);
    }

    public void StartOpponentTurn()
    {
        StartCoroutine(OpponentTurn());
    }

    IEnumerator OpponentTurn()
    {
        Debug.Log("opponent type: " + opponentType);
        if (activeCards.Count == 0)
        {
            SetupDeck();
        }
        yield return new WaitForSeconds(1f);

        List<CardPlacePoint> cardPoints = new List<CardPlacePoint>();
        cardPoints.AddRange(CardPointsController.instance.opponentCardPoints);

        int randomPoint = Random.Range(0, cardPoints.Count);
        CardPlacePoint selectedPoint = cardPoints[randomPoint];

        List<Card> opponentCards = HandController.instance.opponentCards;

        if (opponentType == AItype.single)
        {
            cardPoints.RemoveAt(randomPoint);

            while (selectedPoint.activeCard != null && cardPoints.Count > 0)
            {
                randomPoint = Random.Range(0, cardPoints.Count);
                selectedPoint = cardPoints[randomPoint];
                cardPoints.RemoveAt(randomPoint);
            }
        }

        switch (opponentType)
        {
            case AItype.single:
                OpponentDraw();
                yield return new WaitForSeconds(.75f);

                for (int i = 0; i < opponentCards.Count; i++)
                {
                    Debug.Log("this is the mana cost: " + opponentCards[i].manaCost);
                    if (opponentCards[i].manaCost <= BattleController.instance.opponentMana)
                    {
                        if (selectedPoint.activeCard == null)
                        {
                        opponentCards[i].MoveToPoint(selectedPoint.transform.position, Quaternion.identity);
                        selectedPoint.activeCard = opponentCards[i];
                        opponentCards[i].assignedPlace = selectedPoint;
                        BattleController.instance.spendOpponentMana(opponentCards[i].manaCost);
                        opponentCards.RemoveAt(i);
                        }
                        break;
                    }
                }
                break;

            case AItype.multiple:
                OpponentDraw();
                yield return new WaitForSeconds(.75f);

                //iterate through cards in hand
                for (int i = 0; i < opponentCards.Count; i++)
                {
                    //iterate through the card points
                    for (int j = 0; j < cardPoints.Count; j++)
                    {
                        //if the mana cost allows
                        if (opponentCards[i].manaCost <= BattleController.instance.opponentMana)
                        {
                            //if there is a point available
                            if (cardPoints[j].activeCard == null)
                            {
                                //play the card
                                opponentCards[i].MoveToPoint(cardPoints[j].transform.position, Quaternion.identity);
                                cardPoints[j].activeCard = opponentCards[i];
                                opponentCards[i].assignedPlace = cardPoints[j];
                                BattleController.instance.spendOpponentMana(opponentCards[i].manaCost);
                                HandController.instance.RemoveOpponentHand(opponentCards[i]);
                            }
                        }
                    }
                }
                break;

            case AItype.defensive:
                break;

            case AItype.aggro:
                break;
        }

        yield return new WaitForSeconds(1f);

        BattleController.instance.AdvanceTurn();
    }

    public void StartFirstDraw(int amount)
    {
        StartCoroutine(FirstDraw(amount));
    }

    IEnumerator FirstDraw(int amount)
    {
        while (amount != 0)
        {
            OpponentDraw();
            amount--;
            yield return new WaitForSeconds(.3f);
        }
    }

    public void switchAI()
    {
        opponentType++;
    }
}
