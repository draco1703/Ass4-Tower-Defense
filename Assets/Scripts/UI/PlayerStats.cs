using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    public static int Money;
    public static string CurrencySymbol ;
    public static int Rounds;
    public static int Lives;


    [Header("optinal")]
    public int startLives = 20;
    public int startMoney = 1000;
    public string currencySymbol = "µ";
    [Header("dev for skip rounds")]
    [HideInInspector]
    public int startRunds = 0;
   


    private void Start()
    {
        Money = startMoney;
        Lives = startLives;
        Rounds = startRunds;
        CurrencySymbol = currencySymbol;

    }


}
