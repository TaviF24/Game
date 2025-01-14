using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : Interactable
{
    public bool isLocked;
    private bool screenIsOn = false;
    void Start()
    {
        isLocked = true;
        promptMessage = "You can not finish the game yet. You need " + (300000 - GameManager.instance.player.GetComponent<MoneyCollection>().money) + "$ more";
    }

    void Update()
    {
        if(!screenIsOn && GameManager.instance.player.GetComponent<MoneyCollection>().money >= 300000)
        {
            isLocked = false;
            promptMessage = "Leave the bank";
        }
        if (screenIsOn && Input.GetKeyDown(KeyCode.Return)) 
        {
            #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
            #else
		            Application.Quit();
            #endif
        }
    }

    protected override void Interact()
    {
        if (isLocked)
        {
            GameManager.instance.player.GetComponent<PlayerUI>().finishScreen.SetActive(true);
            screenIsOn = true;
            Time.timeScale = 0f;
        }
        
    }
}
