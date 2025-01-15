using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealComponent : Interactable
{
    protected override void Interact()
    {
        GameManager.instance.player.GetComponent<PlayerHealth>().RestoreHealth(10);
    }
}
