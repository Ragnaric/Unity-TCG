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

    public int initialMana = 2, maxMana = 12;
    public int playerMana;

    // Start is called before the first frame update
    void Start()
    {
        playerMana = initialMana;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spendMana(int amount)
    {
        playerMana = playerMana - amount;

        if (playerMana < 0) {
            playerMana = 0;
        }
    }
}
