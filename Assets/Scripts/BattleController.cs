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
    public int playerMana, opponentMana;
    public int turn = 1;

    public int startingCards = 5;

    public enum TurnOrder { playerMainPhase, playerAttackPhase, enemyMainPhase, enemyAttackPhase }
    public TurnOrder currentPhase;

    public Transform discardPoint;

    public int playerLifePoints = 20;
    public int opponentLifePoints = 20;

    // Start is called before the first frame update
    void Start()
    {
        UIController.instance.SetTurnText(turn);
        updateMana();
        UpdatePlayerLife();
        UpdateOpponentLife();
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

    public void spendOpponentMana(int amount)
    {
        opponentMana = opponentMana - amount;

        if (opponentMana < 0)
        {
            opponentMana = 0;
        }
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

    public void updateOpponentMana()
    {
        if (initialMana + (turn - 1) < maxMana)
        {
            opponentMana = initialMana + (turn - 1);
            UIController.instance.SetOpponentManaText(opponentMana);
        }
    }

    public void UpdatePlayerLife()
    {
        UIController.instance.SetPlayerLifeText(playerLifePoints);
    }

    public void UpdateOpponentLife()
    {
        UIController.instance.SetOpponentLifeText(opponentLifePoints);
    }

    public void AdvanceTurn()
    {
        currentPhase++;

        if ((int)currentPhase >= System.Enum.GetValues(typeof(TurnOrder)).Length)
        {
            currentPhase = 0;
        }

        switch (currentPhase)
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
                OpponentController.instance.StartOpponentTurn();
                break;

            case TurnOrder.enemyAttackPhase:
                Debug.Log(currentPhase);
                CardPointsController.instance.OpponentAttack();
                break;
        }
    }

    public void DamageLifePoints(int amount)
    {
        if (playerLifePoints > 0)
        {
            playerLifePoints -= amount;
            if (playerLifePoints <= 0)
            {
                playerLifePoints = 0;
                //End battle
            }
        }
        UpdatePlayerLife();
        UIDamageIndicator damageClone = Instantiate(UIController.instance.playerDamage, UIController.instance.playerDamage.transform.parent);
        damageClone.damageTaken.text = (amount * -1).ToString();
        damageClone.gameObject.SetActive(true);
    }

    public void DamageOpponentLifePoints(int amount)
    {
        if (opponentLifePoints > 0)
        {
            opponentLifePoints -= amount;
            if (opponentLifePoints < 0)
            {
                opponentLifePoints = 0;
                //End battle
            }
        }
        UpdateOpponentLife();
        UIDamageIndicator damageClone = Instantiate(UIController.instance.opponentDamage, UIController.instance.opponentDamage.transform.parent);
        damageClone.damageTaken.text = (amount * -1).ToString();
        damageClone.gameObject.SetActive(true);
    }
}
