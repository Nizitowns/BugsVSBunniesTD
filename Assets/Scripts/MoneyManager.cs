using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;
    public float Balance= 0;

    public TextMeshProUGUI balanceUI;
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
