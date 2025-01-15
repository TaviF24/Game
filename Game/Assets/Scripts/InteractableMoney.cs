using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableMoney : Interactable
{
    public Vector3 rotate;
    void Update()
    {
        this.transform.Rotate(rotate*1*Time.deltaTime);
    }

    protected override void Interact()
    {
        GameManager.instance.player.GetComponent<MoneyCollection>().TakeMoney();
        Destroy(gameObject);
    }
}
