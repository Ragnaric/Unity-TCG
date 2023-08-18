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
                if (opponentCardPoints[i].activeCard != null)
                {
                    opponentCardPoints[i].activeCard.DamageCard(playerCardPoints[i].activeCard.attackPower);
                    playerCardPoints[i].activeCard.DamageCard(opponentCardPoints[i].activeCard.attackPower);
                }
                else
                {
                    Debug.Log("Direct Attack");
                    //attack enemy health points
                }
                yield return new WaitForSeconds(attackDelay);
            }
        }
        BattleController.instance.AdvanceTurn();
    }
}
