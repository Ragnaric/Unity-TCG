using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    public static DeckController instance;

    private void Awake()
    {
        instance = this;
    }

    public List<CardScriptableObject> deck = new List<CardScriptableObject>();
    private List<CardScriptableObject> activeCards = new List<CardScriptableObject>();

    public Card cardSpawn;

    public int drawCardCost = 1;
    // Start is called before the first frame update
    void Start()
    {
        SetupDeck();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     DrawCard();
        // }
    }

    public void SetupDeck()
    {
        activeCards.Clear();
        List<CardScriptableObject> tempDeck = new List<CardScriptableObject>();
        tempDeck.AddRange(deck);
        while (tempDeck.Count > 0)
        {
            int selected = Random.Range(0, tempDeck.Count);
            activeCards.Add(tempDeck[selected]);
            tempDeck.RemoveAt(selected);
        }
    }

    public void DrawCard()
    {
        if (activeCards.Count == 0)
        {
            SetupDeck();
        }
        Card newCard = Instantiate(cardSpawn, transform.position, transform.rotation);
        // newCard.StartFlip();
        // newCard.FlipCard();
        newCard.cardSO = activeCards[0];
        newCard.SetupCard();
        activeCards.RemoveAt(0);
        HandController.instance.AddCardHand(newCard);
    }

    public void DrawCardForMana()
    {
        if (BattleController.instance.playerMana >= drawCardCost)
        {
            DrawCard();
            BattleController.instance.spendMana(drawCardCost);
        }
        else
        {
            UIController.instance.ShowManaWarning();
        }
    }

    public void StartFirstDraw(int amount)
    {
        StartCoroutine(FirstDraw(amount));
    }

    IEnumerator FirstDraw(int amount)
    {
        while (amount != 0)
        {
            DrawCard();
            amount--;
            yield return new WaitForSeconds(.3f);
        }
    }
}
