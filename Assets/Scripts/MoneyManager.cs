using System;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;
    public int Balance= 0;

    public TextMeshProUGUI balanceUI;

    public static Action OnMoneyChanged;

    public void AddMoney(int amount)
    {
        Balance += amount;
        OnMoneyChanged?.Invoke();
    }
    //Only removes if we are good for the money
    public bool RemoveMoney(int amount)
    {
        if (Balance - amount < 0)
        {
            OnMoneyChanged?.Invoke();
            return false;
        }

        Balance -= amount;
        OnMoneyChanged?.Invoke();
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