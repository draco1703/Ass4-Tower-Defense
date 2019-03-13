using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PriceOnTurrent : MonoBehaviour
{
    public Text showPrice;
    public int price;

    private void Start()
    {
        Invoke("Text",  .01f);
    }

   void Text()
    {
        showPrice.text = PlayerStats.CurrencySymbol.ToString() + price;
    }

}
