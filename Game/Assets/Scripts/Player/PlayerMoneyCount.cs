using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyCollection : MonoBehaviour, IDataPersistence
{
    [SerializeField]
    public TextMeshProUGUI viewCount;
    public int money = 0;

    public void LoadData(GameData gameData)
    {
        money = gameData.collectedMoney;
        viewCount.text = money.ToString() + " $";
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.collectedMoney = money;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "money")
        {
            money=money+10000;
            viewCount.text = money.ToString()+ " $";
            Destroy(other.gameObject);
        }
    }
}
