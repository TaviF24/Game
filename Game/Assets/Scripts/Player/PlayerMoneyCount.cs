using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyCollection : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI viewCount;
    public int money = 0;

    public void TakeMoney()
    {
        money=money+10000;
        viewCount.text = money.ToString()+ " $";
    }
}
