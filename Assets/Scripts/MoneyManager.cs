using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;
    public int Balance= 0;

    public TextMeshProUGUI balanceUI;

    public void AddMoney(int amount)
    {
        Balance += amount;
    }
    //Only removes if we are good for the money
    public bool RemoveMoney(int amount)
    {
        if (Balance - amount < 0) return false;

        Balance -= amount;
        return true;
    }
    private void Start()
    {
        instance = this;
    }


    private void Update()
    {
        if(balanceUI != null)
            balanceUI.text = Balance+ "$";
    }


}
