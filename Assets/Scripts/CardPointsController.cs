using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPointsController : MonoBehaviour
{
    public static CardPointsController instance;

    private void Awake()
    {
        instance = this;
    }

    public CardPlacePoint[] playerCardPoints, opponentCardPoints;
    public float attackDelay = 0.5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerAttack()
    {
        StartCoroutine(attackCoroutine());
    }

    IEnumerator attackCoroutine()
    {
        yield return new WaitForSeconds(attackDelay);

        for (int i = 0; i < playerCardPoints.Length; i++)
        {
            if (playerCardPoints[i].activeCard != null)
            {
                int playerATK = playerCardPoints[i].activeCard.attackPower;
                playerCardPoints[i].activeCard.animate.SetTrigger("Attack");

                if (opponentCardPoints[i].activeCard != null)
                {
                    int opponentATK = opponentCardPoints[i].activeCard.attackPower;
                    opponentCardPoints[i].activeCard.animate.SetTrigger("Damage");
                    opponentCardPoints[i].activeCard.DamageCard(playerATK);
                    playerCardPoints[i].activeCard.DamageCard(opponentATK);
                }
                else
                {
                    Debug.Log("Direct Attack");
                    //attack enemy health points
                }
                yield return new WaitForSeconds(attackDelay);
            }
        }
        CheckAssignedCards();
        BattleController.instance.AdvanceTurn();
    }

    public void OpponentAttack()
    {
        StartCoroutine(opponentAttackCoroutine());
    }

    IEnumerator opponentAttackCoroutine()
    {
        yield return new WaitForSeconds(attackDelay);

        for (int i = 0; i < opponentCardPoints.Length; i++)
        {
            if (opponentCardPoints[i].activeCard != null)
            {
                int opponentATK = opponentCardPoints[i].activeCard.attackPower;
                opponentCardPoints[i].activeCard.animate.SetTrigger("Opponent Attack");

                if (playerCardPoints[i].activeCard != null)
                {
                    int playerATK = playerCardPoints[i].activeCard.attackPower;
                    playerCardPoints[i].activeCard.animate.SetTrigger("Damage");
                    playerCardPoints[i].activeCard.DamageCard(opponentATK);
                    opponentCardPoints[i].activeCard.DamageCard(playerATK);
                }
                else
                {
                    Debug.Log("Direct Attack");
                    //attack player health points
                }
                yield return new WaitForSeconds(attackDelay);
            }
        }
        CheckAssignedCards();
        BattleController.instance.AdvanceTurn();
    }

    public void CheckAssignedCards()
    {
        foreach(CardPlacePoint point in opponentCardPoints)
        {
            if (point.activeCard != null)
            {
                if (point.activeCard.currentHealth <= 0)
                {
                    point.activeCard = null;
                }
            }
        }

        foreach(CardPlacePoint point in playerCardPoints)
        {
            if (point.activeCard != null)
            {
                if (point.activeCard.currentHealth <= 0)
                {
                    point.activeCard = null;
                }
            }
        }
    }
}
