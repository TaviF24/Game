using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI promptText;
    public GameObject assaultHUD, anticipationHUD;
    public GameObject finishScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ActivateAssaultHUD(bool value)
    {
        assaultHUD.SetActive(value);
    }

    public void ActivateAnticipationHUD(bool value)
    {
        anticipationHUD.SetActive(value);
    }

    public void UpdateText(string promptMessage)
	{
		promptText.text = promptMessage;
	}
}
