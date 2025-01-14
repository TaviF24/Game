using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<NextScene>().isLocked = true;
    }

    void Update()
    {
        if(GameManager.instance.player.GetComponent<MoneyCollection>().money >= 100000)
        {
            gameObject.GetComponent<NextScene>().isLocked = false;
        }
    }
}
