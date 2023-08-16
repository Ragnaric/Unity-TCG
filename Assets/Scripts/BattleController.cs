using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController instance;

    private void Awake()
    {
        instance = this;
    }

    public int initialMana = 3, maxMana = 12;
    public int playerMana;
    public int turn = 1;

    public int startingCards = 5;

    public enum TurnOrder { playerMainPhase, playerAttackPhase, enemyMainPhase, enemyAttackPhase }
    public TurnOrder currentPhase;

    // Start is called before the first frame update
    void Start()
    {
        UIController.instance.SetTurnText(turn);
        updateMana();
        DeckController.instance.StartFirstDraw(startingCards);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spendMana(int amount)
    {
        playerMana = playerMana - amount;

        if (playerMana < 0)
        {
            playerMana = 0;
        }

        UIController.instance.SetPlayerManaText(playerMana);
    }

    public void updateTurn()
    {
        turn += 1;
        UIController.instance.SetTurnText(turn);
    }

    public void updateMana()
    {
        if (initialMana + (turn - 1) < maxMana)
        {
            playerMana = initialMana + (turn - 1);
            UIController.instance.SetPlayerManaText(playerMana);
        }
        else
        {
            playerMana = maxMana;
            UIController.instance.SetPlayerManaText(playerMana);
        }
    }

    public void AdvanceTurn()
    {
        currentPhase++;

        if ((int)currentPhase >= System.Enum.GetValues(typeof(TurnOrder)).Length)
        {
            currentPhase = 0;
        }

        switch(currentPhase)
        {
            case TurnOrder.playerMainPhase:
                Debug.Log(currentPhase);
                updateTurn();
                updateMana();
                DeckController.instance.DrawCard();
                break;

            case TurnOrder.playerAttackPhase:
                Debug.Log(currentPhase);
                CardPointsController.instance.PlayerAttack();
                break;

            case TurnOrder.enemyMainPhase:
                Debug.Log(currentPhase);
                break;

            case TurnOrder.enemyAttackPhase:
                Debug.Log(currentPhase);
                break;
        }
    }
}
