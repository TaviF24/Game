using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyCollection : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI viewCount;
    private int Money = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "money")
        {
            Money=Money+10000;
            viewCount.text = Money.ToString()+ " $";
            Destroy(other.gameObject);
        }
    }
}
