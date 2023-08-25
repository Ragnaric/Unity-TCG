using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public float manaWarningTime;
    private float manaWarningCounter;

    private void Awake()
    {
        instance = this;
    }

    public TMP_Text playerManaText;
    public TMP_Text turnNumber;
    public TMP_Text playerLifeTotal;
    public TMP_Text opponentLifeTotal;

    public GameObject manaWarning, endTurnButton;

    public UIDamageIndicator playerDamage, opponentDamage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (manaWarningCounter > 0)
        {
            manaWarningCounter -= Time.deltaTime;
        }
        if (manaWarningCounter <= 0)
        {
            manaWarning.SetActive(false);
        }
    }

    public void SetPlayerManaText(int amount)
    {
        playerManaText.text = "Mana: " + amount;
    }

    public void SetTurnText(int amount)
    {
        turnNumber.text = "Turn: " + amount;
    }

    public void SetPlayerLifeText(int amount)
    {
        playerLifeTotal.text = amount.ToString();
    }

    public void SetOpponentLifeText(int amount)
    {
        opponentLifeTotal.text = amount.ToString();
    }

    public void ShowManaWarning()
    {
        manaWarning.SetActive(true);
        manaWarningCounter = manaWarningTime;
    }

    public void EndPhase()
    {
        //DeckController.instance.DrawCardForMana();
        BattleController.instance.AdvanceTurn();
    }
}
