using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    private void Awake()
    {
        instance = this;
    }

    public TMP_Text playerManaText;

    public GameObject manaWarning;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPlayerManaText(int amount)
    {
        playerManaText.text = "Mana: " + amount;
    }

    public void ShowManaWarning()
    {
        manaWarning.SetActive(true);
    }
}
